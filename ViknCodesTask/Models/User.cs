using System.ComponentModel.DataAnnotations;
using ViknCodesTask.Common;

namespace ViknCodesTask.Models
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Role { get; set; } = "Employee";
    }
}
