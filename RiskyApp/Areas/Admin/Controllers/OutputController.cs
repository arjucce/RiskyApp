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
    public class OutputController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        [BindProperty]
        public OutputVM outputVM { get; set; }

        public OutputController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Index()
        {            
            outputVM = new OutputVM()
            {
                DepartmentList = _context.Departments.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
                CategoryList = _context.Categories.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
                //Datas = _context.Data.ToList()
            };

            return View(outputVM);
        }
    }
}
