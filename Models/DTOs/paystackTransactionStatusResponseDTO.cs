using indexPay.DTO;

namespace indexPay.Models.DTOs
{
    public class paystackTransactionStatusResponseDTO : paystackResponseDto
    {
        public new transactionData data { get; set; }
    }

    public class transactionData
    {
        public decimal amount { get; set; }
        public recipient recipient { get; set; }
        public session session { get; set; }
        public long id { get; set; }
        public dynamic updatedAt { get; set; }
        public dynamic transferred_at { get; set; }
        public string status { get; set; }


    }

    public class session
    {
        public string id { get; set; }
    }

    public class recipient
    {
        public details details { get; set; }
        public string currency { get; set; }
    }

    public class details
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public string bankName { get; set; }
    }
}
