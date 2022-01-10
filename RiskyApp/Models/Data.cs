using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class Data
    {
        [Key]
        public int Id { get; set; }        
        public string OperationTitle { get; set; }        
        [Required]
        [DisplayName("Amount")]
        [DataType(((double)DataType.Custom))]
        public double Amount { get; set; }                 
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Process Date")]
        public DateTime ProcessDate { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }
    }
}
