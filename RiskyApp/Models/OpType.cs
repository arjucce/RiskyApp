using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class OpType
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
    }
}
