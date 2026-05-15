using FluentValidation;
using MedSchedulerUZ.Application.Email;
using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using MedSchedulerUZ.Application.MappingProfiles;
using MedSchedulerUZ.Application.Models.User;
using MedSchedulerUZ.Application.Services.Background;
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

            services.AddHostedService<CertificationExpiryBackgroundService>();

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
            services.AddScoped<ILeaveRequestService, LeaveRequestService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<ICertificationService, CertificationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IShiftSwapService, ShiftSwapService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IValidator<CreateUserModel>, CreateUserValidator>();
            services.AddScoped<IValidator<ResetPasswordModel>, ResetPasswordValidator>();
            services.AddScoped<IValidator<ChangePasswordModel>, ChangePasswordValidator>();
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
            services.Configure<UserSettings>(configuration.GetSection("UserSettings"));
        }
    }
}
