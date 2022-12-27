namespace indexPay.Models.DTOs
{
    public class ResponseMessage
    {
        public bool Error { get; set; }
        public string ErrorCode { get; set; }
        public string Description { get; set; }
        public object Data { get; set; }
    }
}
