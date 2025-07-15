using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.DTOs.AuthDTOs
{
    public class LoginDTO
    {

        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

}
