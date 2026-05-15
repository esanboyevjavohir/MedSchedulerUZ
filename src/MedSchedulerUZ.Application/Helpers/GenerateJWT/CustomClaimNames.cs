using System.Security.Claims;

namespace MedSchedulerUZ.Application.Helpers.GenerateJWT
{
    public class CustomClaimNames
    {
        public const string Email = "email";
        public const string Role = ClaimTypes.Role;
        public const string Id = "id";
    }
}
