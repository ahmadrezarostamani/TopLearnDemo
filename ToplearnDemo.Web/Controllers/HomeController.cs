using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToplearnDemo.Repository.Repository;
using ToplearnDemo.ViewModels.Wallet;
using ZarinpalSandbox;
namespace ToplearnDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWalletRepository _walletRepo;
        public HomeController(IWalletRepository walletRepo)
        {
            _walletRepo = walletRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]/{id}")]
        public async Task<IActionResult> OnlinePayment(int id)
        {
            var model = new PaymentResultViewModel()
            {
                Status = false,
                PaymentCode=null               
            };
            if (HttpContext.Request.Query["Status"]=="OK" && HttpContext.Request.Query["Authority"]!="")
            {               
                string authority = HttpContext.Request.Query["Authority"];
                int price = await _walletRepo.GetTransactionPrice(id);
                if (price == -1)
                {
                    model.ErrorMessage = "تراکنش مورد نظر یافت نشد.";
                    return View(model);
                }
                var payment = new Payment(price);
                var response = await payment.Verification(authority);
                if (response.Status == 100)
                {                    
                    await _walletRepo.UpdateTransaction(id);
                    model.Status = true;
                    model.PaymentCode = response.RefId.ToString();
                }
            }


            return View(model);
        }
    }
}
