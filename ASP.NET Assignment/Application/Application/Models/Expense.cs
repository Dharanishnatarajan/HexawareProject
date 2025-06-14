using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Application.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }  // This will be auto-generated

        [Precision(18, 2)]
        public decimal value { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Category { get; set; }
    }
}
