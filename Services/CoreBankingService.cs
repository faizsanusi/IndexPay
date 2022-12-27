using indexPay.Models.DTOs;
using indexPay.Repositories;
using indexPay.Services.IServices;
using indexPay.Utilities;
using indexPay.Utilities.IUtilities;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace indexPay.Services
{

    public class CoreBankingService : ICoreBankingServices
    {
        private readonly IConfiguration _config;
        private readonly IFlutterWaveAPI _flutterwave;
        private readonly IPaystackAPI _paystack;
        private readonly ITransferRepository _transferRepo;

        public CoreBankingService(IConfiguration configuration, IFlutterWaveAPI flutterWaveAPI, IPaystackAPI paystackAPI, ITransferRepository transferRepository)
        {
            _config = configuration;
            _flutterwave = flutterWaveAPI;
            _paystack = paystackAPI;
            _transferRepo = transferRepository;
        }

        private dynamic providerStrategy(string provider)
        {
            if (string.IsNullOrEmpty(provider))
                provider = _config.GetSection("DefualtProvider").Value;

            // provider strategy pattern implementation
            dynamic initiateRequest = null;
            switch (provider)
            {
                default:
                    break;
                case "flutterwave":
                    initiateRequest = new Providers(new Flutterwave(_flutterwave, _transferRepo));
                    break;
                case "paystack":
                    initiateRequest = new Providers(new Paystack(_paystack, _transferRepo));
                    break;
            }

            return initiateRequest;
        }
        public async Task<ResponseMessage> getBanksList(string provider)
        {
            var request = providerStrategy(provider);

            var banksList = await request.banksList();

            if (banksList == null || banksList.Error)
                return new ResponseMessage { Error = true };
            return new ResponseMessage { Error = false, Data = banksList.Data };
        }


        public async Task<ResponseMessage> validateBankAccount(validateBankAccountDTO validator)
        {
            var request = providerStrategy(null);

            var validateAccount = await request.validateBankAccount(validator);

            if (validateAccount == null || validateAccount.Error)
                return new ResponseMessage { Error = true };
            return new ResponseMessage { Error = false, Data = validateAccount.Data };
        }


        public async Task<ResponseMessage> getTransctionStatus(string provider, string reference)
        {
            var request = providerStrategy(provider);

            var validateAccount = await request.getTransactionStatus(reference);

            if (validateAccount == null || validateAccount.Error)
                return new ResponseMessage { Error = true };
            return new ResponseMessage { Error = false, Data = validateAccount.Data };
        }


        public async Task<ResponseMessage> Transfer(TransferDTO transfer)
        {

            var request = providerStrategy(transfer.provider);

            //run transfer asynchronously on the background
            _ = Task.Run(async () =>
            {
                await request.Transfer(transfer);
            });

            return new ResponseMessage { Error = false, Description = "Transfer Request Successfully Sent" };
        }
    }
}
