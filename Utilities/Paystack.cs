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
    public class Paystack : IProvidersStrategy
    {
        private readonly IPaystackAPI _paystack;
        private readonly ITransferRepository _transferRepo;

        public Paystack(IPaystackAPI paystackAPI, ITransferRepository transferRepository)
        {
            _paystack = paystackAPI;
            _transferRepo = transferRepository;
        }
        public async Task<ResponseMessage> banksList()
        {
            var getBanks = await _paystack.PaystackBankList();
            if (getBanks is null || !getBanks.status)
                return new ResponseMessage { Error = true, Description = "Error Occurred While Getting Banks List", ErrorCode = "99" };

            var responseObj = getBanks.data.Select(x => new ListBankResponseDTO { code = x.code, bankName = x.name, longCode = x.longcode }).ToList();
            return new ResponseMessage { Error = false, Data = responseObj };
        }

        public async Task<ResponseMessage> validateBankAccount(validateBankAccountDTO request)
        {
            try
            {
                var validateAccount = await _paystack.PaystackResolveAccountNumber(request.accountNumber, request.code);

                if (validateAccount is null || !validateAccount.status)
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
                var txnStatus = await _paystack.PaystackTransactionStatus(reference);

                if (txnStatus is null || !txnStatus.status)
                    return new ResponseMessage { Error = true, Description = "Error Occurred While Fetching Transaction Status", ErrorCode = "99" };

                var respObj = new TransactionStatusResponseDTO
                {
                    amount = txnStatus.data.amount,
                    transactionDateTime = txnStatus.data.transferred_at.ToString(),
                    beneficiaryAccountName = txnStatus.data.recipient.details.account_name,
                    beneficiaryAccountNumber = txnStatus.data.recipient.details.account_number,
                    beneficiaryBankCode = txnStatus.data.recipient.details.bank_code,
                    currencyCode = txnStatus.data.recipient.currency,
                    responseCode = "00",
                    responseMessage = txnStatus.data.status,
                    sessionId = txnStatus.data.session.id,
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
                var InitiateTransfer = await _paystack.PaystackInitiateTransfer(new paystackInitiateTransferDTO
                {
                    account_number = transfer.beneficiaryAccountNumber,
                    bank_code = transfer.beneficiaryBankCode,
                    currency = transfer.currencyCode,
                    name = transfer.beneficiaryAccountName,
                    type = "nuban",
                    initiateTransfer = new initiateTransfer
                    {
                        amount = transfer.amount,
                        reason = transfer.narration,
                        source = "balance",
                        reference = transfer.transactionReference
                    }
                }, transfer.maxRetryAttempt);

                if (InitiateTransfer is null || !InitiateTransfer.status)
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
                    transactionDateTime = InitiateTransfer.data.transferred_at.ToString(),
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
