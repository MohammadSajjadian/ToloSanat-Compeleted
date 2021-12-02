using Data.Entities;
using Data.Repository;
using Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.EmailService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.Controllers
{
    public class AccountController : Controller
    {
        #region KeyWords

        private const string success = "success";
        private const string error = "error";
        private const string warning = "warning";
        private const string info = "info";
        private const string Index = "Index";
        private const string Home = "Home";
        private const string Account = "Account";

        #endregion


        #region Deopendency Injections

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IRepository<Group> groupRepo;
        private readonly IRepository<Message> messagesRepo;
        private readonly IRepository<ContractPayers> contractPayersRepo;
        private readonly IRepository<TransportationPayers> transportationPayersRepo;
        private readonly IEmail mail;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager,
            IRepository<Group> _groupRepo, IRepository<Message> _messagesRepo, IRepository<ContractPayers> _contractPayersRepo,
            IRepository<TransportationPayers> _transportationPayersRepo, IEmail _mail)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            groupRepo = _groupRepo;
            messagesRepo = _messagesRepo;
            contractPayersRepo = _contractPayersRepo;
            transportationPayersRepo = _transportationPayersRepo;
            mail = _mail;
        }

        #endregion


        #region Register

        public IActionResult Register()
        {
            return View();
        }


        public async Task<IActionResult> RegisterConfirm(RegisterViewModel model)
        {
            var user = new ApplicationUser()
            {
                UserName = model.userName,
                Email = model.userName,
                name = model.name,
                family = model.family,
                EmailConfirmed = false
            };

            var status = await userManager.CreateAsync(user, model.password);
            if (status.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var address = Url.Action(nameof(AccountConfirm), "Account", new { userId = user.Id, token = token }, "https");

                if (user.name != null && user.family != null)
                    await mail.Send("تایید ایمیل", $"{user.name} {user.family}" +
                        $" عزیز <br/> جهت تایید ایمیل خود و دسترسی به آخرین بروزرسانی های سایت " +
                        $"<a href={address}>اینجا </a> کلیک کنید.", user.Email);
                else
                    await mail.Send("تایید ایمیل", $"کاربر عزیز <br/> جهت تایید ایمیل خود و" +
                        $" دسترسی به آخرین بروزرسانی های سایت <a href={address}>اینجا </a> کلیک کنید.", user.Email);

                user.tokenCreationTime = DateTime.Now;

                await userManager.UpdateAsync(user);

                TempData[info] = "ایمیل تایید حساب کاربری برای شما ارسال شد.";

                return RedirectToAction(Index, Home);
            }
            else
            {
                TempData[error] = "رمزعبور نامعتبر است.";

                return RedirectToAction(nameof(Register));
            }
        }


        public async Task<IActionResult> AccountConfirm(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (DateTime.Now.Subtract(user.tokenCreationTime) <= TimeSpan.FromHours(2))
            {
                var status = await userManager.ConfirmEmailAsync(user, token);
                if (status.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");

                    TempData[success] = "حساب کاربری شما با موفقیت تایید شد.";

                    return RedirectToAction(Index, Home);
                }
                else
                {
                    await userManager.DeleteAsync(user);

                    TempData[error] = "عملیات با شکست مواجه شد.";

                    return RedirectToAction(Index, Home);
                }
            }
            else
            {
                await userManager.DeleteAsync(user);

                TempData[error] = "لینک تایید ایمیل منقضی شده است. دوباره تلاش کنید.";

                return RedirectToAction(nameof(Register));
            }
        }


        public async Task<IActionResult> IsUserExist(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
                return Json(true);
            else
                return Json(false);
        }

        #endregion


        #region Login

        public IActionResult Login()
        {
            return View();
        }


        public async Task<IActionResult> LoginConfirm(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.userName);
            if (user != null && user.EmailConfirmed == true)
            {
                var status = await signInManager.PasswordSignInAsync(user, model.password, model.rememberMe, true);
                if (status.IsLockedOut)
                {
                    TempData[warning] = "به دلیل اشتباه وارد کردن متوالی رمز عبور، اکانت شما به مدت 1 دقیقه غیر فعال خواهد شد.";

                    return RedirectToAction(Index, Home);
                }

                if (status.Succeeded)
                {
                    //if (await userManager.IsInRoleAsync(user, "admin"))
                    //{
                    //    await signInManager.SignOutAsync();

                    //    var address = Url.Action(nameof(AdminConfirm), "Account", new { userId = user.Id }, "https");

                    //    HttpContext.Session.SetString("password", model.password);

                    //    mail.Send("ورود ادمین", $"جهت ورود به حساب کاربری خود <a href={address}>اینجا</a> کلیک کنید.", user.Email);

                    //    TempData[info] = "کد تایید برای شما ارسال شد.";

                    //    return RedirectToAction(Index, Home);
                    //}
                    //else
                    //{
                    TempData[success] = "وارد حساب کاربری خود شدید.";

                    return RedirectToAction(Index, Home);
                    //}
                }
                else
                {
                    TempData[error] = "نام کاربری یا رمزعبور نادرست است.";

                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData[error] = "نام کاربری موجود نمیباشد.";

                return RedirectToAction(nameof(Login));
            }
        }


        public async Task<IActionResult> AdminConfirm(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var password = HttpContext.Session.GetString("password");

            await signInManager.PasswordSignInAsync(user, password, true, false);

            TempData[success] = "وارد حساب کاربری خود شدید.";

            return RedirectToAction(Index, Home);
        }


        public async Task<IActionResult> ForgotPasslvlOne(string email)
        {
            var user = await userManager.FindByNameAsync(email);

            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var address = Url.Action(nameof(ForgotPasslvlTwoBridge), "account", new { userName = user.UserName, token = token }, "https");

                #region Send email by region

                switch (lang)
                {
                    case "fa-IR":
                        await mail.Send("تغییر رمز عبور", $"برای تغییر رمز عبور <a href={address}>اینجا</a> کلیک کنید.", user.Email);
                        break;

                    case "en-US":
                        await mail.Send("Change Password", $"Click <a href={address}>here</a> To change the password.", user.Email);
                        break;

                    case "ar-AE":
                        await mail.Send("غير كلمة السر", $"انقر <a href={address}>هنا</a> لتغيير كلمة مرورك.", user.Email);
                        break;

                    case "it-IT":
                        await mail.Send("cambia la password", $"Fare clic <a href={address}>qui</a> per modificare la password.", user.Email);
                        break;
                }

                #endregion

                user.forgotPassTimeSpan = DateTime.Now;

                await userManager.UpdateAsync(user);

                TempData[info] = "پیامک تغییر رمز عبور برای شما ارسال شد.";
            }
            else
            {
                TempData[error] = "حساب کاربری با این ایمیل تلفن موجود نمی باشد.";

            }
            return RedirectToAction(nameof(Login));
        }


        public IActionResult ForgotPasslvlTwoBridge(string userName, string token)
        {
            TempData["userName"] = userName;
            TempData["token"] = token;

            return RedirectToAction(nameof(ForgotPasslvlTwo));
        }


        public async Task<IActionResult> ForgotPasslvlTwo()
        {
            var userName = TempData["userName"].ToString();
            var token = TempData["token"].ToString();

            var user = await userManager.FindByNameAsync(userName);
            if (DateTime.Now.Subtract(user.forgotPassTimeSpan) > TimeSpan.FromMinutes(3))
            {
                TempData[error] = "کاربر گرامی، اعتبار این لینک 3 دقیقه است. لطفا دوباره تلاش کنید.";

                return RedirectToAction(nameof(Index), Home);
            }

            HttpContext.Session.SetString("userName", userName);
            HttpContext.Session.SetString("token", token);

            return View();
        }


        public async Task<IActionResult> ForgotPasslvlThree(string newPassword)
        {
            var userName = HttpContext.Session.GetString("userName");
            var token = HttpContext.Session.GetString("token");

            var user = await userManager.FindByNameAsync(userName);

            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                TempData[success] = "رمز عبور شما با موفقیت تغییر یافت.";

                return RedirectToAction(nameof(Index), Home);
            }
            else
            {
                TempData[error] = "تغییر رمز عبور با شکست مواجه شد.";

                return RedirectToAction(nameof(Index), Home);
            }

        }


        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(Index, Home);
        }

        #endregion


        #region Chats

        public async Task<IActionResult> Support()
        {
            if (!User.IsInRole("admin"))
            {
                ApplicationUser client = await userManager.FindByNameAsync(User.Identity.Name);

                int groupId = groupRepo.Get(x => x.userId == client.Id, null, null).Select(x => x.id).FirstOrDefault();

                List<Message> messages = messagesRepo.Get(x => x.groupId == groupId, null, "applicationUser");

                return View(messages);
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion


        #region Purchases

        public async Task<IActionResult> Purchases()
        {
            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            List<TransportationPayers> transportationPayers = transportationPayersRepo.Get(x => x.userId == user.Id,
                x => x.OrderByDescending(x => x.id), "province");

            ViewData["TransportationPayers"] = transportationPayers;
            ViewBag.TransportationPayersCount = transportationPayers.Count;

            return View(contractPayersRepo.Get(x => x.userId == user.Id, null, null).FirstOrDefault());
        }

        #endregion
    }
}
