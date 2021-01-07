using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToplearnDemo.DataLayer.Context;
using ToplearnDemo.DomainClassess.Transaction;
using ToplearnDemo.Repository.Repository;
using ToplearnDemo.ViewModels.Wallet;

namespace ToplearnDemo.Repository.Service
{
    public class WalletRepository:IWalletRepository
    {
        private readonly ToplearnDemoContext _db;
        public WalletRepository(ToplearnDemoContext db)
        {
            _db = db;           
        }

        public async Task<IEnumerable<TransactionHistoryViewModel>> GetAllUserTransactions(string username)
        {
            var user = await _db.Users.FirstAsync(user => user.Username == username);
            var transactions = await _db.Transactions.Where(t => t.UserId == user.UserId && t.IsDone).Select(t => new TransactionHistoryViewModel
            {
                PaidDate = t.PaidDate,
                Description = t.Description,
                Price = t.Price,
                TransactionType = t.TransactionTypeId
            }).ToListAsync();

            return transactions;
        }

        public async Task<int> GetUserWalletBalance(string username)
        {
            var user = await _db.Users.FirstAsync(user => user.Username == username);

            var deposit = _db.Transactions.Where(t => t.TransactionTypeId == 1 && t.IsDone && t.UserId == user.UserId).Select(t=>t.Price).ToList();
            var withdraw = _db.Transactions.Where(t => t.TransactionTypeId == 2 && t.UserId == user.UserId).Select(t=>t.Price).ToList();

            return deposit.Sum() - withdraw.Sum();
        }

        public async Task<int> ChargeWallet(string username, int price, string description, bool isDone=false)
        {
            var user = await _db.Users.FirstAsync(user => user.Username == username);
            var wallet = new Transaction()
            {
                UserId=user.UserId,
                TransactionTypeId=1,
                Price=price,
                Description=description,
                IsDone=isDone,
                PaidDate=DateTime.Now
            };
            await _db.Transactions.AddAsync(wallet);
            await _db.SaveChangesAsync();
            return wallet.TransactionId;
        }

        public async Task<int> GetTransactionPrice(int id)
        {
            var wallet = await _db.Transactions.FindAsync(id);
            if (wallet != null)
                return wallet.Price;
            return -1;
        }

        public async Task<int> UpdateTransaction(int id)
        {
            var wallet =await  _db.Transactions.FindAsync(id);
            wallet.IsDone = true;
            return await _db.SaveChangesAsync();
        }
    }
}
