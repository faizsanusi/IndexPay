using indexPay.Models.DTOs;
using System.Threading.Tasks;

namespace indexPay.Repositories
{
    public interface ITransferRepository
    {
        Task<int> CreateTransaction(TransactionDTO transaction);
    }
}
