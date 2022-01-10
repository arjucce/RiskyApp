using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskyApp.Data;
using RiskyApp.Models;

namespace RiskyApp.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;        

        public DepartmentController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }


        public IActionResult Index()
        {
            return View(_context.Departments.ToList());
        }

        public IActionResult Upsert(int? id)
        {
            Department department = new Department();
            if (id == null)
            {
                return View(department);
            }

            department = _context.Departments.FirstOrDefault(e=>e.Id == id);
            if(department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Department department)
        {
            if (ModelState.IsValid)
            {
                if(department.Id == 0)
                {
                    _context.Departments.Add(department);                    
                    _notyf.Success("Department Created");
                }
                else
                {
                    _context.Departments.Update(department);                    
                    _notyf.Success("Department Updated");
                }

                _context.SaveChanges();                
                return RedirectToAction(nameof(Upsert));
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var department = _context.Departments.FirstOrDefault(a=>a.Id == id);
            if(department != null)
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
                _notyf.Warning("Department deleted!!");
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }
    }
}
