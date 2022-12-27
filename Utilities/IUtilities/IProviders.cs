using indexPay.Models.DTOs;
using System.Threading.Tasks;

namespace indexPay.Utilities.IUtilities
{
    public interface IProviders
    {
        Task<ResponseMessage> banksList();
        Task<ResponseMessage> validateBankAccount(validateBankAccountDTO request);
        Task<ResponseMessage> getTransactionStatus(string reference);
        Task<ResponseMessage> Transfer(TransferDTO transfer);
    }
}
