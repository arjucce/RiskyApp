using Microsoft.AspNetCore.Mvc;
using RiskyApp.Data;
using RiskyApp.Models.VM;

namespace RiskyApp.Areas.Admin.Controllers
{
    [Area("User")]
    public class DataController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public DataVM DataVM { get; set; }

        public DataController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(int departmentId, int categoryId)
        {
            ViewBag.DepName = (from a in _context.Departments where a.Id == departmentId select a.Name).SingleOrDefault();            
            var vm = (from a in _context.DepartmentOperations.Where(x => x.DepartmentId == departmentId && x.CategoryId == categoryId).ToList()
                      join b in _context.Departments.Where(a => a.Id == departmentId).ToList() on a.DepartmentId equals b.Id
                      join c in _context.OpTypes.ToList() on a.OperationTypeId equals c.Id orderby a.OperationTypeId 
                      select new DataVM()
                      {
                          DepartmentID = b.Id,
                          DepartmentName = b.Name,
                          OperationTypeID = c.Id,
                          OperationType = c.Type,
                          OperationName = a.OperationName,
                          Amount = 0,
                          FormatUrl=a.FormatUrl,
                          CategoryId=a.CategoryId
                      }).ToList();

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(List<RiskyApp.Models.VM.DataVM> dps)
        {
            if (ModelState.IsValid)
            {
                int i = 0;
                string dt = HttpContext.Request.Form["dt"].ToString();
                DateTime date = Convert.ToDateTime(dt, System.Globalization.CultureInfo.InvariantCulture);

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
               //for Upload file 
                foreach (var file in files)
                {                    
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"Upload\Data");
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    Models.Data data = new Models.Data()
                    {
                        OperationTitle = dps[i].OperationName,
                        Amount = dps[i].Amount,
                        Status = "Upload",
                        Url = @"\Upload\Data\" + fileName + extension,
                        ProcessDate = date,
                        DepartmentId = dps[i].DepartmentID,
                        CategoryId = dps[i].CategoryId
                    };
                    _context.Data.Add(data);
                    _context.SaveChanges();                    
                    i++;
                }

                ///For Input File
                foreach (DataVM dvm in dps)
                {
                    if(dvm.OperationType == "Input")
                    {
                        Models.Data data = new Models.Data()
                        {
                            OperationTitle = dvm.OperationName,
                            Amount = dvm.Amount,
                            Status = "Input",
                            Url = "Not Required",
                            ProcessDate = date,
                            DepartmentId = dvm.DepartmentID,
                            CategoryId = dps[i].CategoryId
                        };
                        _context.Data.Add(data);
                        _context.SaveChanges();
                    }                    
                }

                return RedirectToAction("Index","Home", new { area ="User"});
            }

            return View();
        }

        public FileResult Download(string Format)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var FileVirtualPath = Path.Combine(webRootPath, Format);
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));            
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

    }
}
