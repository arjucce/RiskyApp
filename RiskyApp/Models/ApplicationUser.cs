using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RiskyApp.Models
{
    public class ApplicationUser:IdentityUser
    {                
        public string  Name { get; set; }        
        public string Designation { get; set; }        
        public string Department { get; set; }
        public string Passcode { get; set; }
    }
}
