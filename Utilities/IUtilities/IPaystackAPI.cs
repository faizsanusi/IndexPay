using indexPay.DTO;
using indexPay.Models.DTOs;
using System.Threading.Tasks;

namespace indexPay.Utilities.IUtilities
{
    public interface IPaystackAPI
    {
        Task<PaystackTransferResponseDTO> PaystackTransfer(paystackTransferRequestDTO transfer);
        Task<paystackInitiateTransferResponseDTO> PaystackInitiateTransfer(paystackInitiateTransferDTO transfer, int retryCount = 0 );
        Task<paystackResolveAccountResponseDTO> PaystackResolveAccountNumber(string accountNumber, string sortCode);
        Task<paystackBanksListResponseDTO> PaystackBankList();
        Task<paystackTransactionStatusResponseDTO> PaystackTransactionStatus(string reference);
    }
}
