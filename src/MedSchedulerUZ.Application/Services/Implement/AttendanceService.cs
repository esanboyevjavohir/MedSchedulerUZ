using AutoMapper;
using MedSchedulerUZ.Application.Models.AttendanceModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.Core.Enums;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class AttendanceService : IAttendanceService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public AttendanceService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<AttendanceResponseModel>> ClockInAsync(Guid userId, ClockInModel model)
        {
            var shift = await _context.Shifts
                .FirstOrDefaultAsync(s => s.Id == model.ShiftId);
            if (shift is null)
                return ApiResult<AttendanceResponseModel>.Failure(["Smena topilmadi"]);

            // QR token tekshiruv
            if (shift.QrToken != model.QrToken)
                return ApiResult<AttendanceResponseModel>.Failure(["QR kod noto'g'ri"]);

            // Xodim o'z smenasiga kirayaptimi
            if (shift.UserId != userId)
                return ApiResult<AttendanceResponseModel>.Failure(["Bu smena sizga tegishli emas"]);

            // Smena tugaganmi
            var shiftEnd = shift.ShiftDate.Add(shift.EndTime);
            if (DateTime.UtcNow > shiftEnd)
                return ApiResult<AttendanceResponseModel>.Failure(["Smena tugagan"]);

            // Allaqachon clock-in qilganmi
            var existing = await _context.Attendances
                .FirstOrDefaultAsync(a => a.ShiftId == model.ShiftId && a.UserId == userId);
            if (existing?.ClockIn != null)
                return ApiResult<AttendanceResponseModel>.Failure(["Siz allaqachon kirish belgilagansiz"]);

            // Status aniqlash
            var shiftStart = shift.ShiftDate.Add(shift.StartTime);
            var status = DateTime.UtcNow <= shiftStart.AddMinutes(15)
                ? AttendanceStatus.Present
                : AttendanceStatus.Late;

            var attendance = new Attendance
            {
                UserId = userId,
                ShiftId = model.ShiftId,
                ClockIn = DateTime.UtcNow,
                Status = status,
                CreatedOn = DateTime.UtcNow
            };

            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<AttendanceResponseModel>(attendance);
            return ApiResult<AttendanceResponseModel>.Success(response);
        }

        public async Task<ApiResult<AttendanceResponseModel>> ClockOutAsync(Guid userId, ClockOutModel model)
        {
            var shift = await _context.Shifts
                .FirstOrDefaultAsync(s => s.Id == model.ShiftId);
            if (shift is null)
                return ApiResult<AttendanceResponseModel>.Failure(["Smena topilmadi"]);

            // QR token tekshiruv
            if (shift.QrToken != model.QrToken)
                return ApiResult<AttendanceResponseModel>.Failure(["QR kod noto'g'ri"]);

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.ShiftId == model.ShiftId && a.UserId == userId);
            if (attendance is null)
                return ApiResult<AttendanceResponseModel>.Failure(["Avval kirish belgilang"]);

            if (attendance.ClockOut != null)
                return ApiResult<AttendanceResponseModel>.Failure(["Siz allaqachon chiqish belgilagansiz"]);

            // Early leave tekshiruv
            var shiftEnd = shift.ShiftDate.Add(shift.EndTime);
            if (DateTime.UtcNow < shiftEnd.AddMinutes(-15))
                attendance.Status = AttendanceStatus.EarlyLeave;

            attendance.ClockOut = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = _mapper.Map<AttendanceResponseModel>(attendance);
            return ApiResult<AttendanceResponseModel>.Success(response);
        }

        public async Task<ApiResult<AttendanceResponseModel>> GetByIdAsync(Guid id)
        {
            var attendance = await _context.Attendances
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (attendance is null)
                return ApiResult<AttendanceResponseModel>.Failure(["Davomat topilmadi"]);

            return ApiResult<AttendanceResponseModel>.Success(
                _mapper.Map<AttendanceResponseModel>(attendance));
        }

        public async Task<ApiResult<List<AttendanceResponseModel>>> GetByUserIdAsync(Guid userId)
        {
            var attendances = await _context.Attendances
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync();

            return ApiResult<List<AttendanceResponseModel>>.Success(
                _mapper.Map<List<AttendanceResponseModel>>(attendances));
        }

        public async Task<ApiResult<List<AttendanceResponseModel>>> GetByShiftIdAsync(Guid shiftId)
        {
            var attendances = await _context.Attendances
                .Include(a => a.User)
                .Where(a => a.ShiftId == shiftId)
                .ToListAsync();

            return ApiResult<List<AttendanceResponseModel>>.Success(
                _mapper.Map<List<AttendanceResponseModel>>(attendances));
        }
    }
}
