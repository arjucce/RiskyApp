using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class Email
    {
        [Key]
        public int Id { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }
    }
}
