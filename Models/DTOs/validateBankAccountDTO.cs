namespace indexPay.Models.DTOs
{
    public class validateBankAccountDTO
    {
        public string code { get; set; }
        public string accountNumber { get; set; }
        public string provider { get; set; } = null;
    }
}
