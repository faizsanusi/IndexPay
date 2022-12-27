namespace indexPay.Models.DTOs
{
    public class TransactionStatusResponseDTO
    {
        public decimal amount { get; set; }
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryAccountName { get; set; }
        public string beneficiaryBankCode { get; set; }
        public string transactionReference { get; set; }
        public dynamic transactionDateTime { get; set; }
        public string currencyCode { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public string sessionId { get; set; }
        public string status { get; set; }
    }
}
