using System.Collections.Generic;
using System.Threading.Tasks;

namespace indexPay.Utilities.IUtilities
{
    public interface IRestClient
    {
        Task<T> PostAsync<T>(string Uri, object data, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null);
        Task<T> GetAsync<T>(string Uri, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null);
        Task<T> GetAsync<T>(string Uri);
        Task<T> PostAsync<T>(string Uri, object data, int retryCount = 0);
    }
}
