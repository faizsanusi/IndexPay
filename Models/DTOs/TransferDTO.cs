namespace indexPay.Models.DTOs
{
    public class TransferDTO
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryAccountName { get; set; }
        public string beneficiaryBankCode { get; set; }
        public string transactionReference { get; set; }
        public int maxRetryAttempt { get; set; } = 0;
        public string callBackUrl { get; set; } = null;
        public string provider { get; set; } = null;
    }
}
