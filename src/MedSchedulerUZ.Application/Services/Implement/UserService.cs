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
using MedSchedulerUZ.Application.Validators;

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
        private readonly IValidator<ChangePasswordModel> _changePasswordValidator;

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
            IValidator<ResetPasswordModel> resetPasswordValidator,
            IValidator<ChangePasswordModel> changePasswordValidator)
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
            _changePasswordValidator = changePasswordValidator;
        }

        public async Task<ApiResult<UserResponseModel>> GetMeAsync(Guid currentUserId)
        {
            var user = await _context.Users
                .Include(u => u.Specialization)
                .Include(u => u.Department)
                .Include(u => u.Hospital)
                .FirstOrDefaultAsync(u => u.Id == currentUserId && u.IsActive);

            if (user is null)
                return ApiResult<UserResponseModel>.Failure(["Foydalanuvchi topilmadi"]);

            return ApiResult<UserResponseModel>.Success(_mapper.Map<UserResponseModel>(user));
        }

        public async Task<ApiResult<bool>> UpdateProfileAsync(Guid currentUserId, UpdateProfileModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == currentUserId && u.IsActive);

            if (user is null)
                return ApiResult<bool>.Failure(["Foydalanuvchi topilmadi"]);

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<LoginResponseModel>> LoginAsync(LoginUserModel loginModel)
        {
            var user = await _context.Users
                .Include(u => u.OtpCodes)  // Role Include olib tashlandi
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.IsActive);
            if (user is null)
                return ApiResult<LoginResponseModel>.Failure(["Email yoki parol noto'g'ri"]);

            if (!_passwordHasher.Verify(user.PasswordHash, loginModel.Password, user.Salt))
                return ApiResult<LoginResponseModel>.Failure(["Email yoki parol noto'g'ri"]);

            //if (user.MustChangePassword)
            //{
            //    return ApiResult<LoginResponseModel>.Success(new LoginResponseModel
            //    {
            //        Id = user.Id,
            //        Email = user.Email,
            //        MustChangePassword = true
            //        // AccessToken va RefreshToken berilmaydi
            //    });
            //}

            //if (user.RoleType != UserRole.SuperAdmin && !user.MustChangePassword)
            //{
            //    var isEmailVerified = user.OtpCodes
            //        .Any(o => o.Status == OtpCodeStatus.Verified);
            //    if (!isEmailVerified)
            //        return ApiResult<LoginResponseModel>.Failure(["Email tasdiqlanmagan"]);
            //}

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
                MustChangePassword = user.MustChangePassword,
                ExpireAt = DateTime.UtcNow.AddMinutes(_jwtOption.ExpirationInMinutes),
                RefreshTokenExpireAt = DateTime.UtcNow.AddDays(_userSettings.RefreshTokenExpirationDays)
            });
        }

        private string GeneratePassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789@#$";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ApiResult<CreateUserResponseModel>> RegisterAsync(CreateUserModel model, string currentRole, Guid currentUserId)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (exists)
                return ApiResult<CreateUserResponseModel>.Failure(["Bu email allaqachon ro'yxatdan o'tgan"]);

            // HospitalAdmin faqat o'z shifoxonasiga qo'sha oladi
            if (currentRole == UserRole.HospitalAdmin.ToString())
            {
                var currentUser = await _context.Users.FindAsync(currentUserId);
                if (model.HospitalId != currentUser!.HospitalId)
                    return ApiResult<CreateUserResponseModel>.Failure(["Faqat o'z shifoxonangizga xodim qo'sha olasiz"]);
            }

            // DeptHead faqat o'z bo'limiga qo'sha oladi
            if (currentRole == UserRole.DeptHead.ToString())
            {
                var currentUser = await _context.Users.FindAsync(currentUserId);
                if (model.DepartmentId != currentUser!.DepartmentId)
                    return ApiResult<CreateUserResponseModel>.Failure(["Faqat o'z bo'limingizga xodim qo'sha olasiz"]);
            }

            // Role ga qarab tekshiruv
            if (model.RoleType == UserRole.DeptHead && !model.DepartmentId.HasValue)
                return ApiResult<CreateUserResponseModel>.Failure(["Bo'lim boshlig'i uchun DepartmentId majburiy"]);
            if (model.RoleType == UserRole.Employee && !model.DepartmentId.HasValue)
                return ApiResult<CreateUserResponseModel>.Failure(["Xodim uchun DepartmentId majburiy"]);
            if (model.RoleType == UserRole.Employee && !model.SpecializationId.HasValue)
                return ApiResult<CreateUserResponseModel>.Failure(["Xodim uchun SpecializationId majburiy"]);

            if (model.DepartmentId.HasValue)
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == model.DepartmentId);
                if (department is null)
                    return ApiResult<CreateUserResponseModel>.Failure(["Bo'lim topilmadi"]);
            }
            if (model.SpecializationId.HasValue)
            {
                var specialization = await _context.Specializations.FirstOrDefaultAsync(s => s.Id == model.SpecializationId);
                if (specialization is null)
                    return ApiResult<CreateUserResponseModel>.Failure(["Mutaxassislik topilmadi"]);
            }

            var generatedPassword = GeneratePassword();
            var user = _mapper.Map<User>(model);
            user.Salt = GenerateSalt();
            user.PasswordHash = _passwordHasher.Encrypt(generatedPassword, user.Salt);
            user.MustChangePassword = true;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await _emailService.SendPasswordAsync(model.Email, model.FullName, generatedPassword);
            var response = _mapper.Map<CreateUserResponseModel>(user);
            return ApiResult<CreateUserResponseModel>.Success(response);
        }

        public async Task<ApiResult<UserResponseModel>> GetByIdAsync(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Specialization)  // Role Include olib tashlandi
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return ApiResult<UserResponseModel>.Failure(["Foydalanuvchi topilmadi"]);

            var response = _mapper.Map<UserResponseModel>(user);
            return ApiResult<UserResponseModel>.Success(response);
        }

        public async Task<ApiResult<List<UserResponseModel>>> GetAllAsync(string role, Guid currentUserId)
        {
            IQueryable<User> query = _context.Users
                .Include(u => u.Specialization)
                .Include(u => u.Hospital)
                .Include(u => u.Department)
                .Where(u => u.IsActive);

            if (role == UserRole.SuperAdmin.ToString())
            {
                // Hammani ko'radi — filter yo'q
            }
            else if (role == UserRole.HospitalAdmin.ToString())
            {
                var currentUser = await _context.Users.FindAsync(currentUserId);
                query = query.Where(u =>
                    u.HospitalId == currentUser!.HospitalId &&
                    (u.RoleType == UserRole.DeptHead || u.RoleType == UserRole.Employee));
            }
            else if (role == UserRole.DeptHead.ToString())
            {
                var currentUser = await _context.Users.FindAsync(currentUserId);
                query = query.Where(u =>
                    u.DepartmentId == currentUser!.DepartmentId &&
                    u.RoleType == UserRole.Employee);
            }
            else
            {
                return ApiResult<List<UserResponseModel>>.Failure(["Ruxsat yo'q"]);
            }

            var users = await query.ToListAsync();
            var response = _mapper.Map<List<UserResponseModel>>(users);
            return ApiResult<List<UserResponseModel>>.Success(response);
        }

        public async Task<ApiResult<bool>> ChangePasswordAsync(Guid userId, ChangePasswordModel model)
        {
            var validationResult = await _changePasswordValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
                return ApiResult<bool>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

            if (user is null)
                return ApiResult<bool>.Failure(["Foydalanuvchi topilmadi"]);

            // Eski parolni tekshirish
            if (!_passwordHasher.Verify(user.PasswordHash, model.OldPassword, user.Salt))
                return ApiResult<bool>.Failure(["Eski parol noto'g'ri"]);

            // Yangi parol eski parol bilan bir xil bo'lmasligi kerak
            if (model.OldPassword == model.NewPassword)
                return ApiResult<bool>.Failure(["Yangi parol eski parol bilan bir xil bo'lmasligi kerak"]);

            user.PasswordHash = _passwordHasher.Encrypt(model.NewPassword, user.Salt);
            user.MustChangePassword = false;
            user.RefreshToken = null;
            user.RefreshTokenExpireDate = null;
            user.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<bool>> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return ApiResult<bool>.Failure(["Foydalanuvchi topilmadi"]);

            // SuperAdmin ni o'chirib bo'lmaydi
            if (user.RoleType == UserRole.SuperAdmin)
                return ApiResult<bool>.Failure(["Super adminni o'chirib bo'lmaydi"]);

            user.IsActive = false;
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

            bool isSent = await _emailService.SendOtpAsync(maybeUser.Email, optCode.Code);

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

        public async Task<ApiResult<bool>> ForgotPasswordAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
            if (user is null)
                return ApiResult<bool>.Failure(["Foydalanuvchi topilmadi"]);

            string tempPassword = GeneratePassword();
            user.ResetPasswordToken = _passwordHasher.Encrypt(tempPassword, user.Salt);
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddMinutes(10);
            await _context.SaveChangesAsync();

            bool isSent = await _emailService.SendResetPasswordAsync(user.Email, user.FullName, tempPassword);
            if (!isSent)
                return ApiResult<bool>.Failure(["Email yuborishda xatolik yuz berdi"]);

            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<bool>> ResetPasswordAsync(ResetPasswordModel model)
        {
            var validationResult = await _resetPasswordValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
                return ApiResult<bool>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);
            if (user is null)
                return ApiResult<bool>.Failure(["Foydalanuvchi topilmadi"]);

            if (user.ResetPasswordTokenExpiry < DateTime.UtcNow)
                return ApiResult<bool>.Failure(["Vaqtincha parol muddati tugagan"]);

            if (user.ResetPasswordToken != _passwordHasher.Encrypt(model.TemporaryPassword, user.Salt))
                return ApiResult<bool>.Failure(["Vaqtincha parol noto'g'ri"]);

            if (model.NewPassword != model.ConfirmPassword)
                return ApiResult<bool>.Failure(["Yangi parollar mos kelmadi"]);

            user.PasswordHash = _passwordHasher.Encrypt(model.NewPassword, user.Salt);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;
            user.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<TokenResponseModel>> ValidateAndRefreshToken(Guid id, string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            if (user is null)
                return ApiResult<TokenResponseModel>.Failure(["Foydalanuvchi topilmadi"]);

            if (user.RefreshToken != refreshToken)
                return ApiResult<TokenResponseModel>.Failure(["Refresh token noto'g'ri"]);

            if (user.RefreshTokenExpireDate < DateTime.UtcNow)
                return ApiResult<TokenResponseModel>.Failure(["Refresh token muddati tugagan, qayta kiring"]);

            var newRefreshToken = _jwtTokenHandler.GenerateRefreshToken();
            var newAccessToken = _jwtTokenHandler.GenerateAccessToken(user);

            user.RefreshToken = newRefreshToken;  // refreshToken o'rniga newRefreshToken
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(_userSettings.RefreshTokenExpirationDays);
            user.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResult<TokenResponseModel>.Success(new TokenResponseModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        public async Task<ApiResult<bool>> ResendOtpCode(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.OtpCodes)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                return ApiResult<bool>.Failure(["Foydalanuvchi topilmadi"]);

            // Allaqachon tasdiqlangan bo'lsa
            var isVerified = user.OtpCodes.Any(o => o.Status == OtpCodeStatus.Verified);
            if (isVerified)
                return ApiResult<bool>.Failure(["Email allaqachon tasdiqlangan"]);

            var lastOtp = user.OtpCodes
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();

            if (lastOtp is null)
                return ApiResult<bool>.Failure(["OTP topilmadi"]);

            // Qayta yuborishdan oldin kutish vaqti
            if (!CanResend(lastOtp.CreatedAt))
            {
                var waitTime = GetWaitTimeForResend(lastOtp.CreatedAt);
                return ApiResult<bool>.Failure([$"Iltimos {waitTime} soniya kuting"]);
            }

            // Muddati o'tmagan bo'lsa — eskisini qayta yuborish
            if (!IsExpired(lastOtp.CreatedAt))
            {
                bool isSent = await _emailService.SendOtpAsync(user.Email, lastOtp.Code);
                if (!isSent)
                    return ApiResult<bool>.Failure(["Email yuborishda xatolik"]);
                return ApiResult<bool>.Success(true);
            }

            // Muddati o'tgan bo'lsa — yangi kod yaratish
            var pendingCodes = user.OtpCodes
                .Where(o => o.Status == OtpCodeStatus.Unverified)
                .ToList();
            foreach (var code in pendingCodes)
                code.Status = OtpCodeStatus.Expired;

            var newOtp = new OtpCode
            {
                Code = OtpCodeHelper.GenerateOtpCode(),
                Status = OtpCodeStatus.Unverified,
                UserId = userId
            };
            user.OtpCodes.Add(newOtp);

            bool isSentNew = await _emailService.SendOtpAsync(user.Email, newOtp.Code);
            if (!isSentNew)
                return ApiResult<bool>.Failure(["Email yuborishda xatolik"]);

            await _context.SaveChangesAsync();
            return ApiResult<bool>.Success(true);
        }

        private bool CanResend(DateTimeOffset createdAt) =>
            createdAt.AddSeconds(
                _userSettings.OtpExpirationTimeInSeconds - _userSettings.OtpResendTimeInSeconds) < DateTimeOffset.Now;

        private int GetWaitTimeForResend(DateTimeOffset createdAt)
        {
            var resendTime = createdAt.AddSeconds(
                _userSettings.OtpExpirationTimeInSeconds - _userSettings.OtpResendTimeInSeconds);
            var waitTime = resendTime - DateTimeOffset.Now;
            return Math.Max(0, (int)waitTime.TotalSeconds);
        }
    }
}
