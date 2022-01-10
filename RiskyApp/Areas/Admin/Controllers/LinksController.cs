using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiskyApp.Data;
using RiskyApp.Models;
using RiskyApp.Models.VM;
using System.Net;
using System.Net.Mail;

namespace RiskyApp.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        [BindProperty]
        public LinkVM linkVM { get; set; }
        [BindProperty]
        public LinkListVM linkListVM { get; set; }
        [BindProperty]
        public EmailVM emailVM { get; set; }

        public LinksController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            var links = (from a in _context.Links.ToList()
                                       join b in _context.Departments.ToList() on a.DepartmentId equals b.Id
                                       orderby a.DepartmentId
                                       select  new LinkListVM()
                                        {
                                            Id = a.Id,
                                            Name= b.Name,
                                            DataLink= a.DataLink
                                        }).ToList();
            return View(links);
        }

        public IActionResult Upsert()
        {
            linkVM = new LinkVM()
            {
                Links = new Link(),
                DepartmentList = _context.Departments.Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };                       
            return View(linkVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Link link)
        {
            if (ModelState.IsValid)
            {
                var checkD = _context.Links.FirstOrDefault(a=>a.DepartmentId == link.DepartmentId);
                if (checkD == null)
                {
                    link = new Link()
                    {
                        DataLink = "http://10.11.1.164/Data/User/Data/Index?departmentId=" + linkVM.DepartmentId,
                        DepartmentId = linkVM.DepartmentId
                    };

                    _context.Links.Add(link);
                    _context.SaveChanges();
                    TempData["Success"] = "Link generated.";
                    return RedirectToAction(nameof(Upsert));
                }
                else
                {
                    TempData["Success"] =link.DataLink + "already exists !!!!";
                    return RedirectToAction(nameof(Upsert));
                }                
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            var link = _context.Links.FirstOrDefault(x => x.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            _context.Links.Remove(link);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Email()
        {
            emailVM = new EmailVM()
            {                
                DepartmentList = _context.Departments.Select(i => new SelectListItem() {Text = i.Name,Value = i.Id.ToString()}),
                CategoryList = _context.Categories.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
                MonthList = _context.Months.Select(i => new SelectListItem() { Text = i.Name, Value = i.Name.ToString() }),
                YearList = _context.Years.Select(i => new SelectListItem() { Text = i.YearName, Value = i.YearName.ToString() }),
                Months = _context.Months.ToList()
            };
            return View(emailVM);
        }
        
        [HttpPost]
        public IActionResult EmailSend(string[] months)
        {
            try
            {
                //Getting on/or before date
                string dt = HttpContext.Request.Form["dt"].ToString();
                DateTime date = Convert.ToDateTime(dt, System.Globalization.CultureInfo.InvariantCulture);
                string m = "", multicc = "";                
                //Getting view data
                emailVM = new EmailVM()
                {
                    DepartmentId = emailVM.DepartmentId,
                    CategoryId = emailVM.CategoryId,
                    PeriodFrom = emailVM.PeriodFrom,
                    PeriodTo = emailVM.PeriodTo,
                    Years = emailVM.Years
                };
                
                //Getting email To and Cc. And also category name and generating link for email
                string to = (from a in _context.Emails where a.DepartmentId == emailVM.DepartmentId && a.CategoryId ==emailVM.CategoryId select a.To).FirstOrDefault();
                string cc = (from a in _context.Emails where a.DepartmentId == emailVM.DepartmentId && a.CategoryId == emailVM.CategoryId select a.CC).FirstOrDefault();
                string categoryName = (from a in _context.Categories where a.Id == emailVM.CategoryId select a.Name).FirstOrDefault();
                string link = "http://10.11.1.164/Data/User/Data/Index?departmentId=" + emailVM.DepartmentId + "&" + "categoryId=" + emailVM.CategoryId;

                //Email configuration for send mail
                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("icrr.rmd@bankasia-bd.com", "Risk Management Division");
                mail.From = new MailAddress("rmd.ba@bankasia-bd.com", "Risk Management Division");
                //mail.To.Add(new MailAddress("muhammad.mohiuddin@bankasia-bd.com", "muhammad.mohiuddin@bankasia-bd.com"));
                //mail.To.Add(new MailAddress("md.ariful@bankasia-bd.com", "md.ariful@bankasia-bd.com"));
                
                ///Multiple To adding
                if (to.Contains(","))
                {
                    string[] to_multiple = to.Split(',');
                    foreach (string toMail in to_multiple)
                    {
                        mail.To.Add(new MailAddress(toMail));
                    }
                }
                else
                {
                    mail.To.Add(new MailAddress("" + to + "", "" + to + ""));
                }

                ///Multiple CC adding
                if (!string.IsNullOrEmpty(cc))
                {
                    if (cc.Contains(","))
                    {
                        string[] cc_multiple = cc.Split(',');
                        foreach (string ccMail in cc_multiple)
                        {
                            mail.CC.Add(new MailAddress(ccMail));
                        }
                    }
                    else
                    {
                        mail.CC.Add(new MailAddress("" + cc + "", "" + cc + ""));
                    }
                }
                

                //Multiple CC recepient
                if (emailVM.DepartmentId == 1016)
                {
                    multicc = "rashidul.kabir@bankasia-bd.com,ibrahim.khalil@bankasia-bd.com,monis.mortuza@bankasia-bd.com,t.islam@bankasia-bd.com,muhammad.mohiuddin@bankasia-bd.com,md.ariful@bankasia-bd.com,sagar.saha@bankasia-bd.com";
                }
                else if (emailVM.DepartmentId == 2)
                {
                    multicc = "adil.chowdhury@bankasia-bd.com,rashidul.kabir@bankasia-bd.com,monis.mortuza@bankasia-bd.com,t.islam@bankasia-bd.com,muhammad.mohiuddin@bankasia-bd.com,md.ariful@bankasia-bd.com,sagar.saha@bankasia-bd.com";
                }
                else
                {
                    multicc = "rashidul.kabir@bankasia-bd.com,monis.mortuza@bankasia-bd.com,t.islam@bankasia-bd.com,muhammad.mohiuddin@bankasia-bd.com,md.ariful@bankasia-bd.com,sagar.saha@bankasia-bd.com";
                }

                string[] multiCCC = multicc.Split(',');
                foreach (string ccMail in multiCCC)
                {
                    mail.CC.Add(new MailAddress(ccMail));
                }

                var smtp = new System.Net.Mail.SmtpClient();                
                mail.Subject = HttpContext.Request.Form["mailSubject"].ToString();
                mail.Body = @"Dear Sir," + "</br></br>"
                                    + "As part of preparing <b> " + categoryName + " </b> few information is required from your end covering the period from <b> "+ emailVM.PeriodFrom + " to  " + emailVM.PeriodTo + ",  " + emailVM.Years + " </b>. "
                                    + "</br>" + "</br>"
                                    + "Please use the link below to submit your information. Mentionable the link also contains list of information required from your end along with related template for reporting (as and where applicable)."
                                    + "</br>" + "</br>"
                                    + " <a href=" + link + ">" + " " + link + " " + "</a>"
                                    + "</br>" + "</br>"
                                    + "To meet the reporting deadline it will be good time if we receive your response on or before  <b>  " + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(date)) + " </b>. Also please feel free to communicate us Ext. 001683, 001644, 001698, 001682, 001615."
                                    + "</br>" + "</br>"
                                    + "<b> (As part our continuous effort to evolve, we have adopted system based reporting system. This is an auto generated email.)  </b>"
                                    + "</br>" + "</br>" + "</br>"
                                    + "Thanks is advance,"
                                    + "</br>" + "</br>"
                                    + "Team-RMD";                                                                                                                                                                                                                                                         ;
                mail.IsBodyHtml = true;
                smtp.Host = "mail.bankasia-bd.com";
                smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                smtp.Send(mail);
                _notyf.Success("Mail send successfull !!!!");
                return RedirectToAction(nameof(Email));
            }
            catch(Exception ex)
            {
                _notyf.Warning(" "+ ex.Message.ToString() +" ");
                return RedirectToAction(nameof(Email));
            }

            return View();                      
        }
    }
}
