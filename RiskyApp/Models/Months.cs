using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class Months
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
