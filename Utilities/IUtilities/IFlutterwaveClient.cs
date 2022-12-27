using System.Collections.Generic;
using System.Threading.Tasks;

namespace indexPay.Utilities.IUtilities
{
    public interface IFlutterwaveClient : IRestClient
    {
        Task<T> GetAsync<T>(string Uri, IDictionary<string, string> headers = null, IDictionary<string, string> parameters = null, IDictionary<string, string> pathVariables = null);
    }
}
