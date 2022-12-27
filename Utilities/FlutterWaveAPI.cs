using indexPay.DTO;
using indexPay.Models.DTOs;
using indexPay.Utilities.IUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indexPay.Utilities
{
    public class FlutterWaveAPI : IFlutterWaveAPI
    {
        private readonly IFlutterwaveClient _flutterwaveClient;

        public FlutterWaveAPI(IFlutterwaveClient flutterwaveClient)
        {
            _flutterwaveClient = flutterwaveClient;
        }


        public async Task<flutterwaveTransferResponseDTO> FlutterwaveTransfer(flutterwaveTransferRequestDTO request)
        {
            try
            {
                var JsonResponse = await _flutterwaveClient.PostAsync<flutterwaveTransferResponseDTO>("https://api.flutterwave.com/v3/transfers", request, 0);
                return JsonResponse;
            }
            catch (Exception)
            {
                return new flutterwaveTransferResponseDTO { status = "Error", message = "Error Occured while making request to Flutterwave" };
            }
        }

        public async Task<flutterwaveBanksListResponseDTO> FlutterwaveGetBanksList()
        {
            try
            {
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("country", "NG");
                var JsonResponse = await _flutterwaveClient.GetAsync<flutterwaveBanksListResponseDTO>("https://api.flutterwave.com/v3/banks/NG");
                return JsonResponse;
            }
            catch (Exception)
            {
                return new flutterwaveBanksListResponseDTO { status = "Error", message = "Error Occured while making request to Flutterwave" };
            }
        }

        public async Task<flutterwaveTransactionStatusResponseDTO> FlutterwaveTransactionStatus(string reference)
        {
            try
            {
                var JsonResponse = await _flutterwaveClient.GetAsync<flutterwaveTransactionStatusResponseDTO>($"https://api.flutterwave.com/v3/transfers/{reference}");
                return JsonResponse;
            }
            catch (Exception)
            {
                return new flutterwaveTransactionStatusResponseDTO { status = "Error", message = "Error Occured while making request to Flutterwave" };
            }
        }


        public async Task<FlutterwaveResolveAccountNumberResponseDTO> FlutterwaveResolveAccount(FlutterwaveResolveAccountRequestDTO request)
        {
            try
            {
                var JsonResponse = await _flutterwaveClient.PostAsync<FlutterwaveResolveAccountNumberResponseDTO>("https://api.flutterwave.com/v3/accounts/resolve", request, 0);
                return JsonResponse;
            }
            catch (Exception)
            {
                return new FlutterwaveResolveAccountNumberResponseDTO { status = "Error", message = "Error Occured while making request to Flutterwave" };
            }
        }
    }
}
