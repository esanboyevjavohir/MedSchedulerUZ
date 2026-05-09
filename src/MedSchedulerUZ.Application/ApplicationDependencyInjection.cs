using FluentValidation;
using MedSchedulerUZ.Application.Email;
using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.MappingProfiles;
using MedSchedulerUZ.Application.Models.User;
using MedSchedulerUZ.Application.Services.Implement;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Application.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedSchedulerUZ.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,
            IWebHostEnvironment env, IConfiguration configuration)
        {
            services.AddServices(env);

            services.AddEmailConfiguration(configuration);

            services.AddJwtConfiguration(configuration);

            services.RegisterAutoMapper();

            services.RegisterCashing();

            return services;
        }

        private static void AddServices(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IValidator<CreateUserModel>, CreateUserValidator>();
            services.AddScoped<IValidator<ResetPasswordModel>, ResetPasswordValidator>();
        }

        private static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(IMappingProfilesMarker));
        }

        private static void RegisterCashing(this IServiceCollection services)
        {
            services.AddMemoryCache();
        }

        public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
        }

        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOption>(configuration.GetSection("JwtSettings"));
        }
    }
}
