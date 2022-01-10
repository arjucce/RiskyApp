using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Operation Category Name")]
        [StringLength(50)]
        public string Name { get; set; }    
    }
}
