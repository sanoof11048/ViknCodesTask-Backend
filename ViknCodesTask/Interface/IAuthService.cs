using ViknCodesTask.Common;
using ViknCodesTask.DTOs.AuthDTOs;

namespace ViknCodesTask.Interface
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDTO>> RegisterAsync(RegisterDTO dto);
        Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginDTO dto);
    }
}
