using indexPay.Models.DTOs;
using System.Threading.Tasks;

namespace indexPay.Services.IServices
{
    public interface ICoreBankingServices
    {
        Task<ResponseMessage> getBanksList(string provider);
        Task<ResponseMessage> validateBankAccount(validateBankAccountDTO request);
        Task<ResponseMessage> getTransctionStatus(string provider, string reference);
        Task<ResponseMessage> Transfer(TransferDTO transfer);
    }
}
