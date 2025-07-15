using System.ComponentModel.DataAnnotations;

namespace HotPot.Models
{
    public class MenuCategory
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
