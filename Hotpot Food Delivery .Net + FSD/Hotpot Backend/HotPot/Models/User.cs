using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HotPot.Models
{
    public class User : IdentityUser<int>
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [Required, StringLength(20)]
        public string Role { get; set; }
    }
}
