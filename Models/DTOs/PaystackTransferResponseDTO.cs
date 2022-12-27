namespace indexPay.DTO
{
    public class PaystackTransferResponseDTO : paystackResponseDto  
    {
        public new transferData data { get; set; }
    }

    public class transferData
    {
        public bool active { get; set; }
        public dynamic createdAt { get; set; }
        public string currency { get; set; }
        public dynamic description { get; set; }
        public string domain { get; set; }
        public string email { get; set; }
        public long id { get; set; }
        public long integration { get; set; }
        public dynamic metadata { get; set; }
        public string name { get; set; }
        public string recipient_code { get; set; }
        public string nuban { get; set; }
        public dynamic updatedAt { get; set; }
        public bool is_deleted { get; set; }
        public bool isDeleted { get; set; }
        public dynamic details { get; set; }
    }
}
