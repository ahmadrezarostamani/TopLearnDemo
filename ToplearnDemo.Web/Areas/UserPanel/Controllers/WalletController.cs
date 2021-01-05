using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToplearnDemo.Repository.Repository;
using ToplearnDemo.ViewModels.Wallet;
using ZarinpalSandbox;
namespace ToplearnDemo.Web.Areas.UserPanel.Controllers
{
    [Area("Userpanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private readonly IWalletRepository _walletRepo;
        public WalletController(IWalletRepository walletRepo)
        {
            _walletRepo = walletRepo;
        }


        [Route("[area]/[controller]")]
        public async Task<IActionResult> Index()
        {
            ViewBag.TransactionHistory = await _walletRepo.GetAllUserTransactions(User.Identity.Name);
            return View();
        }

        [Route("[area]/[controller]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ChargeWalletViewModel model)
        {
            ViewBag.TransactionHistory = await _walletRepo.GetAllUserTransactions(User.Identity.Name);
            if (ModelState.IsValid)
            {
                var id=await _walletRepo.ChargeWallet(User.Identity.Name, model.Price, "شارژ کیف پول");
                var payment = new Payment(model.Price);
                var response =await payment.PaymentRequest("شارژ کیف پول", "https://localhost:44395/onlinepayment/"+id);
                if (response.Status==100)
                {
                    Response.Redirect(response.Link);
                }
            }

            return View(model);
        }
    }
}
