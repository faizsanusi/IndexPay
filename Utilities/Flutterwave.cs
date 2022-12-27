using indexPay.DTO;
using indexPay.Models;
using indexPay.Models.DTOs;
using indexPay.Repositories;
using indexPay.Utilities.IUtilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace indexPay.Utilities
{
    public class Flutterwave : IProvidersStrategy
    {
        private readonly IFlutterWaveAPI _flutterWave;
        private readonly ITransferRepository _transferRepo;

        public Flutterwave(IFlutterWaveAPI flutterWaveAPI, ITransferRepository transferRepository)
        {
            _flutterWave = flutterWaveAPI;
            _transferRepo = transferRepository;
        }
        public async Task<ResponseMessage> banksList()
        {
            try
            {
                var getBanks = await _flutterWave.FlutterwaveGetBanksList();
                if (getBanks is null || getBanks.status != "success" || getBanks.data is null)
                    return new ResponseMessage { Error = true, Description = "Error Occurred While Getting Banks List", ErrorCode = " 99" };

                var responseObj = getBanks.data.Select(x => new ListBankResponseDTO { code = x.code, bankName = x.name, longCode = null }).ToList();
                return new ResponseMessage { Error = false, Data = responseObj };
            }
            catch (Exception)
            {
                return new ResponseMessage { Error = true, Description = "Error Occurred While Getting Banks List", ErrorCode = "99" };
            }

        }

        public async Task<ResponseMessage> validateBankAccount(validateBankAccountDTO request)
        {
            try
            {
                var validateAccount = await _flutterWave.FlutterwaveResolveAccount(new FlutterwaveResolveAccountRequestDTO
                {
                    account_bank = request.code,
                    account_number = request.accountNumber
                });

                if (validateAccount is null || validateAccount.status != "success")
                    return new ResponseMessage { Error = true, Description = "Error Occurred While validation Account Number", ErrorCode = "99" };

                var respObj = new validateAccountNumberResponseDTO
                {
                    accountName = validateAccount.data.account_name,
                    accountNumber = validateAccount.data.account_number,
                    bankCode = request.code,
                    bankName = null
                };
                return new ResponseMessage { Error = false, Data = respObj };
            }
            catch (Exception)
            {
                return new ResponseMessage { Error = true, Description = "Error Occurred While validation Account Number" };
            }
        }


        public async Task<ResponseMessage> getTransactionStatus(string reference)
        {
            try
            {
                var txnStatus = await _flutterWave.FlutterwaveTransactionStatus(reference);

                if (txnStatus is null || txnStatus.status != "success")
                    return new ResponseMessage { Error = true, Description = "Error Occurred While Fetching Transaction Status", ErrorCode = "99" };

                var respObj = new TransactionStatusResponseDTO
                {
                    amount = txnStatus.data.amount,
                    transactionDateTime = txnStatus.data.created_at.ToString(),
                    beneficiaryAccountName = txnStatus.data.full_name,
                    beneficiaryAccountNumber = txnStatus.data.account_number,
                    beneficiaryBankCode = txnStatus.data.bank_code,
                    currencyCode = txnStatus.data.currency,
                    responseCode = "00",
                    responseMessage = txnStatus.data.status,
                    sessionId = txnStatus.data.id.ToString(),
                    status = Status.Success.ToString(),
                    transactionReference = txnStatus.data.id.ToString()
                };
                return new ResponseMessage { Error = false, Data = respObj };
            }
            catch (Exception)
            {
                return new ResponseMessage { Error = true, Description = "Error Occurred While Fetching Transaction Status", ErrorCode = "99" };
            }
        }


        public async Task<ResponseMessage> Transfer(TransferDTO transfer)
        {
            try
            {
                var InitiateTransfer = await _flutterWave.FlutterwaveTransfer(new flutterwaveTransferRequestDTO
                {
                    debit_currency = "NGN",
                    account_bank = transfer.beneficiaryBankCode,
                    account_number = transfer.beneficiaryAccountNumber,
                    amount = transfer.amount,
                    callback_url = transfer.callBackUrl,
                    currency = transfer.currencyCode,
                    narration = transfer.narration,
                    reference = transfer.transactionReference
                });

                if (InitiateTransfer is null || InitiateTransfer.status != "success")
                    return new ResponseMessage { Error = true, Description = "Error Occurred while Initiating Transfer", ErrorCode = "99" };

                //persist transaction to db
                await _transferRepo.CreateTransaction(new TransactionDTO
                {
                    CreatedDate = DateTime.Now,
                    AccountName = transfer.beneficiaryAccountName,
                    AccountNumber = transfer.beneficiaryAccountNumber,
                    Amount = InitiateTransfer.data.amount,
                    BankCode = transfer.beneficiaryBankCode,
                    CurrencyCode = InitiateTransfer.data.currency,
                    ResponseCode = "00",
                    ResponseMessage = InitiateTransfer.message,
                    SessionId = InitiateTransfer.data.id.ToString(),
                    Status = Status.Pending,
                    TransactonRef = InitiateTransfer.data.id.ToString()
                });
                var respObj = new TransferResponseDTO
                {
                    amount = InitiateTransfer.data.amount,
                    transactionDateTime = InitiateTransfer.data.created_at.ToString(),
                    beneficiaryAccountName = transfer.beneficiaryAccountName,
                    beneficiaryAccountNumber = transfer.beneficiaryAccountNumber,
                    beneficiaryBankCode = transfer.beneficiaryBankCode,
                    currencyCode = InitiateTransfer.data.currency,
                    responseCode = "00",
                    responseMessage = InitiateTransfer.message,
                    sessionId = InitiateTransfer.data.id.ToString(),
                    status = Status.Pending.ToString(),
                    transactionReference = InitiateTransfer.data.id.ToString()
                };
                return new ResponseMessage { Error = false, Data = respObj };
            }
            catch (Exception)
            {
                return new ResponseMessage { Error = true, Description = "Error Occurred while Initiating Transfe", ErrorCode = "99" };
            }
        }
    }
}
