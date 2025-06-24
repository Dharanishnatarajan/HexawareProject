using HotPot.DTOs;

namespace HotPot.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Login(LoginDTO loginDTO);
        Task<AuthResponseDTO> Register(RegisterDTO registerDTO);
    }
}