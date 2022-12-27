using indexPay.Utilities.IUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static indexPay.Startup;

namespace indexPay.Utilities
{
    public class PaystackClient : IPaystackClient
    {

        private readonly IConfiguration _config;
        private readonly string _paystackSecretKey;
        private readonly RestClient _paystackClient;

        public PaystackClient(IConfiguration configuration)
        {
            _config = configuration;
            _paystackSecretKey = _config.GetSection("PayStackSecretkey").Value;
            var options = new RestClientOptions(_config.GetSection("PayStackUrl").Value)
            {
                ThrowOnAnyError = false
            };
            _paystackClient = new RestClient(options);

        }


        public async Task<T> GetAsync<T>(string Uri, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null)
        {
            var request = new RestRequest(Uri);
            request.AddHeader("Authorization", "Bearer " + _paystackSecretKey);
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if (parameters is not null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            return await _paystackClient.GetAsync<T>(request);
        }

        public async Task<T> GetAsync<T>(string Uri)
        {
            //  _paystackClient.AddDefaultHeader("Authorization", "Bearer " +_paystackSecretKey);

            var request = new RestRequest(Uri);
            request.AddHeader("Authorization", "Bearer " + _paystackSecretKey);

            return await _paystackClient.GetAsync<T>(request);
        }

        public async Task<T> PostAsync<T>(string Uri, object data, int retryCount = 0)
        {
            int count = 0;
            int BackoffRate = 1000;   // in milliseconds

            var request = new RestRequest(Uri);
            request.AddJsonBody(data);
            request.AddHeader("Authorization", "Bearer " + _paystackSecretKey);
            request.Method = Method.Post;
            var response = await _paystackClient.ExecuteAsync<T>(request);

            //exponential retry implementation when server error occurs from provider
            var getResponsecode = (int)response.StatusCode;
            if (getResponsecode.ToString().Substring(0,1) == "5")
            {
                while (count <= retryCount && getResponsecode.ToString().Substring(0, 1) == "5")
                {
                    count++;
                    Debug.WriteLine("Attempting Request. Count: " + count);
                    Thread.Sleep((count ^ 2) * BackoffRate); // Exponential Backoff
                    response = await _paystackClient.ExecuteAsync<T>(request);
                }
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }


        public Task<T> PostAsync<T>(string Uri, object data, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null)
        {
            throw new System.NotImplementedException();
        }

    }
}
