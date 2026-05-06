using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Core.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MedSchedulerUZ.Application.Helpers
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var authOptions = configuration.GetSection("JwtSettings").Get<JwtOption>();
            var secretKey = Encoding.UTF8.GetBytes(authOptions!.SecretKey);

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = authOptions.Issuer,
                        ValidAudience = authOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        ClockSkew = TimeSpan.Zero,

                        RoleClaimType = CustomClaimNames.Role,
                        NameClaimType = CustomClaimNames.Id
                    };
                });

            serviceCollection.AddAuthorization(options =>
            {
                options.AddPolicy("RequireSuperAdmin", policy =>
                    policy.RequireClaim(CustomClaimNames.Role,
                        nameof(UserRole.SuperAdmin)));

                options.AddPolicy("RequireHospitalAdmin", policy =>
                    policy.RequireClaim(CustomClaimNames.Role,
                        nameof(UserRole.HospitalAdmin)));

                options.AddPolicy("RequireDeptHead", policy =>
                    policy.RequireClaim(CustomClaimNames.Role,
                        nameof(UserRole.DeptHead)));

                options.AddPolicy("SuperAdminOrHospitalAdmin", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == CustomClaimNames.Role &&
                            (c.Value == nameof(UserRole.SuperAdmin) ||
                             c.Value == nameof(UserRole.HospitalAdmin)))));

                options.AddPolicy("ManagementOnly", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == CustomClaimNames.Role &&
                            (c.Value == nameof(UserRole.SuperAdmin) ||
                             c.Value == nameof(UserRole.HospitalAdmin) ||
                             c.Value == nameof(UserRole.DeptHead)))));
            });

            return serviceCollection;
        }
    }
}
