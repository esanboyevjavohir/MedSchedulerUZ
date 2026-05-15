using AutoMapper;
using MedSchedulerUZ.Application.Models.CertificationModel;
using MedSchedulerUZ.Application.Models.NotificationModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.Core.Enums;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class CertificationService : ICertificationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public CertificationService(
            DatabaseContext context,
            IMapper mapper,
            INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ApiResult<AddCertificationResponseModel>> AddAsync(AddCertificationModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId && u.IsActive);
            if (user is null)
                return ApiResult<AddCertificationResponseModel>.Failure(["Xodim topilmadi"]);

            // Bir xil nomli sertifikat allaqachon bormi
            var exists = await _context.Certifications
                .AnyAsync(c => c.UserId == model.UserId && c.Name == model.Name);
            if (exists)
                return ApiResult<AddCertificationResponseModel>.Failure(["Bu nomli sertifikat allaqachon mavjud"]);

            var certification = _mapper.Map<Certification>(model);
            certification.CreatedOn = DateTime.UtcNow;
            certification.IsNotified = false;

            await _context.Certifications.AddAsync(certification);
            await _context.SaveChangesAsync();

            return ApiResult<AddCertificationResponseModel>.Success(
                new AddCertificationResponseModel { Id = certification.Id });
        }

        public async Task<ApiResult<List<CertificationResponseModel>>> GetByUserIdAsync(Guid userId)
        {
            var certifications = await _context.Certifications
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.ExpiryDate)
                .ToListAsync();

            return ApiResult<List<CertificationResponseModel>>.Success(
                _mapper.Map<List<CertificationResponseModel>>(certifications));
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id)
        {
            var certification = await _context.Certifications.FirstOrDefaultAsync(c => c.Id == id);
            if (certification is null)
                return ApiResult<bool>.Failure(["Sertifikat topilmadi"]);

            _context.Certifications.Remove(certification);
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }

        public async Task CheckExpiringAsync()
        {
            var expiring = await _context.Certifications
                .Include(c => c.User)
                .Where(c => c.ExpiryDate.HasValue &&
                            c.ExpiryDate.Value <= DateTime.UtcNow.AddDays(30) &&
                            c.ExpiryDate.Value > DateTime.UtcNow &&
                            !c.IsNotified)
                .ToListAsync();

            foreach (var cert in expiring)
            {
                await _notificationService.CreateAsync(new CreateNotificationModel
                {
                    UserId = cert.UserId,
                    Message = $"'{cert.Name}' sertifikatingizning muddati {cert.ExpiryDate!.Value:dd.MM.yyyy} da tugaydi. Iltimos, yangilang.",
                    Type = NotificationType.CertExpiry
                });

                cert.IsNotified = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
