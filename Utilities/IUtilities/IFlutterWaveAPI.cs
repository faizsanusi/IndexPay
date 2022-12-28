using indexPay.DTO;
using indexPay.Models.DTOs;
using System.Threading.Tasks;

namespace indexPay.Utilities.IUtilities
{
    public interface IFlutterWaveAPI
    {
        Task<flutterwaveTransferResponseDTO> FlutterwaveTransfer(flutterwaveTransferRequestDTO request, int retryCount = 0);
        Task<flutterwaveBanksListResponseDTO> FlutterwaveGetBanksList();
        Task<FlutterwaveResolveAccountNumberResponseDTO> FlutterwaveResolveAccount(FlutterwaveResolveAccountRequestDTO request);
        Task<flutterwaveTransactionStatusResponseDTO> FlutterwaveTransactionStatus(string reference);
    }
}
