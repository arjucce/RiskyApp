using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RiskyApp.Data;
using RiskyApp.Models;
using RiskyApp.Models.VM;

namespace RiskyApp.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class EmailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        [BindProperty]
        public EmailVM emailVM { get; set; }

        public EmailController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            List<EmailMgmtVM> emailMgmtVMs = new List<EmailMgmtVM>();

            emailMgmtVMs = (from a in _context.Emails.ToList()
                            join b in _context.Departments.ToList() on a.DepartmentId equals b.Id
                            join c in _context.Categories.ToList() on a.CategoryId equals c.Id
                            select new EmailMgmtVM { DataID = a.Id, DepartmentName = b.Name, EmailTo = a.To, EmailCC = a.CC, CategoryName = c.Name }).ToList();            

            return View(emailMgmtVMs);
        }

        public IActionResult Create()
        {
            emailVM = new EmailVM()
            {
                To="",
                CC="",
                DepartmentList = _context.Departments.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
                CategoryList = _context.Categories.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() })
            };

            return View(emailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Email email)
        {
            try
            {
                email = new Email()
                {
                    DepartmentId = emailVM.DepartmentId,
                    CategoryId = emailVM.CategoryId,
                    To = emailVM.To,
                    CC = emailVM.CC
                };
                _context.Emails.Add(email);
                _context.SaveChanges();
                _notyf.Success("New mail address created !!!");
                return RedirectToAction(nameof(Create));
            }
            catch(Exception ex)
            {
                _notyf.Warning(" "+ ex.Message.ToString() +" ");
                return RedirectToAction(nameof(Create));
            }                     
            //return View(emailVM);
        }


        public IActionResult Delete(int? id)
        {            
            var mailID = _context.Emails.FirstOrDefault(a => a.Id == id);
            if (mailID != null)
            {
                _context.Emails.Remove(mailID);
                _context.SaveChanges();
                _notyf.Warning("Email deleted!!");
                return RedirectToAction(nameof(Index));
            }

            return View(Index);
        }

    }
}
