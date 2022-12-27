using indexPay.DTO;
using System.Diagnostics.SymbolStore;

namespace indexPay.Models.DTOs
{
    public class flutterwaveTransactionStatusResponseDTO : flutterwaveResponseDto
    {
        public new txnData data { get; set; }
    }

    public class txnData
    {
        public decimal amount { get; set; }
        public string account_number { get; set; }
        public string full_name { get; set; }
        public string bank_code { get; set; }
        public string reference { get; set; }
        public long id { get; set; }
        public dynamic created_at { get; set; }
        public string currency { get; set; }
        public string complete_message { get; set; }
        public string status { get; set; }
    }
}
