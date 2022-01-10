using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models.VM
{
    public class EmailVM
    {
        [Required]
        [Display(Name = "Department Name")]
        public int DepartmentId { get; set; }
        [Required]        
        public string To { get; set; }                

        public string CC { get; set; }
        [Required]
        [Display(Name = "Operation Category Name")]
        public int CategoryId { get; set; }
        public string Month { get; set; }
        [Required]
        [Display(Name = "Period From")]
        public string PeriodFrom { get; set; }
        [Required]
        [Display(Name = "Period To")]
        public string PeriodTo { get; set; }
        [Required]
        [Display(Name = "Year")]
        public string Years { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<Months> Months { get; set; }
    }
}
