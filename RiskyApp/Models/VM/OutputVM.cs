using Microsoft.AspNetCore.Mvc.Rendering;

namespace RiskyApp.Models.VM
{
    public class OutputVM
    {
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        //public IEnumerable<Data> Datas { get; set; }

        //public string OperationName { get; set; }
        //public string DepartmentName { get; set; }
        //public string CategoryName { get; set; }
        //public double Amount { get; set; }
        //public string Url { get; set; }
        //public DateTime ProcessDate { get; set; }
        
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }


    }
}
