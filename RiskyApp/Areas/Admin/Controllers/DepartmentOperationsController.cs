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
    public class DepartmentOperationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public DepartmentOperationVM departmentOperationVM { get; set; }

        public DepartmentOperationsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, INotyfService notyf)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            List<DepartmentOperation> departmentOperations = _context.DepartmentOperations.ToList();
            List<Department> departments = _context.Departments.ToList();
            List<OpType> types = _context.OpTypes.ToList();

            var result = (from a in departmentOperations
                         join b in departments on a.DepartmentId equals b.Id
                         join c in types on a.OperationTypeId equals c.Id
                         join d in _context.Categories.ToList() on a.CategoryId equals d.Id
                         select new DepartmentOperationVM 
                         {
                             DepartmentOperationID = a.Id,
                             DepartmentName = b.Name,
                             OperationTypeName = c.Type,
                             OperationName = a.OperationName,
                             FormatUrl = a.FormatUrl,
                             OperationCategory = d.Name
                         }).AsEnumerable().OrderBy(a=>a.DepartmentName);

            ViewBag.dpVM = result;
            return View();
        }
        
        public IActionResult Create()
        {            
            departmentOperationVM = new DepartmentOperationVM()
            {
                DepartmentOperations = new DepartmentOperation(),
                OperationType = new OpType(),
                DepartmentList =  _context.Departments.Select(i => new SelectListItem(){ Text = i.Name ,Value = i.Id.ToString() }),
                OperationTypeList = _context.OpTypes.Select(i => new SelectListItem() { Text = i.Type, Value = i.Id.ToString() }),
                CategoryList = _context.Categories.Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() })
            };
            return View(departmentOperationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentOperation departmentOperation)
        {
            try
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"Upload\format");
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    departmentOperation = new DepartmentOperation()
                    {
                        DepartmentId = departmentOperationVM.DepartmentID,
                        OperationName = departmentOperationVM.DepartmentOperations.OperationName,
                        OperationTypeId = departmentOperationVM.OperationTypeID,
                        FormatUrl = @"\Upload\format\" + fileName + extension,
                        CategoryId = departmentOperationVM.OperationCategoryID
                    };
                    _context.Add(departmentOperation);
                    _context.SaveChanges();
                    _notyf.Success("Operation created successfully !!!!");
                }
                else
                {
                    departmentOperation = new DepartmentOperation()
                    {
                        DepartmentId = departmentOperationVM.DepartmentID,
                        OperationName = departmentOperationVM.DepartmentOperations.OperationName,
                        OperationTypeId = departmentOperationVM.OperationTypeID,
                        FormatUrl = "Not Required",
                        CategoryId = departmentOperationVM.OperationCategoryID
                    };
                    _context.Add(departmentOperation);
                    _context.SaveChanges();
                    _notyf.Success("Operation created successfully !!!!");
                }

                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                _notyf.Warning(" "+ex.Message.ToString()+" ");
                return View(departmentOperation);   
            }                      
        }

               
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentOperation = await _context.DepartmentOperations.FirstOrDefaultAsync(m => m.Id == id);
            if (departmentOperation == null)
            {
                return NotFound();
            }

            _context.DepartmentOperations.Remove(departmentOperation);
            _context.SaveChanges();
            _notyf.Warning("Operation deleted successfully !!!!");
            return RedirectToAction(nameof(Index));            
        }                
    }
}
