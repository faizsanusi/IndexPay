namespace indexPay.DTO
{
    public class paystackResolveAccountResponseDTO : paystackResponseDto
    {
        public new paystackData data { get; set; }
    }

    public class paystackData
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public dynamic bank_id { get; set; }
    }
}
