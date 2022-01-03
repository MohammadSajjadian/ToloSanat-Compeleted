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
        private const string siteName = "www.tolosanat.com";

        #endregion


        #region Deopendency Injections

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IRepository<Group> groupRepo;
        private readonly IRepository<Message> messagesRepo;
        private readonly IRepository<Order> orderRepo;
        private readonly IEmail mail;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager,
            IRepository<Group> _groupRepo, IRepository<Message> _messagesRepo, IRepository<Order> _orderRepo, IEmail _mail)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            groupRepo = _groupRepo;
            messagesRepo = _messagesRepo;
            orderRepo = _orderRepo;
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
                PhoneNumber = model.phoneNumber,
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

                #region Multi language Email

                string subject = "";
                string message = "";

                switch (lang)
                {
                    case "fa-IR":
                        subject = "تایید ایمیل";
                        message = $"{user.name} {user.family} عزیز <br/> جهت تایید ایمیل خود و دسترسی به آخرین بروزرسانی های سایت <a href={address}>اینجا</a> کلیک کنید.<br/>{siteName}";
                        break;

                    case "en-US":
                        subject = "Email confirmation";
                        message = $"{user.name} {user.family} Dear <br/> Click <a href={address}> here </a> to confirm your email and access the latest site updates.<br/>{siteName}";
                        break;

                    case "ar-AE":
                        subject = "تأكيد البريد الإلكتروني";
                        message = $"{user.name} {user.family} عزيزي <br/> انقر <a href={address}> هنا </a> لتأكيد بريدك الإلكتروني والوصول إلى آخر تحديثات الموقع.<br/>{siteName}";
                        break;

                    case "it-IT":
                        subject = "Conferma via email";
                        message = $"{user.name} {user.family} Gentile <br/> Fai clic <a href={address}> qui </a> per confermare la tua email e accedere agli ultimi aggiornamenti del sito.<br/>{siteName}";
                        break;
                }
                await mail.Send(subject, message, user.Email);

                #endregion

                user.tokenCreationTime = DateTime.Now;

                await userManager.UpdateAsync(user);

                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[info] = "ایمیل تایید حساب کاربری برای شما ارسال شد.";
                        break;

                    case "en-US":
                        TempData[info] = "An account confirmation email has been sent to you.";
                        break;

                    case "ar-AE":
                        TempData[info] = "تم إرسال بريد إلكتروني لتأكيد الحساب إليك.";
                        break;

                    case "it-IT":
                        TempData[info] = "Ti è stata inviata un'email di conferma dell'account.";
                        break;
                }

                #endregion

                return RedirectToAction("Index", "Home");
            }
            else
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[info] = "رمزعبور نامعتبر است.";
                        break;

                    case "en-US":
                        TempData[info] = "Invalid password.";
                        break;

                    case "ar-AE":
                        TempData[info] = "رمز مرور خاطئ.";
                        break;

                    case "it-IT":
                        TempData[info] = "Password non valida.";
                        break;
                }

                #endregion

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

                    #region Multi language TempData

                    switch (lang)
                    {
                        case "fa-IR":
                            TempData[success] = "حساب کاربری شما با موفقیت تایید شد.";
                            break;

                        case "en-US":
                            TempData[success] = "Your account has been successfully verified.";
                            break;

                        case "ar-AE":
                            TempData[success] = "تم التحقق من حسابك بنجاح.";
                            break;

                        case "it-IT":
                            TempData[success] = "Il tuo account è stato verificato con successo.";
                            break;
                    }

                    #endregion

                    return RedirectToAction(nameof(Login), "Account");
                }
                else
                {
                    await userManager.DeleteAsync(user);

                    #region Multi language TempData

                    switch (lang)
                    {
                        case "fa-IR":
                            TempData[error] = "عملیات با شکست مواجه شد.";
                            break;

                        case "en-US":
                            TempData[error] = "The operation failed.";
                            break;

                        case "ar-AE":
                            TempData[error] = "فشلت العملية.";
                            break;

                        case "it-IT":
                            TempData[error] = "L'operazione non è riuscita.";
                            break;
                    }

                    #endregion

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                await userManager.DeleteAsync(user);

                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "لینک تایید ایمیل منقضی شده است. دوباره تلاش کنید.";
                        break;

                    case "en-US":
                        TempData[error] = "The email verification link has expired. Try again.";
                        break;

                    case "ar-AE":
                        TempData[error] = "انتهت صلاحية رابط التحقق من البريد الإلكتروني. حاول مجددا.";
                        break;

                    case "it-IT":
                        TempData[error] = "Il link di verifica dell'e-mail è scaduto. Riprova.";
                        break;
                }

                #endregion

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
                    #region Multi language TempData

                    switch (lang)
                    {
                        case "fa-IR":
                            TempData[warning] = "به دلیل اشتباه وارد کردن متوالی رمز عبور، اکانت شما به مدت 1 دقیقه غیر فعال خواهد شد.";
                            break;

                        case "en-US":
                            TempData[warning] = "Your account will be deactivated for 1 minute due to incorrect password entry.";
                            break;

                        case "ar-AE":
                            TempData[warning] = "سيتم إلغاء تنشيط حسابك لمدة دقيقة واحدة بسبب إدخال كلمة مرور غير صحيحة.";
                            break;

                        case "it-IT":
                            TempData[warning] = "Il tuo account verrà disattivato per 1 minuto a causa dell'immissione di una password errata.";
                            break;
                    }

                    #endregion

                    return RedirectToAction("Index", "Home");
                }

                if (status.Succeeded)
                {
                    if (await userManager.IsInRoleAsync(user, "admin"))
                    {
                        await signInManager.SignOutAsync();

                        var address = Url.Action(nameof(AdminConfirm), "Account", new { userId = user.Id }, Request.Scheme);

                        HttpContext.Session.SetString("password", model.password);

                        await mail.Send("ورود ادمین", $"جهت ورود به حساب کاربری خود <a href={address}>اینجا</a> کلیک کنید.<br/>{siteName}", user.Email);

                        TempData[info] = "کد تایید برای شما ارسال شد.";

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        #region Multi language TempData

                        switch (lang)
                        {
                            case "fa-IR":
                                TempData[success] = "وارد حساب کاربری خود شدید.";
                                break;

                            case "en-US":
                                TempData[success] = "Log in to your account.";
                                break;

                            case "ar-AE":
                                TempData[success] = "تسجيل الدخول إلى حسابك.";
                                break;

                            case "it-IT":
                                TempData[success] = "Accedi al tuo account.";
                                break;
                        }

                        #endregion

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    #region Multi language TempData

                    switch (lang)
                    {
                        case "fa-IR":
                            TempData[error] = "نام کاربری یا رمزعبور نادرست است.";
                            break;

                        case "en-US":
                            TempData[error] = "Username or password is incorrect.";
                            break;

                        case "ar-AE":
                            TempData[error] = "اسم المستخدم أو كلمة المرور غير صحيحة.";
                            break;

                        case "it-IT":
                            TempData[error] = "Nome utente o password non sono corretti.";
                            break;
                    }

                    #endregion

                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "نام کاربری موجود نمیباشد.";
                        break;

                    case "en-US":
                        TempData[error] = "Username is not available.";
                        break;

                    case "ar-AE":
                        TempData[error] = "اسم المستخدم غير متاح.";
                        break;

                    case "it-IT":
                        TempData[error] = "il nome utente non è disponibile.";
                        break;
                }

                #endregion

                return RedirectToAction(nameof(Login));
            }
        }


        public async Task<IActionResult> AdminConfirm(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var password = HttpContext.Session.GetString("password");

            await signInManager.PasswordSignInAsync(user, password, true, false);

            TempData[success] = "وارد حساب کاربری خود شدید.";

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> ForgotPasslvlOne(string email)
        {
            var user = await userManager.FindByNameAsync(email);

            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var address = Url.Action(nameof(ForgotPasslvlTwoBridge), "Account", new { userName = user.UserName, token = token }, "https");

                #region Send email by region

                switch (lang)
                {
                    case "fa-IR":
                        await mail.Send("تغییر رمز عبور", $"برای تغییر رمز عبور <a href={address}>اینجا</a> کلیک کنید.<br/>{siteName}", user.Email);
                        break;

                    case "en-US":
                        await mail.Send("Change Password", $"Click <a href={address}>here</a> To change the password.<br/>{siteName}", user.Email);
                        break;

                    case "ar-AE":
                        await mail.Send("غير كلمة السر", $"انقر <a href={address}>هنا</a> لتغيير كلمة مرورك.<br/>{siteName}", user.Email);
                        break;

                    case "it-IT":
                        await mail.Send("cambia la password", $"Fare clic <a href={address}>qui</a> per modificare la password.<br/>{siteName}", user.Email);
                        break;
                }

                #endregion

                user.forgotPassTimeSpan = DateTime.Now;

                await userManager.UpdateAsync(user);

                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[info] = "پیامک تغییر رمز عبور برای شما ارسال شد.";
                        break;

                    case "en-US":
                        TempData[info] = "A password change SMS was sent to you.";
                        break;

                    case "ar-AE":
                        TempData[info] = "تم إرسال رسالة SMS لتغيير كلمة المرور إليك.";
                        break;

                    case "it-IT":
                        TempData[info] = "Ti è stato inviato un SMS di modifica della password.";
                        break;
                }

                #endregion
            }
            else
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "حساب کاربری با این ایمیل موجود نمی باشد.";
                        break;

                    case "en-US":
                        TempData[error] = "Account with this email is not available.";
                        break;

                    case "ar-AE":
                        TempData[error] = "الحساب مع هذا البريد الإلكتروني غير متوفر.";
                        break;

                    case "it-IT":
                        TempData[error] = "L'account con questa email non è disponibile.";
                        break;
                }

                #endregion
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
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "کاربر گرامی، اعتبار این لینک 3 دقیقه است. لطفا دوباره تلاش کنید.";
                        break;

                    case "en-US":
                        TempData[error] = "Dear user, the validity of this link is 3 minutes. Please try again.";
                        break;

                    case "ar-AE":
                        TempData[error] = "عزيزي المستخدم صلاحية هذا الرابط 3 دقائق. حاول مرة اخرى.";
                        break;

                    case "it-IT":
                        TempData[error] = "Gentile utente, la validità di questo link è di 3 minuti. Per favore riprova.";
                        break;
                }

                #endregion

                return RedirectToAction("Index", "Home");
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
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[success] = "رمز عبور شما با موفقیت تغییر یافت.";
                        break;

                    case "en-US":
                        TempData[success] = "Your password has been successfully changed.";
                        break;

                    case "ar-AE":
                        TempData[success] = "كلمة السر الخاصة بك تم تغييرها بنجاح.";
                        break;

                    case "it-IT":
                        TempData[success] = "la tua password è stata cambiata con successo.";
                        break;
                }

                #endregion

                return RedirectToAction("Index", "Home");
            }
            else
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "تغییر رمز عبور با شکست مواجه شد.";
                        break;

                    case "en-US":
                        TempData[error] = "Password change failed.";
                        break;

                    case "ar-AE":
                        TempData[error] = "فشل تغيير كلمة المرور.";
                        break;

                    case "it-IT":
                        TempData[error] = "Modifica della password non riuscita.";
                        break;
                }

                #endregion

                return RedirectToAction("Index", "Home");
            }

        }


        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Admin

        [Authorize(policy: "AdminPolicy")]
        public async Task<IActionResult> IsUserBan(string userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);

            if (user.EmailConfirmed == true)
            {
                user.EmailConfirmed = false;

                await userManager.UpdateAsync(user);

                return Json(new { count = userManager.Users.Where(x => x.EmailConfirmed == false).Count(), text = "کاربر با موفقیت مسدود شد." });
            }
            else
            {
                user.EmailConfirmed = true;

                await userManager.UpdateAsync(user);

                return Json(new { count = userManager.Users.Where(x => x.EmailConfirmed == false).Count(), text = "محدودیت های کاربر برداشته شد." });
            }
        }

        #endregion


        #region Profile

        [Authorize]
        public IActionResult EditProfile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> EditProfileConfirm(EditProfileViewModel model)
        {
            if (model.userName != null)
            {
                if (await userManager.FindByNameAsync(model.userName) == null)
                {
                    ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

                    if (model.userName != null)
                    {
                        user.UserName = model.userName;
                        user.Email = model.userName;
                    }
                    if (model.phoneNumber != null)
                    {
                        user.PhoneNumber = model.phoneNumber;
                    }
                    if (model.name != null)
                    {
                        user.name = model.name;
                    }
                    if (model.family != null)
                    {
                        user.family = model.family;
                    }

                    await userManager.UpdateAsync(user);
                    await signInManager.RefreshSignInAsync(user);

                    #region Multi language TempData

                    switch (lang)
                    {
                        case "fa-IR":
                            TempData[success] = "اطلاعات شما با موفقیت ویرایش شد.";
                            break;

                        case "en-US":
                            TempData[success] = "Your information has been successfully edited.";
                            break;

                        case "ar-AE":
                            TempData[success] = "تم تحرير المعلومات الخاصة بك بنجاح.";
                            break;

                        case "it-IT":
                            TempData[success] = "Le tue informazioni sono state modificate con successo.";
                            break;
                    }

                    #endregion

                    return RedirectToAction(nameof(EditProfile), "Account");
                }
                else
                {
                    #region Multi language TempData

                    switch (lang)
                    {
                        case "fa-IR":
                            TempData[error] = "این نام کاربری در حال حاظر موجود میباشد.";
                            break;

                        case "en-US":
                            TempData[error] = "This username is currently available.";
                            break;

                        case "ar-AE":
                            TempData[error] = "اسم المستخدم هذا متاح حاليا.";
                            break;

                        case "it-IT":
                            TempData[error] = "Questo nome utente è attualmente disponibile.";
                            break;
                    }

                    #endregion

                    return RedirectToAction(nameof(EditProfile), "Account");
                }
            }
            else
            {
                ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

                if (model.userName != null)
                {
                    user.UserName = model.userName;
                    user.Email = model.userName;
                }
                if (model.phoneNumber != null)
                {
                    user.PhoneNumber = model.phoneNumber;
                }
                if (model.name != null)
                {
                    user.name = model.name;
                }
                if (model.family != null)
                {
                    user.family = model.family;
                }

                await userManager.UpdateAsync(user);
                await signInManager.RefreshSignInAsync(user);

                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[success] = "اطلاعات شما با موفقیت ویرایش شد.";
                        break;

                    case "en-US":
                        TempData[success] = "Your information has been successfully edited.";
                        break;

                    case "ar-AE":
                        TempData[success] = "تم تحرير المعلومات الخاصة بك بنجاح.";
                        break;

                    case "it-IT":
                        TempData[success] = "Le tue informazioni sono state modificate con successo.";
                        break;
                }

                #endregion

                return RedirectToAction(nameof(EditProfile), "Account");
            }

        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ChangePasswordConfirm(string password, string newPassword)
        {
            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            var status = await userManager.ChangePasswordAsync(user, password, newPassword);

            if (status.Succeeded)
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[success] = "رمز عبور با موفقیت تغییر یافت.";
                        break;

                    case "en-US":
                        TempData[success] = "Password changed successfully.";
                        break;

                    case "ar-AE":
                        TempData[success] = "تم تغيير الرقم السري بنجاح.";
                        break;

                    case "it-IT":
                        TempData[success] = "password cambiata con successo.";
                        break;
                }

                #endregion

                return RedirectToAction("Index", "Home");
            }
            else
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "تغییر رمز عبور با شکست مواجه شد.";
                        break;

                    case "en-US":
                        TempData[error] = "Password change failed.";
                        break;

                    case "ar-AE":
                        TempData[error] = "فشل تغيير كلمة المرور.";
                        break;

                    case "it-IT":
                        TempData[error] = "Modifica della password non riuscita.";
                        break;
                }

                #endregion

                return RedirectToAction(nameof(ChangePassword), "Account");
            }

        }

        #endregion


        #region Chats

        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> Purchases()
        {
            ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);

            return View(orderRepo.Get(x => x.userId == user.Id, x => x.OrderByDescending(x => x.id), null));
        }

        #endregion
    }
}
