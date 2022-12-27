using indexPay.DTO;
using System.Collections.Generic;

namespace indexPay.Models.DTOs
{
    public class paystackBanksListResponseDTO : paystackResponseDto
    {
        public new IEnumerable<paystackBanksData> data { get; set; }

    }

    public class paystackBanksData
    {
        public long id { get; set; }
        public string name { get; set; }
        public string longcode { get; set; }
        public string code { get; set; }
    }
}
