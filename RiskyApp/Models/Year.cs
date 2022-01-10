

using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class Year
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string YearName { get; set; }
    }
}
