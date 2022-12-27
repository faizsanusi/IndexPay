using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Dapper;
using indexPay.Models;
using indexPay.Models.DTOs;

namespace indexPay.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly IConfiguration _config;
        private readonly string _indexDbConn;

        public TransferRepository(IConfiguration configuration)
        {
            _config = configuration;
            _indexDbConn = _config.GetConnectionString("DBConnection");
        }

        public async Task<int> CreateTransaction(TransactionDTO transaction)
        {
            transaction.CreatedDate = DateTime.UtcNow;
            using (var connection = new SqlConnection(_indexDbConn))
            {
                connection.Open();
                string sql = "insert into Transactions (Amount, AccountNumber, AccountName, BankCode, TransactonRef, CreatedDate, CurrencyCode, ResponseMessage, ResponseCode, SessionId, Status) " +
                    "values (@Amount, @AccountNumber, @AccountName, @BankCode, @TransactonRef, @CreatedDate, @CurrencyCode, @ResponseMessage, @ResponseCode, @SessionId, @Status)";
                var affectedRows = await connection.ExecuteAsync(sql, transaction, commandType: CommandType.Text);
                return affectedRows;
            }
        }
    }
}
