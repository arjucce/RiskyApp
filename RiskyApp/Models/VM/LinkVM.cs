using Microsoft.AspNetCore.Mvc.Rendering;

namespace RiskyApp.Models.VM
{
    public class LinkVM
    {
        public Link Links { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set;}
        public int DepartmentId { get; set; }
    }
}
