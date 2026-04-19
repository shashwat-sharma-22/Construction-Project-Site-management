using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user);
    }
}