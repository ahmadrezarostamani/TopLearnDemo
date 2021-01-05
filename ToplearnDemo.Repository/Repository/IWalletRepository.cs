using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToplearnDemo.ViewModels.Wallet;

namespace ToplearnDemo.Repository.Repository
{
    public interface IWalletRepository
    {
        Task<int> GetUserWalletBalance(string username);
        Task<IEnumerable<TransactionHistoryViewModel>> GetAllUserTransactions(string username);
        Task<int> ChargeWallet(string username, int price, string description, bool isDone=false);
        Task<int> GetTransactionPrice(int id);
        Task<int> UpdateTransaction(int id);
    }
}
