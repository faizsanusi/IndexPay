
namespace indexPay.DTO
{
    public class paystackInitiateTransferDTO : paystackTransferRequestDTO
    {
        public initiateTransfer initiateTransfer { get; set; }
    }

    public class initiateTransfer
    {
        public string source { get; set; }
        public string reason { get; set; }
        public decimal amount { get; set; }
        public string recipient { get; set; }
        public string reference { get; set; }
    }
}
