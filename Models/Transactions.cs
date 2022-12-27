using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace indexPay.Models
{
    public class Transactions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Range(typeof(decimal),
"-79228162514264337593543950335",
"79228162514264337593543950335",
ErrorMessage = "There's an error")]
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankCode { get; set; }
        public string TransactonRef { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CurrencyCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public string SessiongId { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Created,
        Pending,
        Success,
        Failure,
        Retry

    }
}
