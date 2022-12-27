namespace indexPay.DTO
{
    public class flutterwaveTransferResponseDTO : flutterwaveResponseDto
    {
        public new rsponseData data { get; set; }
    }

    public class rsponseData
    {
        public long id { get; set; }
        public string bank_code { get; set; }
        public string full_name { get; set; }
        public dynamic created_at { get; set; }
        public string currency { get; set; }
        public string debit_currency { get; set; }
        public decimal amount { get; set; }
        public decimal fee { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public dynamic meta { get; set; }
        public string narration { get; set; }
        public string complete_message { get; set; }
        public int requires_approval { get; set; }
        public int is_approved  { get; set; }
        public string bank_name { get; set; }
    }
}
