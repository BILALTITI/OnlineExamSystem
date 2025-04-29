using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "Student"
    }
}
