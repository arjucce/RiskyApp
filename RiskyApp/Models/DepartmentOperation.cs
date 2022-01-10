using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class DepartmentOperation
    {
        [Key]
        public int Id { get; set; }                               
        [Required]
        [StringLength(100)]                
        [Display(Name ="Operation Name")]
        public string OperationName { get; set; }        
        public int OperationTypeId { get; set; }       
        public int DepartmentId { get; set; }
        public string FormatUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
