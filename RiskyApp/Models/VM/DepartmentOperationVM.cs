using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models.VM
{
    public class DepartmentOperationVM
    {
        public DepartmentOperation DepartmentOperations { get; set; }
        public OpType OperationType { get; set; }        

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> OperationTypeList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
                
        [Required]
        [Display(Name ="Department Name")]
        public int DepartmentID { get; set; }
        [Required]
        [Display(Name = "Type of Operation")]
        public int OperationTypeID { get; set; }
        [Required]
        [Display(Name = "Operation Category Name")]
        public int OperationCategoryID { get; set; }

        public int DepartmentOperationID { get; set; }
        public string DepartmentName { get; set; }
        public string OperationTypeName { get; set; }
        public string OperationName { get; set; }
        public string FormatUrl { get; set; }
        public string OperationCategory { get; set; }
    }
}
