﻿using Data.Entities;
using Data.Repository;
using Dto.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.EmailService;
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

        #endregion


        #region Dependency Injction

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<ElementProp> elementPropRepo;
        private readonly IRepository<Order> orderRepo;
        private readonly IEmail email;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public PaymentController(UserManager<ApplicationUser> _userManager, IRepository<ElementProp> _elementPropRepo,
            IRepository<Order> _orderRepo, IEmail _email)
        {
            userManager = _userManager;
            elementPropRepo = _elementPropRepo;
            orderRepo = _orderRepo;
            email = _email;
        }

        #endregion


        #region Payment

        public IActionResult Order()
        {
            return View(elementPropRepo.Get(x => x.language.title == lang, null, null).FirstOrDefault());
        }


        public async Task<IActionResult> OrderPayment()
        {
            try
            {
                ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

                var request = await new Payment().Request(new DtoRequest()
                {
                    Amount = user.price,
                    CallbackUrl = "https://localhost:44335/Payment/OrderPaymentResult",
                    Description = " هزینه تصویب شده از طرف مدیریت",
                    Email = user.Email,
                    MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                    Mobile = user.PhoneNumber
                }, Payment.Mode.sandbox);

                if (request.Status == 100)
                {
                    return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{request.Authority}");
                }
                else
                {
                    #region Multi language TempData

                    switch (lang)
                    {
                        case "fa-IR":
                            TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید";
                            break;

                        case "en-US":
                            TempData[error] = "Payment was encountered. Please try again";
                            break;

                        case "ar-AE":
                            TempData[error] = "تمت مصادفة الدفع. حاول مرة اخرى";
                            break;

                        case "it-IT":
                            TempData[error] = "È stato riscontrato il pagamento. Per favore riprova";
                            break;

                        case "tr-TR":
                            TempData[error] = "Ödeme ile karşılaşıldı. Lütfen tekrar deneyin";
                            break;
                    }

                    #endregion

                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید";
                        break;

                    case "en-US":
                        TempData[error] = "Payment was encountered. Please try again";
                        break;

                    case "ar-AE":
                        TempData[error] = "تمت مصادفة الدفع. حاول مرة اخرى";
                        break;

                    case "it-IT":
                        TempData[error] = "È stato riscontrato il pagamento. Per favore riprova";
                        break;

                    case "tr-TR":
                        TempData[error] = "Ödeme ile karşılaşıldı. Lütfen tekrar deneyin";
                        break;
                }

                #endregion

                return RedirectToAction("Index", "Home");
            }
        }


        public async Task<IActionResult> OrderPaymentResult(string authority, string status)
        {
            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            var verification = await new Payment().Verification(new DtoVerification
            {
                Amount = user.price,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                Authority = authority
            }, Payment.Mode.sandbox);

            if (verification.Status == 100 && status == "OK")
            {
                Order ord = new()
                {
                    userId = user.Id,
                    payTime = DateTime.Now,
                    trackingCode = verification.RefId,
                    price = user.price,
                    IsAdminConfirmed = false
                };

                orderRepo.Add(ord);
                orderRepo.SaveChange();

                string address = Url.Action("Orders", "Admin", new { pagenumber = 1 }, "https");

                foreach (var item in userManager.Users.ToList())
                {
                    if (await userManager.IsInRoleAsync(item, "admin"))
                    {
                        await email.Send("ثبت خرید", $"<p style='font-size: 17px'>خرید جدیدی انجام شد</p><br><p style='font-size: 17px'>جهت مشاهده<a style='color: #e67e23' href='{address}'><b>(اینجا)</b></a> کلیک کنید</p>", item.Email);
                    }
                }

                return View(ord);
            }
            else
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "پرداخت با مشکل مواجه شد. لطفا دوباره تلاش کنید";
                        break;

                    case "en-US":
                        TempData[error] = "Payment was encountered. Please try again";
                        break;

                    case "ar-AE":
                        TempData[error] = "تمت مصادفة الدفع. حاول مرة اخرى";
                        break;

                    case "it-IT":
                        TempData[error] = "È stato riscontrato il pagamento. Per favore riprova";
                        break;

                    case "tr-TR":
                        TempData[error] = "Ödeme ile karşılaşıldı. Lütfen tekrar deneyin";
                        break;
                }

                #endregion

                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}
