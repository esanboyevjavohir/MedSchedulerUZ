using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.Helpers.GenerateJWT
{
    public interface IJwtTokenHandler
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
