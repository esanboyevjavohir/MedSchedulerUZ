using AutoMapper;
using MedSchedulerUZ.Application.Models.User;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.DataAccess.Persistence;
using MedSchedulerUZ.Application.Helpers.GenerateJWT;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MedSchedulerUZ.Core.Enums;
using MedSchedulerUZ.Application.Helpers;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtOption _jwtOption;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;
        private readonly UserSettings _userSettings;
        private readonly IValidator<CreateUserModel> _createUserValidator;
        private readonly IValidator<ResetPasswordModel> _resetPasswordValidator;

        public UserService(
            IMapper mapper,
            ILogger<UserService> logger,
            DatabaseContext databaseContext,
            IJwtTokenHandler jwtTokenHandler,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IOptions<UserSettings> userSettings,
            IOptions<JwtOption> jwtOption,            
            IValidator<CreateUserModel> createUserValidator,
            IValidator<ResetPasswordModel> resetPasswordValidator)
        {
            _mapper = mapper;
            _logger = logger;
            _context = databaseContext;
            _jwtTokenHandler = jwtTokenHandler;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _userSettings = userSettings.Value;
            _jwtOption = jwtOption.Value;             
            _createUserValidator = createUserValidator;
            _resetPasswordValidator = resetPasswordValidator;
        }

        public async Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel loginModel)
        {
            var user = await _context.Users
                .Include(u => u.Role)        // RoleType uchun kerak
                .Include(u => u.OtpCodes)
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.IsActive);

            if (user is null)
                return ApiResult<LoginResponseModel>.Failure(["Email yoki parol noto'g'ri"]);

            // SuperAdmin OTP tasdiqlamasdan kira oladi
            if (user.RoleType != UserRole.SuperAdmin)
            {
                var isEmailVerified = user.OtpCodes
                    .Any(o => o.Status == OtpCodeStatus.Verified);

                if (!isEmailVerified)
                    return ApiResult<LoginResponseModel>.Failure(["Email tasdiqlanmagan"]);
            }

            if (!_passwordHasher.Verify(user.PasswordHash, loginModel.Password, user.Salt))
                return ApiResult<LoginResponseModel>.Failure(["Email yoki parol noto'g'ri"]);

            var accessToken = _jwtTokenHandler.GenerateAccessToken(user);
            var refreshToken = _jwtTokenHandler.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(_userSettings.RefreshTokenExpirationDays);
            await _context.SaveChangesAsync();

            return ApiResult<LoginResponseModel>.Success(new LoginResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpireAt = DateTime.UtcNow.AddMinutes(_jwtOption.ExpirationInMinutes)
            });
        }

        public async Task<ApiResult<CreateUserResponseModel>> RegisterAsync(CreateUserModel model)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email == model.Email);

            if (exists)
                return ApiResult<CreateUserResponseModel>.Failure(["Bu email allaqachon ro'yxatdan o'tgan"]);

            var user = _mapper.Map<User>(model);

            // Salt generatsiya qilinadi va hash saqlanadi
            user.Salt = GenerateSalt();
            user.PasswordHash = _passwordHasher.Encrypt(model.Password, user.Salt);
            user.EmployeeCode = GenerateEmployeeCode();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<CreateUserResponseModel>(user);
            return ApiResult<CreateUserResponseModel>.Success(response);
        }

        public async Task<ApiResult<UserResponseModel>> GetByIdAsync(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Specialization)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return ApiResult<UserResponseModel>.Failure(["Foydalanuvchi topilmadi"]);

            var response = _mapper.Map<UserResponseModel>(user);
            return ApiResult<UserResponseModel>.Success(response);
        }

        public async Task<ApiResult<List<UserResponseModel>>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Specialization)
                .Where(u => u.IsActive)
                .ToListAsync();

            var response = _mapper.Map<List<UserResponseModel>>(users);
            return ApiResult<List<UserResponseModel>>.Success(response);
        }

        public async Task<ApiResult<bool>> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return ApiResult<bool>.Failure(["Foydalanuvchi topilmadi"]);

            user.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }

        private string GenerateSalt()
        {
            var bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string GenerateEmployeeCode()
        {
            return "EMP-" + Guid.NewGuid().ToString("N")[..6].ToUpper();
        }

        public async Task<ApiResult<bool>> SendOtpCode(Guid userId)
        {
            var maybeUser = await _context.Users
                .Include(a => a.OtpCodes)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (maybeUser == null)
            {
                return ApiResult<bool>.Failure(new List<string> { "User not found" });
            }

            var optCode = new OtpCode
            {
                Code = OtpCodeHelper.GenerateOtpCode(),
                Status = OtpCodeStatus.Unverified
            };

            maybeUser.OtpCodes.Add(optCode);

            bool isSent = await _emailService.SendEmailAsync(maybeUser.Email, optCode.Code);

            if (!isSent)
            {
                return ApiResult<bool>.Failure(new List<string> { "Failed to send OTP email" });
            }

            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }

        public bool IsExpired(DateTimeOffset createdAt) =>
            createdAt.AddSeconds(_userSettings.OtpExpirationTimeInSeconds) < DateTimeOffset.Now;

        public async Task<ApiResult<bool>> VerifyOtpCode(string code, Guid userId)
        {
            if (string.IsNullOrEmpty(code))
            {
                return ApiResult<bool>.Failure(new List<string> { "OTP code cannot be empty" });
            }

            var user = await _context.Users
                .Include(c => c.OtpCodes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return ApiResult<bool>.Failure(new List<string> { "User not found" });
            }

            var lastOtp = user.OtpCodes
                .Where(o => o.Status == OtpCodeStatus.Unverified)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();

            if (lastOtp == null)
            {
                return ApiResult<bool>.Failure(new List<string> { "No active OTP found" });
            }

            if (IsExpired(lastOtp.CreatedAt))
            {
                lastOtp.Status = OtpCodeStatus.Expired;
                await _context.SaveChangesAsync();
                return ApiResult<bool>.Failure(new List<string> { "OTP has expired" });
            }

            if (lastOtp.Code != code)
            {
                return ApiResult<bool>.Failure(new List<string> { "Invalid OTP code" });
            }

            lastOtp.Status = OtpCodeStatus.Verified;
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }
    }
}
