using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MedSchedulerUZ.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(GetSeedUsers());
        }

        const string seedSalt = "f67273d6-d1ee-4129-9740-75a8df1a5c5b";
        const string seedPassword = "Javohir@2606";

        const string testSalt = "medscheduler-seed-2026";
        const string testPassword = "Med@12345";

        private List<User> GetSeedUsers()
        {
            return new()
            {
                new User
                {
                    Id = new Guid("a0ae7f44-f3a2-4ea6-8030-01a4ea1b1ae3"),
                    FullName = "Esanboyev Javohir",
                    Email = "javohiresanboyev053@gmail.com",
                    PhoneNumber = "+998933116612",
                    RoleType = UserRole.SuperAdmin,
                    HospitalId = null,
                    DepartmentId = null,
                    SpecializationId = null,
                    IsActive = true,
                    MustChangePassword = false,
                    Salt = seedSalt,
                    PasswordHash = Encrypt(seedPassword, seedSalt),
                    CreatedOn = new DateTime(2026, 4, 11, 0, 0, 0, DateTimeKind.Utc)
                }
            };
        }

        private static string Encrypt(string password, string salt)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                iterations: 1000,
                hashAlgorithm: HashAlgorithmName.SHA256);
            return Convert.ToBase64String(algorithm.GetBytes(32));
        }
    }
}
