using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name ="Department Name")]        
        public string Name { get; set; }


    }
}
