using System;
using System.Threading.Tasks;

namespace indexPay.Utilities.IUtilities
{
    public interface IMemCache
    {
        object Get(string key);
        void Set(string key, string value, int expiryInSeconds);
    }
}
