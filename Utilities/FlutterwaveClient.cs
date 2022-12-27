using indexPay.Utilities.IUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static indexPay.Startup;

namespace indexPay.Utilities
{
    public class FlutterwaveClient : IFlutterwaveClient
    {
        //private static readonly RestClient _flutterwaveClient;
        private readonly IConfiguration _config;
        private readonly string _flutterWaveSecretKey;
        private readonly RestClient _flutterwaveClient;


        public FlutterwaveClient(IConfiguration configuration)
        {
            _config = configuration;
            _flutterWaveSecretKey = _config.GetSection("FlutterwaveSecretKeyLive").Value;
            var options = new RestClientOptions(_config.GetSection("FlutterwaveUrl").Value)
            {
                ThrowOnAnyError = false
            };
            _flutterwaveClient = new RestClient();
        }


        public async Task<T> GetAsync<T>(string Uri, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null)
        {
            var request = new RestRequest(Uri);
            request.AddHeader("Authorization", "Bearer " + _flutterWaveSecretKey);
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

            
            return await _flutterwaveClient.GetAsync<T>(request);
        }

        public async Task<T> GetAsync<T>(string Uri)
        {
            var request = new RestRequest(Uri);
            request.AddHeader("Authorization", "Bearer " + _flutterWaveSecretKey);

            return await _flutterwaveClient.GetAsync<T>(request);
        }

        public async Task<T> GetAsync<T>(string Uri, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null, IDictionary<string, string> pathVariables = null)
        {
            var request = new RestRequest(Uri);
            request.AddHeader("Authorization", "Bearer " + _flutterWaveSecretKey);
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

            if (pathVariables is not null)
            {
                foreach (var pathVariable in pathVariables)
                {
                    request.AddUrlSegment(pathVariable.Key, pathVariable.Value);
                }
            }


            return await _flutterwaveClient.GetAsync<T>(request);
        }

        public async Task<T> PostAsync<T>(string Uri, object data, int retryCount = 0)
        {
            int count = 0;
            int BackoffRate = 1000;   // in milliseconds

            var request = new RestRequest(Uri);
            request.AddJsonBody(data);
            request.AddHeader("Authorization", "Bearer " + _flutterWaveSecretKey);
            request.Method = Method.Post;
            var response = await _flutterwaveClient.ExecuteAsync<T>(request);

            //exponential retry implementation when server error occurs from provider
            var getResponsecode = (int)response.StatusCode;
            if (getResponsecode.ToString().Substring(0, 1) == "5")
            {
                while (count <= retryCount && getResponsecode.ToString().Substring(0, 1) == "5")
                {
                    count++;
                    Debug.WriteLine("Attempting Request. Count: " + count);
                    Thread.Sleep((count ^ 2) * BackoffRate); // Exponential Backoff
                    response = await _flutterwaveClient.ExecuteAsync<T>(request);
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
