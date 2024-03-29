﻿using Common.EmailSender;
using Data.Entities;
using Data.Repository;
using Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.EmailService;
using Services.ResizeService;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class CRUDController : Controller
    {
        #region KeyWords

        private const string success = "success";
        private const string error = "error";
        private const string warning = "warning";
        private const string info = "info";
        private const string ElementSuccess = "فیلد مورد نظر با موفقیت ثبت شد";
        private const string ElementError = "عملیات با شکست مواجه شد";
        private const string ElementUpdate = "فیلد مورد نظر ویرایش شد";
        private const string ElementRemove = "فیلد مورد حذف شد";
        private const string siteName = "www.tolosanat.com";

        #endregion


        #region Dependency Injections

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Group> groupRepo;
        private readonly IRepository<Message> messageRepo;
        private readonly IRepository<Project> projectRepo;
        private readonly IRepository<Post> postRepo;
        private readonly IRepository<Order> orderRepo;
        private readonly IRepository<Inbox> inboxRepo;
        private readonly IRepository<Language> languageRepo;
        private readonly IRepository<ElementProp> elementpropRepo;
        private readonly IRepository<SlideBar> slideBarRepo;
        private readonly IRepository<Element1> element1Repo;
        private readonly IRepository<Element2> element2Repo;
        private readonly IRepository<Element3> element3Repo;
        private readonly IRepository<Element4> element4Repo;
        private readonly IEmail email;
        private readonly IResize resize;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public CRUDController(UserManager<ApplicationUser> _userManager, IRepository<Group> _groupRepo, IRepository<Message> _messageRepo,
            IRepository<Project> _projectRepo, IRepository<Post> _postRepo, IRepository<Order> _orderRepo, IRepository<Inbox> _inboxRepo,
            IRepository<SlideBar> _slideBarRepo, IRepository<Element1> _element1Repo, IRepository<Element2> _element2Repo, IRepository<Element3> _element3Repo,
            IRepository<Element4> _element4Repo, IRepository<Language> _languageRepo, IResize _resize, IEmail _email,
            IRepository<ElementProp> _elementpropRepo)
        {
            userManager = _userManager;
            groupRepo = _groupRepo;
            messageRepo = _messageRepo;
            projectRepo = _projectRepo;
            postRepo = _postRepo;
            orderRepo = _orderRepo;
            inboxRepo = _inboxRepo;
            languageRepo = _languageRepo;
            elementpropRepo = _elementpropRepo;
            slideBarRepo = _slideBarRepo;
            element1Repo = _element1Repo;
            element2Repo = _element2Repo;
            element3Repo = _element3Repo;
            element4Repo = _element4Repo;
            email = _email;
            resize = _resize;
        }

        #endregion


        #region Chat CRUD

        public IActionResult DeleteChat(int id, int pagenumber)
        {
            Group group = groupRepo.Find(id);
            group.IsDeleteForAdmin = true;

            messageRepo.Get(x => x.groupId == id, null, null).ToList().ForEach(x =>
            {
                x.IsDeleteForAdmin = true;
            });

            groupRepo.Update(group);
            groupRepo.SaveChange();

            TempData[success] = "گفت و گو با موفقیت حذف شد";

            return RedirectToAction("ChatManagement", "Admin", new { pagenumber });
        }

        #endregion


        #region Project CRUD

        public async Task<IActionResult> InsertProject(ProjectViewModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string language = CultureInfo.CurrentCulture.Name;

            int projectId = 0;
            int lang = 1;
            model.languageIds.Select(x => new { id = lang++, value = x }).ToList().ForEach(x =>
            {
                var project = new Project();

                int title = 1;
                model.titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                {
                    int description = 1;
                    model.description.Select(z => new { id = description++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && y.id == z.id)
                        {
                            project.title = y.value;
                            project.Description = z.value;
                            project.languageId = x.value;
                            project.userId = user.Id;

                            byte[] b = new byte[model.img.Length];
                            model.img.OpenReadStream().Read(b, 0, b.Length);

                            project.thumbNail = resize.Resizer(b, 300, 300, ImageFormat.Jpeg);
                            project.detailImg = b;

                            projectRepo.Add(project);
                            projectRepo.SaveChange();

                            #region Check for languageId Equalution.

                            foreach (var item in languageRepo.Get(null, null, null))
                            {
                                if (item.title == language && x.id == item.id)
                                {
                                    projectId = project.id;
                                    break;
                                }
                            }

                            #endregion
                        }
                    });
                });
            });

            if (model.IsEmail == true)
            {
                #region Send Project Email

                string subject = "";
                string message = "";
                string url = Url.Action("ProjectDetailBridge", "Project", new { id = projectId }, "https");

                switch (language)
                {
                    case "fa-IR":
                        subject = "بازدید از نمونه کار جدید";
                        message = $"<p style='font-size: 17px'>کاربر گرامی، از جدید ترین نمونه کار ما از طریق لینک زیر دیدن کنید</p><br /><label>{siteName}</label>";
                        break;

                    case "en-US":
                        subject = "Visit the new work sample";
                        message = $"<p style='font-size: 17px'>Dear user, see our latest work sample via the link below</p><br /><label>{siteName}</label>";
                        break;

                    case "ar-AE":
                        subject = "قم بزيارة نموذج العمل الجديد";
                        message = $"<p style='font-size: 17px'>عزيزي المستخدم ، شاهد أحدث نموذج عمل لدينا عبر الرابط أدناه</p><br /><label>{siteName}</label>";
                        break;

                    case "it-IT":
                        subject = "Visita il nuovo campione di lavoro";
                        message = $"<p style='font-size: 17px'>Gentile utente, guarda il nostro ultimo esempio di lavoro tramite il link sottostante</p><br /><label>{siteName}</label>";
                        break;

                    case "tr-TR":
                        subject = "Yeni çalışma örneğini ziyaret edin";
                        message = $"<p style='font-size: 17px'>Sayın kullanıcı, aşağıdaki bağlantı üzerinden en son çalışma örneğimize bakın</p><br /><label>{siteName}</label>";
                        break;
                }

                SendEmail sendEmail = new(userManager, email);
                await sendEmail.Send(url, subject, message);

                #endregion
            }

            //if (model.IsSms)
            //{
            //    #region Send Project SMS

            //    string subject = "";
            //    string message = "";
            //    string url = Url.Action("ShowProjectBridge", "Post", new { id = projectId }, "https");

            //    switch (language)
            //    {
            //        case "fa-IR":
            //            subject = "بازدید از نمونه کار جدید";
            //            message = $"<p style='font-size: 17px'>کاربر گرامی، از جدید ترین نمونه کار ما از لینک زیر دیدن کنید</p><br /><label>{siteName}</label>";
            //            break;

            //        case "en-US":
            //            subject = "Visit the new work sample";
            //            message = $"<p style='font-size: 17px'>Dear user, see the latest example of our work from the following link</p><br /><label>{siteName}</label>";
            //            break;

            //        case "ar-AE":
            //            subject = "قم بزيارة نموذج العمل الجديد";
            //            message = $"<p style='font-size: 17px'> عزيزي المستخدم ، شاهد أحدث مثال على عملنا من الرابط التالي</p><br /><label>{siteName}</label>";
            //            break;

            //        case "it-IT":
            //            subject = "Visita il nuovo campione di lavoro";
            //            message = $"<p style='font-size: 17px'>Gentile utente, guarda l'ultimo esempio del nostro lavoro dal seguente link</p><br /><label>{siteName}</label>";
            //            break;

            //        case "tr-TR":
            //            subject = "Yeni çalışma örneğini ziyaret edin";
            //            message = $"<p style='font-size: 17px'>Sayın kullanıcı, aşağıdaki bağlantıdan çalışmalarımızın en son örneğine bakın</p><br /><label>{siteName}</label>";
            //            break;
            //    }

            //    SendEmail sendEmail = new(userManager, email);
            //    await sendEmail.Send(url, subject, message);

            //    #endregion
            //}

            TempData[success] = "نمونه کار مورد نظر با موفقیت ثبت شد";

            return RedirectToAction(nameof(InsertProject), "Admin");
        }


        public IActionResult UpdateProject(int id, string title, string description, IFormFile img)
        {
            var project = projectRepo.Find(id);

            project.title = title;
            project.Description = description;

            if (img != null)
            {
                byte[] b = new byte[img.Length];
                img.OpenReadStream().Read(b, 0, b.Length);

                project.thumbNail = resize.Resizer(b, 300, 300, ImageFormat.Jpeg);
                project.detailImg = b;
            }

            try
            {
                projectRepo.Update(project);

                projectRepo.SaveChange();

                TempData[success] = "نمونه کار با موفقیت ویرایش شد";

                return RedirectToAction("ProjectOptions", "Admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData[error] = "ویرایش نمونه کار با شکست مواجه شد";

                return RedirectToAction("ProjectOptions", "Admin", new { pagenumber = 1 });
            }
        }


        public IActionResult DeleteProject(int id, int pagenumber)
        {
            try
            {
                projectRepo.Remove(id);
                projectRepo.SaveChange();

                TempData[success] = "نمونه کار مورد نظر با موفقیت حذف شد";

                return RedirectToAction("ProjectOptions", "Admin", new { pagenumber });
            }
            catch
            {
                TempData[success] = "حذف نمونه کار با شکست مواجه شد";

                return RedirectToAction("ProjectOptions", "Admin", new { pagenumber });
            }
        }

        #endregion


        #region Post CRUD

        public async Task<IActionResult> InsertPost(PostViewModel model)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string language = CultureInfo.CurrentCulture.Name;

            int postId = 0;
            int lang = 1;
            model.languageIds.Select(x => new { id = lang++, value = x }).ToList().ForEach(x =>
            {
                var post = new Post();

                int title = 1;
                model.titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                 {
                     int shortDes = 1;
                     model.shortDescriptions.Select(z => new { id = shortDes++, value = z }).ToList().ForEach(z =>
                     {
                         int longDes = 1;
                         model.longDescriptions.Select(k => new { id = longDes++, value = k }).ToList().ForEach(k =>
                          {
                              if (x.id == y.id && y.id == z.id && z.id == k.id)
                              {
                                  post.title = y.value;
                                  post.shortDecpription = z.value;
                                  post.longDecpription = k.value;
                                  post.languageId = x.value;
                                  post.userId = user.Id;

                                  byte[] b = new byte[model.img.Length];
                                  model.img.OpenReadStream().Read(b, 0, b.Length);

                                  post.thumbNail = resize.Resizer(b, 370, 220, ImageFormat.Jpeg);
                                  post.detailImg = resize.Resizer(b, 770, 470, ImageFormat.Jpeg);

                                  postRepo.Add(post);
                                  postRepo.SaveChange();

                                  #region Check for languageId Equalution.

                                  foreach (var item in languageRepo.Get(null, null, null))
                                  {
                                      if (item.title == language && x.id == item.id)
                                      {
                                          postId = post.id;
                                          break;
                                      }
                                  }

                                  #endregion
                              }
                          });
                     });
                 });
            });

            if (model.IsEmail == true)
            {
                #region Send Project Email

                string subject = "";
                string message = "";
                string url = Url.Action("BlogDetailBridge", "Blog", new { id = postId }, "https");

                switch (language)
                {
                    case "fa-IR":
                        subject = "بازدید از مطلب جدید";
                        message = $"<p style='font-size: 17px'> Dear user, visit our latest article on the website via the following link</p><br /><label>{siteName}</label>";
                        break;

                    case "en-US":
                        subject = "Visit the new article";
                        message = $"<p style='font-size: 17px'>Dear user, see our latest post via the following link</p><br /><label>{siteName}</label>";
                        break;

                    case "ar-AE":
                        subject = "قم بزيارة المقال الجديد";
                        message = $"<p style='font-size: 17px'> عزيزي المستخدم ، تفضل بزيارة أحدث مقالتنا على الموقع عبر الرابط التالي </p><br /><label>{siteName}</label>";
                        break;

                    case "it-IT":
                        subject = "Visita il nuovo articolo";
                        message = $"<p style='font-size: 17px'> Gentile utente, visita il nostro ultimo articolo sul sito Web tramite il seguente link</p><br /><label>{siteName}</label>";
                        break;

                    case "tr-TR":
                        subject = "Yeni makaleyi ziyaret edin";
                        message = $"<p style='font-size: 17px'> Sayın kullanıcı, aşağıdaki bağlantı aracılığıyla web sitesindeki en son makalemizi ziyaret edin</p><br /><label>{siteName}</label>";
                        break;
                }

                SendEmail sendEmail = new(userManager, email);
                await sendEmail.Send(url, subject, message);

                #endregion
            }

            //if (model.IsSms)
            //{
            //    #region Send Project SMS

            //    string subject = "";
            //    string message = "";
            //    string url = Url.Action("ShowProjectBridge", "Post", new { id = projectId }, "https");

            //    switch (language)
            //    {
            //        case "fa-IR":
            //            subject = "بازدید از پست جدید";
            //            message = $"<p style='font-size: 17px'>کاربر گرامی، از جدید ترین پست ما از طریق لینک زیر دیدن کنید:</p><br /><label>{siteName}</label>";
            //            break;

            //        case "en-US":
            //            subject = "Visit the new post";
            //            message = $"<p style='font-size: 17px'>Dear user, see our latest post via the following link:</p><br /><label>{siteName}</label>";
            //            break;

            //        case "ar-AE":
            //            subject = "قم بزيارة المنشور الجديد";
            //            message = $"<p style='font-size: 17px'> عزيزي المستخدم ، شاهد أحدث منشوراتنا عبر الرابط التال:</p><br /><label>{siteName}</label>";
            //            break;

            //        case "it-IT":
            //            subject = "Visita il nuovo post";
            //            message = $"<p style='font-size: 17px'>Gentile utente, guarda il nostro ultimo post tramite il seguente link:</p><br /><label>{siteName}</label>";
            //            break;

            //        case "tr-TR":
            //            subject = "Yeni gönderiyi ziyaret edin";
            //            message = $"<p style='font-size: 17px'>Sayın kullanıcı, aşağıdaki bağlantı aracılığıyla en son yayınımıza bakın:</p><br /><label>{siteName}</label>";
            //            break;
            //    }

            //    SendEmail sendEmail = new(userManager, email);
            //    await sendEmail.Send(url, subject, message);

            //    #endregion
            //}

            TempData[success] = "پست مورد نظر با موفقیت ثبت شد.";

            return RedirectToAction(nameof(InsertPost), "Admin");
        }


        public IActionResult UpdatePost(int id, string title, string shortDescription, string longDescription, IFormFile img)
        {
            var post = postRepo.Find(id);

            post.title = title;
            post.shortDecpription = shortDescription;
            post.longDecpription = longDescription;

            if (img != null)
            {
                byte[] b = new byte[img.Length];
                img.OpenReadStream().Read(b, 0, b.Length);

                post.thumbNail = resize.Resizer(b, 370, 220, ImageFormat.Jpeg);
                post.detailImg = resize.Resizer(b, 770, 470, ImageFormat.Jpeg);
            }

            try
            {
                postRepo.Update(post);

                postRepo.SaveChange();

                TempData[success] = "پست با موفقیت ویرایش شد";

                return RedirectToAction("PostOptions", "Admin", new { pagenumber = 1 });
            }
            catch
            {
                TempData[error] = "ویرایش پست با شکست مواجه شد";

                return RedirectToAction("PostOptions", "Admin", new { pagenumber = 1 });
            }
        }


        public IActionResult DeletePost(int id, int pagenumber)
        {
            try
            {
                postRepo.Remove(id);
                postRepo.SaveChange();

                TempData[success] = "پست مورد نظر با موفقیت حذف شد";

                return RedirectToAction("PostOptions", "Admin", new { pagenumber });
            }
            catch
            {
                TempData[success] = "حذف پست با شکست مواجه شد";

                return RedirectToAction("PostOptions", "Admin", new { pagenumber });
            }
        }

        #endregion


        #region Inbox CRUD

        [AllowAnonymous]
        public async Task<IActionResult> InsertInbox(InboxViewModel model)
        {
            var inbox = new Inbox()
            {
                nameFamily = model.nameFamily,
                phonenumber = model.phonenumber,
                message = model.message
            };

            try
            {
                inboxRepo.Add(inbox);
                inboxRepo.SaveChange();

                #region Send email to admin

                string address = Url.Action("InboxOptions", "Admin", new { pagenumber = 1 }, "https");

                foreach (var item in userManager.Users.ToList())
                {
                    if (await userManager.IsInRoleAsync(item, "admin"))
                    {
                        await email.Send("پیغام جدید", $"<p style='font-size: 17px'>پیغام جدیدی به صندوق پیغام های شما ارسال شد. جهت مشاهده <a style='color: #e67e23' href='{address}'><b>(اینجا)</b></a> کلیک کنید</p><br /><label>siteName</label>", item.Email);

                        break;
                    }
                }

                #endregion

                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[success] = "پیغام شما با موفقیت ارسال شد";
                        break;

                    case "en-US":
                        TempData[success] = "Your message was sent successfully";
                        break;

                    case "ar-AE":
                        TempData[success] = "لقد تم ارسال رسالتك بنجاح";
                        break;

                    case "it-IT":
                        TempData[success] = "Il tema dell'invio del tuo messaggio ha successo";
                        break;

                    case "tr-TR":
                        TempData[success] = "Mesajınız başarıyla gönderildi";
                        break;
                }

                #endregion

                return RedirectToAction("ContactUs", "Home");
            }
            catch
            {
                #region Multi language TempData

                switch (lang)
                {
                    case "fa-IR":
                        TempData[error] = "ارسال پیغام با شکست مواجه شد. دوباره تلاش کنید";
                        break;

                    case "en-US":
                        TempData[error] = "Failed to send message. Try again";
                        break;

                    case "ar-AE":
                        TempData[error] = "فشل في إرسال الرسالة. حاول مجددا";
                        break;

                    case "it-IT":
                        TempData[error] = "Impossibile inviare il messaggio. Riprova";
                        break;

                    case "tr-TR":
                        TempData[error] = "Mesaj gönderilemedi. Tekrar deneyin";
                        break;
                }

                #endregion

                return RedirectToAction("ContactUs", "Home");
            }
        }


        public IActionResult DeleteInbox(int id, int pagenumber)
        {
            try
            {
                inboxRepo.Remove(id);
                inboxRepo.SaveChange();

                TempData[success] = "پیغام با موفقیت حذف شد";

                return RedirectToAction("InboxOptions", "Admin", new { pagenumber });
            }
            catch
            {
                TempData[error] = "حذف پیغام با شکست مواجه شد. دوباره تلاش کنید";

                return RedirectToAction("InboxOptions", "Admin", new { pagenumber });
            }
        }


        public int ConfirmationToggle(int inboxId)
        {
            Inbox inbox = inboxRepo.Find(inboxId);

            if (!inbox.IsConfirm)
                inbox.IsConfirm = true;
            else
                inbox.IsConfirm = false;

            inboxRepo.Update(inbox);
            inboxRepo.SaveChange();

            return inboxRepo.Get(x => x.IsConfirm == false, null, null).Count;
        }

        #endregion


        #region About Us CRUD

        public IActionResult UpdateAboutUsProp(List<int> languageIds, List<string> AboutUsShortDeses, List<string> AboutUsLongDeses)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int aboutUsShortDeses = 1;
                AboutUsShortDeses.Select(y => new { id = aboutUsShortDeses++, value = y }).ToList().ForEach(y =>
                {
                    int aboutUsLongDeses = 1;
                    AboutUsLongDeses.Select(z => new { id = aboutUsLongDeses++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && y.id == z.id)
                        {
                            elementProp.aboutUsShortDes = y.value;
                            elementProp.aboutUsLongDes = z.value;

                            elementpropRepo.Update(elementProp);
                            elementpropRepo.SaveChange();
                        }
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Contact Us CRUD

        public IActionResult UpdateContactUsProp(List<int> languageIds, List<string> ContactUsTitles, List<string> ContactUsDescriptions)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int contactUsTitles = 1;
                ContactUsTitles.Select(y => new { id = contactUsTitles++, value = y }).ToList().ForEach(y =>
                {
                    int contactUsDescriptions = 1;
                    ContactUsDescriptions.Select(z => new { id = contactUsDescriptions++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && y.id == z.id)
                        {
                            elementProp.contactUsTitle = y.value;
                            elementProp.contactUsDescription = z.value;

                            elementpropRepo.Update(elementProp);
                            elementpropRepo.SaveChange();
                        }
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Order CRUD

        public IActionResult UpdateOrderProp(List<int> languageIds, List<string> OrderDescription)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int orderDescription = 1;
                OrderDescription.Select(y => new { id = orderDescription++, value = y }).ToList().ForEach(y =>
                {
                    if (x.id == y.id)
                    {
                        elementProp.OrderDescription = y.value;

                        elementpropRepo.Update(elementProp);
                        elementpropRepo.SaveChange();
                    }
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> UserCostAsync(string userId, int price)
        {
            try
            {
                ApplicationUser user = await userManager.FindByIdAsync(userId);

                user.price = price;
                await userManager.UpdateAsync(user);

                TempData[success] = "هزینه مورد نظر اعمال شد";

                return RedirectToAction("AdminSideChatsBridge", "Admin", new { clientId = userId });
            }
            catch
            {
                TempData[error] = "ویرایش اطلاعات با شکست مواجه شد. دوباره تلاش کنید";

                return RedirectToAction("AdminSideChatsBridge", "Admin", new { clientId = userId });
            }
        }


        public int OrderConfirmationToggle(int OrderId)
        {
            Order order = orderRepo.Find(OrderId);

            if (!order.IsAdminConfirmed)
                order.IsAdminConfirmed = true;
            else
                order.IsAdminConfirmed = false;

            orderRepo.Update(order);
            orderRepo.SaveChange();

            return orderRepo.Get(x => x.IsAdminConfirmed == false, null, null).Count;
        }

        #endregion


        #region Header

        public IActionResult UpdateHeaderProp(List<int> languageIds, IFormFile mainLogo, IFormFile secondaryLogo, List<string> Address, string Email,
            string PhoneNumber, string InstaLink, string TelegramLink, IFormFile projectImage, List<string> SiteTopTab, List<string> ExtraDes1,
            List<string> ExtraDes2)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int address = 1;
                Address.Select(y => new { id = address++, value = y }).ToList().ForEach(y =>
                {
                    int siteTopTab = 1;
                    SiteTopTab.Select(z => new { id = siteTopTab++, value = z }).ToList().ForEach(z =>
                    {
                        int extraDes1 = 1;
                        ExtraDes1.Select(k => new { id = extraDes1++, value = k }).ToList().ForEach(k =>
                        {
                            int extraDes2 = 1;
                            ExtraDes2.Select(m => new { id = extraDes2++, value = m }).ToList().ForEach(m =>
                            {
                                if (x.id == y.id && y.id == z.id && z.id == k.id && k.id == m.id)
                                {
                                    if (x.value == 1)
                                    {
                                        if (mainLogo != null)
                                        {
                                            byte[] b = new byte[mainLogo.Length];
                                            mainLogo.OpenReadStream().Read(b, 0, b.Length);

                                            elementProp.mainLogo = b;
                                        }

                                        if (secondaryLogo != null)
                                        {
                                            byte[] d = new byte[secondaryLogo.Length];
                                            secondaryLogo.OpenReadStream().Read(d, 0, d.Length);

                                            elementProp.secondaryLogo = resize.Resizer(d, 35, 35, ImageFormat.Png);
                                        }
                                    }

                                    if (projectImage != null)
                                    {
                                        byte[] d = new byte[projectImage.Length];
                                        projectImage.OpenReadStream().Read(d, 0, d.Length);

                                        elementProp.projectImage = resize.Resizer(d, 1920, 630,
                                            System.IO.Path.GetExtension(projectImage.FileName) == ".jpg" ? ImageFormat.Jpeg : ImageFormat.Png);
                                    }

                                    elementProp.address = y.value;
                                    elementProp.email = Email;
                                    elementProp.phoneNumber = PhoneNumber;
                                    elementProp.instaLink = InstaLink;
                                    elementProp.telegramLink = TelegramLink;
                                    elementProp.siteTopTab = z.value;
                                    elementProp.extraDes1 = k.value;
                                    elementProp.extraDes2 = m.value;

                                    elementpropRepo.Update(elementProp);
                                    elementpropRepo.SaveChange();
                                }
                            });
                        });
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Slide Bar CRUD

        public IActionResult InsertSlidebar(CommonAttributesViewModel model)
        {
            int lang = 1;
            model.languageIds.Select(x => new { id = lang++, value = x }).ToList().ForEach(x =>
            {
                SlideBar slideBar = new();

                int title = 1;
                model.titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                {
                    int description = 1;
                    model.descriptions.Select(z => new { id = description++, value = z }).ToList().ForEach(z =>
                    {
                        int buttonTitles = 1;
                        model.buttonTitles.Select(m => new { id = buttonTitles++, value = m }).ToList().ForEach(m =>
                        {
                            if (x.id == y.id && y.id == z.id && z.id == m.id)
                            {
                                slideBar.languageId = x.value;
                                slideBar.title = y.value;
                                slideBar.description = z.value;
                                slideBar.buttonTitle = m.value;
                                slideBar.buttonLink = model.buttonLink;

                                if (model.img != null && System.IO.Path.GetExtension(model.img.FileName) != ".gif" ||
                                    System.IO.Path.GetExtension(model.img.FileName) != ".mp4" ||
                                    System.IO.Path.GetExtension(model.img.FileName) != ".mp3")
                                {
                                    byte[] b = new byte[model.img.Length];
                                    model.img.OpenReadStream().Read(b, 0, b.Length);

                                    slideBar.img = resize.Resizer(b, 1920, 720, ImageFormat.Jpeg);
                                }

                                slideBarRepo.Add(slideBar);
                            }
                        });
                    });
                });
            });

            try
            {
                slideBarRepo.SaveChange();

                TempData[success] = ElementSuccess;

                return RedirectToAction("SlideBarOptions", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("SlideBarOptions", "Admin");
            }
        }


        public IActionResult DeleteSlidebar(int id)
        {
            try
            {
                slideBarRepo.Remove(id);
                slideBarRepo.SaveChange();

                TempData[success] = ElementRemove;

                return RedirectToAction("SlideBarOptions", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("SlideBarOptions", "Admin");
            }
        }


        public IActionResult UpdateSlideBar(int id, string title, string description, string buttonTitle,
            string buttonLink, IFormFile img)
        {
            var slideBar = slideBarRepo.Find(id);

            slideBar.title = title;
            slideBar.description = description;
            slideBar.buttonTitle = buttonTitle;
            slideBar.buttonLink = buttonLink;

            if (img != null)
            {
                if (System.IO.Path.GetExtension(img.FileName) != ".gif" ||
                    System.IO.Path.GetExtension(img.FileName) != ".mp4" ||
                    System.IO.Path.GetExtension(img.FileName) != ".mp3")
                {
                    byte[] b = new byte[img.Length];
                    img.OpenReadStream().Read(b, 0, b.Length);

                    slideBar.img = resize.Resizer(b, 1920, 720, ImageFormat.Jpeg);
                }
            }

            try
            {
                slideBarRepo.Update(slideBar);
                slideBarRepo.SaveChange();

                TempData[success] = ElementUpdate;

                return RedirectToAction("SlideBarOptions", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("SlideBarOptions", "Admin");
            }
        }

        #endregion


        #region Element1 CRUD

        public IActionResult UpdateElement1Prop(List<int> languageIds, List<string> PreTitles, List<string> Titles,
            List<string> Descriptions)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int preTitle = 1;
                PreTitles.Select(y => new { id = preTitle++, value = y }).ToList().ForEach(y =>
                {
                    int title = 1;
                    Titles.Select(z => new { id = title++, value = z }).ToList().ForEach(z =>
                    {
                        int descriptions = 1;
                        Descriptions.Select(m => new { id = descriptions++, value = m }).ToList().ForEach(m =>
                        {
                            if (x.id == y.id && y.id == z.id && z.id == m.id)
                            {
                                elementProp.e1PreTitle = y.value;
                                elementProp.e1Title = z.value;
                                elementProp.e1Description = m.value;

                                elementpropRepo.Update(elementProp);
                                elementpropRepo.SaveChange();
                            }
                        });
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }


        public IActionResult InsertElement1(CommonAttributesViewModel model)
        {
            int lang = 1;
            model.languageIds.Select(x => new { id = lang++, value = x }).ToList().ForEach(x =>
            {
                Element1 element1 = new();

                int title = 1;
                model.titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                {
                    int description = 1;
                    model.descriptions.Select(z => new { id = description++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && x.id == z.id && y.id == z.id)
                        {
                            element1.languageId = x.value;
                            element1.title = y.value;
                            element1.description = z.value;

                            element1Repo.Add(element1);
                        }
                    });
                });
            });

            try
            {
                element1Repo.SaveChange();

                TempData[success] = ElementSuccess;

                return RedirectToAction("Element1Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element1Options", "Admin");
            }
        }


        public IActionResult DeleteElement1(int id)
        {
            try
            {
                element1Repo.Remove(id);
                element1Repo.SaveChange();

                TempData[success] = ElementRemove;

                return RedirectToAction("Element1Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element1Options", "Admin");
            }
        }


        public IActionResult UpdateElement1(int id, string title, string description)
        {
            var element1 = element1Repo.Find(id);

            element1.title = title;
            element1.description = description;

            try
            {
                element1Repo.Update(element1);
                element1Repo.SaveChange();

                TempData[success] = ElementUpdate;

                return RedirectToAction("Element1Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element1Options", "Admin");
            }
        }

        #endregion


        #region Element2 CRUD

        public IActionResult UpdateElement2Prop(List<int> languageIds, List<string> PreTitles, List<string> Titles,
            List<string> Descriptions, string ImageTitle, IFormFile img)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int preTitle = 1;
                PreTitles.Select(y => new { id = preTitle++, value = y }).ToList().ForEach(y =>
                {
                    int title = 1;
                    Titles.Select(z => new { id = title++, value = z }).ToList().ForEach(z =>
                    {
                        int descriptions = 1;
                        Descriptions.Select(m => new { id = descriptions++, value = m }).ToList().ForEach(m =>
                        {
                            if (x.id == y.id && y.id == z.id && z.id == m.id)
                            {
                                elementProp.e2PreTitle = y.value;
                                elementProp.e2Title = z.value;
                                elementProp.e2Description = m.value;
                                elementProp.e2ImageTitle = ImageTitle;

                                if (img != null)
                                {
                                    byte[] b = new byte[img.Length];
                                    img.OpenReadStream().Read(b, 0, b.Length);

                                    elementProp.e2img = resize.Resizer(b, 470, 530,
                                        System.IO.Path.GetExtension(img.FileName) == ".jpg" ? ImageFormat.Jpeg : ImageFormat.Png);
                                }

                                elementpropRepo.Update(elementProp);
                                elementpropRepo.SaveChange();
                            }
                        });
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }


        public IActionResult InsertElement2(CommonAttributesViewModel model)
        {
            int lang = 1;
            model.languageIds.Select(x => new { id = lang++, value = x }).ToList().ForEach(x =>
            {
                Element2 element2 = new();

                int title = 1;
                model.titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                {
                    int description = 1;
                    model.descriptions.Select(z => new { id = description++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && y.id == z.id)
                        {
                            element2.languageId = x.value;
                            element2.title = y.value;
                            element2.description = z.value;

                            element2Repo.Add(element2);
                        }
                    });
                });
            });

            try
            {
                element2Repo.SaveChange();

                TempData[success] = ElementSuccess;

                return RedirectToAction("Element2Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element2Options", "Admin");
            }
        }


        public IActionResult DeleteElement2(int id)
        {
            try
            {
                element2Repo.Remove(id);
                element2Repo.SaveChange();

                TempData[success] = ElementRemove;

                return RedirectToAction("Element2Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element2Options", "Admin");
            }
        }


        public IActionResult UpdateElement2(int id, string title, string description)
        {
            var element2 = element2Repo.Find(id);

            element2.title = title;
            element2.description = description;

            try
            {
                element2Repo.Update(element2);
                element2Repo.SaveChange();

                TempData[success] = ElementUpdate;

                return RedirectToAction("Element2Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element2Options", "Admin");
            }
        }

        #endregion


        #region Element3 CRUD

        public IActionResult UpdateElement3Prop(List<int> languageIds, List<string> PreTitles, List<string> Titles,
            List<string> Descriptions, IFormFile img)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int preTitle = 1;
                PreTitles.Select(y => new { id = preTitle++, value = y }).ToList().ForEach(y =>
                {
                    int title = 1;
                    Titles.Select(z => new { id = title++, value = z }).ToList().ForEach(z =>
                    {
                        int descriptions = 1;
                        Descriptions.Select(m => new { id = descriptions++, value = m }).ToList().ForEach(m =>
                        {
                            if (x.id == y.id && y.id == z.id && z.id == m.id)
                            {
                                elementProp.e3PreTitle = y.value;
                                elementProp.e3Title = z.value;
                                elementProp.e3Description = m.value;

                                if (img != null)
                                {
                                    byte[] b = new byte[img.Length];
                                    img.OpenReadStream().Read(b, 0, b.Length);

                                    elementProp.e3img = resize.Resizer(b, 570, 434,
                                        System.IO.Path.GetExtension(img.FileName) == ".jpg" ? ImageFormat.Jpeg : ImageFormat.Png);
                                }

                                elementpropRepo.Update(elementProp);
                                elementpropRepo.SaveChange();
                            }
                        });
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }


        public IActionResult InsertElement3(CommonAttributesViewModel model)
        {
            int lang = 1;
            model.languageIds.Select(x => new { id = lang++, value = x }).ToList().ForEach(x =>
            {
                Element3 element3 = new();

                int title = 1;
                model.titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                {
                    int description = 1;
                    model.descriptions.Select(z => new { id = description++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && x.id == z.id && y.id == z.id)
                        {
                            element3.languageId = x.value;
                            element3.title = y.value;
                            element3.description = z.value;

                            element3Repo.Add(element3);
                        }
                    });
                });
            });

            try
            {
                element3Repo.SaveChange();

                TempData[success] = ElementSuccess;

                return RedirectToAction("Element3Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element3Options", "Admin");
            }
        }


        public IActionResult DeleteElement3(int id)
        {
            try
            {
                element3Repo.Remove(id);
                element3Repo.SaveChange();

                TempData[success] = ElementRemove;

                return RedirectToAction("Element3Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element3Options", "Admin");
            }
        }


        public IActionResult UpdateElement3(int id, string title, string description)
        {
            var element3 = element3Repo.Find(id);

            element3.title = title;
            element3.description = description;

            try
            {
                element3Repo.Update(element3);
                element3Repo.SaveChange();

                TempData[success] = ElementUpdate;

                return RedirectToAction("Element3Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element3Options", "Admin");
            }
        }

        #endregion


        #region Element4 CRUD

        public IActionResult UpdateElement4Prop(List<int> languageIds, List<string> PreTitles, List<string> Titles,
            List<string> Descriptions, IFormFile img)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int preTitle = 1;
                PreTitles.Select(y => new { id = preTitle++, value = y }).ToList().ForEach(y =>
                {
                    int title = 1;
                    Titles.Select(z => new { id = title++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && y.id == z.id)
                        {
                            elementProp.e4PreTitle = y.value;
                            elementProp.e4Title = z.value;

                            if (img != null)
                            {
                                byte[] b = new byte[img.Length];
                                img.OpenReadStream().Read(b, 0, b.Length);

                                elementProp.e4img = resize.Resizer(b, 568, 519,
                                    System.IO.Path.GetExtension(img.FileName) == ".jpg" ? ImageFormat.Jpeg : ImageFormat.Png);
                            }

                            elementpropRepo.Update(elementProp);
                            elementpropRepo.SaveChange();
                        }
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }


        public IActionResult InsertElement4(CommonAttributesViewModel model)
        {
            int lang = 1;
            model.languageIds.Select(x => new { id = lang++, value = x }).ToList().ForEach(x =>
            {
                Element4 Element4 = new();

                int title = 1;
                model.titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                {
                    int description = 1;
                    model.descriptions.Select(z => new { id = description++, value = z }).ToList().ForEach(z =>
                    {
                        if (x.id == y.id && x.id == z.id && y.id == z.id)
                        {
                            Element4.languageId = x.value;
                            Element4.title = y.value;
                            Element4.description = z.value;

                            element4Repo.Add(Element4);
                        }
                    });
                });
            });

            try
            {
                element4Repo.SaveChange();

                TempData[success] = ElementSuccess;

                return RedirectToAction("Element4Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element4Options", "Admin");
            }
        }


        public IActionResult DeleteElement4(int id)
        {
            try
            {
                element4Repo.Remove(id);
                element4Repo.SaveChange();

                TempData[success] = ElementRemove;

                return RedirectToAction("Element4Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element4Options", "Admin");
            }
        }


        public IActionResult UpdateElement4(int id, string title, string description)
        {
            var Element4 = element4Repo.Find(id);

            Element4.title = title;
            Element4.description = description;

            try
            {
                element4Repo.Update(Element4);
                element4Repo.SaveChange();

                TempData[success] = ElementUpdate;

                return RedirectToAction("Element4Options", "Admin");
            }
            catch
            {
                TempData[error] = ElementError;

                return RedirectToAction("Element4Options", "Admin");
            }
        }

        #endregion


        #region Element5 CRUD

        public IActionResult UpdateElement5Prop(List<int> languageIds, List<string> Titles, List<string> Descriptions,
            List<string> ButtonTexts, List<string> ButtonLinks)
        {
            int langId = 1;
            languageIds.Select(x => new { id = langId++, value = x }).ToList().ForEach(x =>
            {
                ElementProp elementProp = elementpropRepo.Get(y => y.languageId == x.value, null, null).FirstOrDefault();

                int title = 1;
                Titles.Select(y => new { id = title++, value = y }).ToList().ForEach(y =>
                {
                    int descriptions = 1;
                    Descriptions.Select(z => new { id = descriptions++, value = z }).ToList().ForEach(z =>
                    {
                        int buttonTexts = 1;
                        ButtonTexts.Select(m => new { id = buttonTexts++, value = m }).ToList().ForEach(m =>
                        {
                            int buttonLinks = 1;
                            ButtonLinks.Select(n => new { id = buttonLinks++, value = n }).ToList().ForEach(n =>
                            {
                                if (x.id == y.id && y.id == z.id && z.id == m.id && m.id == n.id)
                                {
                                    elementProp.e5Title = y.value;
                                    elementProp.e5Description = z.value;
                                    elementProp.e5ButtonText = m.value;
                                    elementProp.e5ButtonLink = n.value;

                                    elementpropRepo.Update(elementProp);
                                    elementpropRepo.SaveChange();
                                }
                            });
                        });
                    });
                });
            });

            TempData[success] = ElementUpdate;

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
