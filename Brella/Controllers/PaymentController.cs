using Data.Entities;
using Data.Repository;
using Dto.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ZarinPal.Class;

namespace Brella.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        #region KeyWords

        private const string error = "error";
        private const string Index = "Index";
        private const string Home = "Home";

        #endregion


        #region Dependency Injction

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<ElementProp> elementPropRepo;
        private readonly IRepository<ContractPayers> contractPayersRepo;
        private readonly IRepository<TransportationPayers> transportationPayersRepo;
        private readonly IRepository<Province> provinceRepo;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public PaymentController(UserManager<ApplicationUser> _userManager, IRepository<ElementProp> _elementPropRepo,
            IRepository<Province> _provinceRepo, IRepository<ContractPayers> _contractPayersRepo, IRepository<TransportationPayers> _transportationPayersRepo)
        {
            userManager = _userManager;
            elementPropRepo = _elementPropRepo;
            contractPayersRepo = _contractPayersRepo;
            transportationPayersRepo = _transportationPayersRepo;
            provinceRepo = _provinceRepo;
        }

        #endregion


        #region Contract

        public IActionResult Contract()
        {
            ViewBag.price = elementPropRepo.Get(x => x.language.faTitle == "فارسی", null, null).Select(x => x.price).FirstOrDefault();

            return View(elementPropRepo.Get(x => x.language.title == lang, null, null).FirstOrDefault());
        }


        public async Task<IActionResult> ContractPayment()
        {
            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            if (contractPayersRepo.Get(null, null, null).Any(x => x.userId == user.Id) == false)
            {
                ElementProp elementProp = elementPropRepo.Get(x => x.language.faTitle == "فارسی", null, null).FirstOrDefault();

                var request = await new Payment().Request(new DtoRequest()
                {
                    Amount = elementProp.price,
                    CallbackUrl = "https://localhost:44335/Payment/ContractPaymentResult",
                    Description = "خرید قرارداد",
                    Email = user.Email,
                    MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                    Mobile = ""
                }, Payment.Mode.sandbox);

                if (request.Status == 100)
                {
                    return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{request.Authority}");
                }
                else
                {
                    TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید.";

                    return RedirectToAction(Index, Home);
                }
            }
            else
            {
                ContractPayers contractPayers = contractPayersRepo.Get(x => x.userId == user.Id, null, null).FirstOrDefault();
                ElementProp elementProp = elementPropRepo.Get(x => x.language.faTitle == "فارسی", null, null).FirstOrDefault();

                ViewBag.price = elementProp.price;

                return View(contractPayers);
            }
        }


        public async Task<IActionResult> ContractPaymentResult(string authority, string status)
        {
            ElementProp elementProp = elementPropRepo.Get(x => x.language.faTitle == "فارسی", null, null).FirstOrDefault();

            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            var verification = await new Payment().Verification(new DtoVerification
            {
                Amount = elementProp.price,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                Authority = authority
            }, Payment.Mode.sandbox);

            if (verification.Status == 100 && status == "OK")
            {
                ContractPayers contractPayers = new()
                {
                    userId = user.Id,
                    trackingCode = verification.RefId,
                    payTime = DateTime.Now,
                    price = elementProp.price,
                    IsAdminConfirmed = false
                };

                contractPayersRepo.Add(contractPayers);
                contractPayersRepo.SaveChange();

                return View(contractPayers);
            }
            else
            {
                TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید.";

                return RedirectToAction(Index, Home);
            }
        }

        #endregion


        #region Transportation

        public IActionResult Transportation()
        {
            ViewData["Provinces"] = provinceRepo.Get(null, null, null);

            return View(elementPropRepo.Get(x => x.language.title == lang, null, null).FirstOrDefault());
        }


        public async Task<IActionResult> TransportationPayment(int provinceId)
        {
            try
            {
                Province province = provinceRepo.Find(provinceId);
                ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

                var request = await new Payment().Request(new DtoRequest()
                {
                    Amount = province.price,
                    CallbackUrl = "https://localhost:44335/Payment/TransportationPaymentResult?provinceId=" + province.id,
                    Description = "هزینه حمل و نقل",
                    Email = user.Email,
                    MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                    Mobile = ""
                }, Payment.Mode.sandbox);

                if (request.Status == 100)
                {
                    return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{request.Authority}");
                }
                else
                {
                    TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید.";

                    return RedirectToAction(Index, Home);
                }
            }
            catch
            {
                TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید.";

                return RedirectToAction(Index, Home);
            }
        }


        public async Task<IActionResult> TransportationPaymentResult(string authority, string status, int provinceId)
        {
            Province province = provinceRepo.Find(provinceId);
            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            var verification = await new Payment().Verification(new DtoVerification
            {
                Amount = province.price,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                Authority = authority
            }, Payment.Mode.sandbox);

            if (verification.Status == 100 && status == "OK")
            {
                TransportationPayers transportationPayers = new()
                {
                    userId = user.Id,
                    payTime = DateTime.Now,
                    trackingCode = verification.RefId,
                    provinceId = province.id,
                    price = province.price,
                    IsAdminConfirmed = false
                };

                transportationPayersRepo.Add(transportationPayers);
                transportationPayersRepo.SaveChange();

                return View(transportationPayers);
            }
            else
            {
                TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید.";

                return RedirectToAction(Index, Home);
            }
        }


        public string FindProvincePrice(int provinceId)
        {
            int provincePrice = provinceRepo.Find(provinceId).price;

            return $"قیمت: <b>{provincePrice:0,0}<b/> تومان";
        }

        #endregion
    }
}
