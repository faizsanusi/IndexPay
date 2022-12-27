namespace indexPay.DTO
{
    public class paystackInitiateTransferResponseDTO : paystackResponseDto
    {
        public new responseObj data { get; set; }
    }

    public class responseObj
    {
        public dynamic transfersessionid { get; set; }
        public string live { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string reference { get; set; }
        public string source { get; set; }
        public dynamic source_details { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public long id { get; set; }
        public dynamic failures { get; set; }
        public string transfer_code { get; set; }
        public dynamic titan_code { get; set; }
        public dynamic transferred_at { get; set; }
        public long integration { get; set; }
        public dynamic updatedAt { get; set; }
        public dynamic createdAt { get; set; }
        public long recipient { get; set; }
        public long request { get; set; }
    }
}
