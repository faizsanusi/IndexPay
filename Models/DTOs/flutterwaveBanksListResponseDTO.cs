using indexPay.DTO;
using System.Collections.Generic;

namespace indexPay.Models.DTOs
{
    public class flutterwaveBanksListResponseDTO : flutterwaveResponseDto
    {
        public new IEnumerable<banksList> data { get; set; }
    }

    public class banksList
    {
        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}
