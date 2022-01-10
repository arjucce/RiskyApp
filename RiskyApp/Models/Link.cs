using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskyApp.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }        
        public string DataLink { get; set; }            
        public int DepartmentId { get; set; }
    }
}
