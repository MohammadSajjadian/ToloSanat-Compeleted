using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.EmailService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        #region KeyWords

        private const string success = "success";
        private const string error = "error";
        private const string siteName = "www.xyz.com";

        #endregion


        #region Dependency Injections

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Group> groupRepo;
        private readonly IRepository<Message> messageRepo;
        private readonly IEmail email;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public ChatController(UserManager<ApplicationUser> _userManager, IRepository<Group> _groupRepo,
            IRepository<Message> _messageRepo, IEmail _email)
        {
            userManager = _userManager;
            groupRepo = _groupRepo;
            messageRepo = _messageRepo;
            email = _email;
        }

        #endregion


        #region Chat

        [Authorize(policy: "AdminPolicy")]
        public async Task<IActionResult> InsertAdminMessage(string adminId, string clientId, int groupId, string text, IFormFile file)
        {
            try
            {
                ApplicationUser admin = await userManager.FindByIdAsync(adminId);
                ApplicationUser client = await userManager.FindByIdAsync(clientId);

                Message message = new()
                {
                    userId = admin.Id,
                    groupId = groupId,
                    text = text,
                    createTime = DateTime.Now,
                    IsDeleteForAdmin = false,
                };

                if (file != null)
                {
                    if (Path.GetExtension(file.FileName) == ".pdf")
                    {
                        byte[] b = new byte[file.Length];
                        file.OpenReadStream().Read(b, 0, b.Length);

                        message.file = b;
                    }
                }

                messageRepo.Add(message);
                messageRepo.SaveChange();

                Group group = groupRepo.Find(groupId);
                group.lastMessageTime = message.createTime;

                groupRepo.Update(group);
                groupRepo.SaveChange();

                #region Answer Email by Region

                string subject = "";
                string body = "";

                switch (lang)
                {
                    case "fa-IR":
                        subject = "پاسخ پیغام";
                        body = $"پیغام شما پاسخ داده شد.<br />{siteName}";
                        break;

                    case "en-US":
                        subject = "Reply to message";
                        body = $"Your message has been answered.<br />{siteName}";
                        break;

                    case "ar-AE":
                        subject = "الرد على الرسالة";
                        body = $"تم الرد على رسالتك.<br />{siteName}";
                        break;

                    case "it-IT":
                        subject = "Rispondi al messaggio";
                        body = $"Il tuo messaggio ha ricevuto risposta.<br />{siteName}";
                        break;
                }

                await email.Send(subject, body, client.Email);

                #endregion

                return RedirectToAction("AdminSideChatsBridge", "Admin", new { clientId });
            }
            catch
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "ارسال پیغام با خطا مواجه شد.";
                        break;

                    case "en-US":
                        TempData[error] = "Error sending message.";
                        break;

                    case "ar-AE":
                        TempData[error] = "خطأ في إرسال الرسالة.";
                        break;

                    case "it-IT":
                        TempData[error] = "Errore nell'invio del messaggio.";
                        break;
                }

                #endregion

                return RedirectToAction("AdminSideChatsBridge", "Admin", new { clientId });
            }
        }


        public async Task<IActionResult> InsertClientMessage(string clientId, string text, IFormFile file)
        {
            if (groupRepo.Get(null, null, null).Any(x => x.userId == clientId) == true)
            {
                Group group = groupRepo.Get(x => x.userId == clientId, null, null).FirstOrDefault();
                group.IsDeleteForAdmin = false;

                Message message = new()
                {
                    userId = clientId,
                    groupId = group.id,
                    text = text,
                    createTime = DateTime.Now,
                    IsDeleteForAdmin = false
                };

                if (file != null)
                {
                    if (Path.GetExtension(file.FileName) == ".pdf")
                    {
                        byte[] b = new byte[file.Length];

                        file.OpenReadStream().Read(b, 0, b.Length);
                        message.file = b;
                    }
                }

                messageRepo.Add(message);
                messageRepo.SaveChange();

                #region Answer Email by SMS

                foreach (var item in userManager.Users.ToList())
                {
                    if (await userManager.IsInRoleAsync(item, "admin"))
                    {
                        await email.Send("پاسخ پیغام", "پیغام شما پاسخ داده شد.<br />" + siteName, item.Email);

                        break;
                    }
                }

                #endregion

                return RedirectToAction("Support", "Account");
            }
            else
            {
                Group group = new() { userId = clientId, IsDeleteForAdmin = false };
                groupRepo.Add(group);
                groupRepo.SaveChange();

                Message message = new()
                {
                    userId = clientId,
                    groupId = group.id,
                    text = text,
                    createTime = DateTime.Now,
                    IsDeleteForAdmin = false
                };

                if (file != null)
                {
                    if (Path.GetExtension(file.FileName) == ".pdf")
                    {
                        byte[] b = new byte[file.Length];

                        file.OpenReadStream().Read(b, 0, b.Length);
                        message.file = b;
                    }
                }

                messageRepo.Add(message);
                messageRepo.SaveChange();

                #region Answer SMS by Region

                foreach (var item in userManager.Users.ToList())
                {
                    if (await userManager.IsInRoleAsync(item, "admin"))
                    {
                        await email.Send("پاسخ پیغام", "پیغام شما پاسخ داده شد.\n" + siteName, item.Email);

                        break;
                    }
                }

                #endregion

                return RedirectToAction("Support", "Account");
            }
        }

        #endregion


        #region PDf

        public IActionResult DownloadPdf(int messageId)
        {
            Message message = messageRepo.Find(messageId);

            return File(message.file, "application/pdf", "file.pdf");
        }

        #endregion
    }
}
