using indexPay.DTO;
using indexPay.Models.DTOs;
using indexPay.Utilities.IUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace indexPay.Utilities
{
    public class PaystackAPI : IPaystackAPI
    {
        private readonly IPaystackClient _payStackClient;
        private readonly IConfiguration _config;

        public PaystackAPI(IConfiguration configuration,IPaystackClient paystackClient)
        {
            _payStackClient = paystackClient;
            _config = configuration;
        }
      

        public async Task<paystackResolveAccountResponseDTO> PaystackResolveAccountNumber(string accountNumber, string sortCode)
        {
            try
            {

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("account_number", accountNumber);
                parameters.Add("bank_code", sortCode);
                //send to paystack
                var response = await _payStackClient.GetAsync<paystackResolveAccountResponseDTO>("bank/resolve", null, parameters);
                return response;
            }
            catch (Exception)
            {
                return new paystackResolveAccountResponseDTO { status = false, message = "Unable to resolve account name" };
            }
        }

        public async Task<paystackTransactionStatusResponseDTO> PaystackTransactionStatus(string reference)
        {
            try
            {
                //send to paystack
                var response = await _payStackClient.GetAsync<paystackTransactionStatusResponseDTO>($"transfer/{reference}");
                return response;
            }
            catch (Exception)
            {
                return new paystackTransactionStatusResponseDTO { status = false, message = "Unable to get transaction details" };
            }
        }

        public async Task<paystackBanksListResponseDTO> PaystackBankList()
        {
            try
            {
                //send to paystack
                var response = await _payStackClient.GetAsync<paystackBanksListResponseDTO>("bank", null);
                return response;
            }
            catch (Exception)
            {
                return new paystackBanksListResponseDTO { status = false, message = "Unable to get banks list" };
            }
        }


        public async Task<PaystackTransferResponseDTO> PaystackTransfer(paystackTransferRequestDTO transfer)
        {
            try
            {

                //forward request to paystack
                string payStackUrl = _config.GetSection("PayStackUrl").Value + $"transferrecipient";
                //build headers
                IDictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", "Bearer " + _config.GetSection("PayStackSecretkey").Value);

                var response = await _payStackClient.PostAsync<PaystackTransferResponseDTO>("transferrecipient",transfer,headers);

                return response;
            }
            catch (Exception)
            {

                return new PaystackTransferResponseDTO { status = false, message = "Error Occured while making request to paystack" };

            }
        }

     
        public async Task<paystackInitiateTransferResponseDTO> PaystackInitiateTransfer(paystackInitiateTransferDTO transfer, int retryCount = 0)
        {
            try
            {
                {
                    //call PaystackTransferAPI
                    var paystack = await _payStackClient.PostAsync<PaystackTransferResponseDTO>("transferrecipient", new paystackTransferRequestDTO
                    {
                        type = transfer.type,
                        name = transfer.name,
                        account_number = transfer.account_number,
                        bank_code = transfer.bank_code,
                        currency = transfer.currency
                    }, retryCount);

                    if (!paystack.status || (paystack.data.recipient_code is null))
                    {
                        return new paystackInitiateTransferResponseDTO { status = false, message = "Error Occured while making request to paystack" };
                    }
                    //forward request to paystack
                    transfer.initiateTransfer.recipient = paystack.data.recipient_code;
                    var JsonResponse = await _payStackClient.PostAsync<paystackInitiateTransferResponseDTO>("transfer", transfer.initiateTransfer, retryCount);

                    return JsonResponse;
                }
            }
            catch (Exception)
            {
                return new paystackInitiateTransferResponseDTO { status = false, message = "Error Occured while making request to paystack" };
            }
        }
    }
}
