using indexPay.Models.DTOs;
using indexPay.Utilities.IUtilities;
using System.Threading.Tasks;

namespace indexPay.Utilities
{
    public class Providers : IProviders
    {
        private readonly IProvidersStrategy providersStrategy;

        public Providers(IProvidersStrategy providersStrategy)
        {
            this.providersStrategy = providersStrategy;
        }

        public async Task<ResponseMessage> banksList()
        {
            return await providersStrategy.banksList(); 
        }

        public async Task<ResponseMessage> getTransactionStatus(string reference)
        {
            return await providersStrategy.getTransactionStatus(reference);
        }

        public async Task<ResponseMessage> Transfer(TransferDTO transfer)
        {
            return await providersStrategy.Transfer(transfer);  
        }

        public async Task<ResponseMessage> validateBankAccount(validateBankAccountDTO request)
        {
            return await providersStrategy.validateBankAccount(request);
        }
    }
}
