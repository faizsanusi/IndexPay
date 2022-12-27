using indexPay.DTO;

namespace indexPay.Models.DTOs
{
    public class FlutterwaveResolveAccountNumberResponseDTO : flutterwaveResponseDto
    {
        public new data data { get; set; }
    }

    public class data
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
    }
}
