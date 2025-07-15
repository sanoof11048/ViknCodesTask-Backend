using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        [EmailAddress, Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
