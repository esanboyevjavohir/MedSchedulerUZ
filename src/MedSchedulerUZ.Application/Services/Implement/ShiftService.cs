using AutoMapper;
using MedSchedulerUZ.Application.Models.ShiftModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.Core.Enums;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class ShiftService : IShiftService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public ShiftService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateShiftResponseModel>> CreateAsync(CreateShiftModel model)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == model.ScheduleId);
            if (schedule is null)
                return ApiResult<CreateShiftResponseModel>.Failure(["Jadval topilmadi"]);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId && u.IsActive);
            if (user is null)
                return ApiResult<CreateShiftResponseModel>.Failure(["Xodim topilmadi"]);

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == model.DepartmentId);
            if (department is null)
                return ApiResult<CreateShiftResponseModel>.Failure(["Bo'lim topilmadi"]);

            if (user.SpecializationId.HasValue)
            {
                var specialization = await _context.Specializations
                    .FirstOrDefaultAsync(s => s.Id == user.SpecializationId);
                if (specialization?.DepartmentId != model.DepartmentId)
                    return ApiResult<CreateShiftResponseModel>.Failure(
                        ["Xodim faqat o'z mutaxassisligiga mos bo'limga tayinlanishi mumkin"]);
            }

            // Xodim shu kunda boshqa smenasi bormi tekshir
            var conflict = await _context.Shifts.AnyAsync(s =>
                s.UserId == model.UserId &&
                s.ShiftDate == model.ShiftDate &&
                s.Status != ShiftStatus.Cancelled);
            if (conflict)
                return ApiResult<CreateShiftResponseModel>.Failure(["Xodim bu kunda allaqachon smenaga tayinlangan"]);

            var shift = _mapper.Map<Shift>(model);
            shift.Status = ShiftStatus.Scheduled;

            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();

            return ApiResult<CreateShiftResponseModel>.Success(new CreateShiftResponseModel { Id = shift.Id });
        }

        public async Task<ApiResult<UpdateShiftResponseModel>> UpdateAsync(Guid id, UpdateShiftModel model)
        {
            var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == id);
            if (shift is null)
                return ApiResult<UpdateShiftResponseModel>.Failure(["Smena topilmadi"]);

            shift.ShiftDate = model.ShiftDate;
            shift.StartTime = model.StartTime;
            shift.EndTime = model.EndTime;
            shift.ShiftType = model.ShiftType;
            shift.Status = model.Status;
            shift.IsOnCall = model.IsOnCall;
            shift.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResult<UpdateShiftResponseModel>.Success(new UpdateShiftResponseModel { Id = shift.Id });
        }

        public async Task<ApiResult<ShiftResponseModel>> GetByIdAsync(Guid id)
        {
            var shift = await _context.Shifts
                .Include(s => s.User)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shift is null)
                return ApiResult<ShiftResponseModel>.Failure(["Smena topilmadi"]);

            var response = _mapper.Map<ShiftResponseModel>(shift);
            return ApiResult<ShiftResponseModel>.Success(response);
        }

        public async Task<ApiResult<List<ShiftResponseModel>>> GetAllAsync()
        {
            var shifts = await _context.Shifts
                .Include(s => s.User)
                .Include(s => s.Department)
                .ToListAsync();

            var response = shifts.Select(s =>
            {
                var dto = _mapper.Map<ShiftResponseModel>(s);
                return dto;
            }).ToList();

            return ApiResult<List<ShiftResponseModel>>.Success(response);
        }

        public async Task<ApiResult<List<ShiftResponseModel>>> GetByUserIdAsync(Guid userId)
        {
            var shifts = await _context.Shifts
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.ShiftDate)
                .ToListAsync();

            var response = shifts.Select(s =>
            {
                var dto = _mapper.Map<ShiftResponseModel>(s);
                return dto;
            }).ToList();

            return ApiResult<List<ShiftResponseModel>>.Success(response);
        }

        public async Task<ApiResult<List<ShiftResponseModel>>> GetByScheduleIdAsync(Guid scheduleId)
        {
            var shifts = await _context.Shifts
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.ScheduleId == scheduleId)
                .OrderBy(s => s.ShiftDate)
                .ToListAsync();

            var response = shifts.Select(s =>
            {
                var dto = _mapper.Map<ShiftResponseModel>(s);
                return dto;
            }).ToList();

            return ApiResult<List<ShiftResponseModel>>.Success(response);
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id)
        {
            var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == id);
            if (shift is null)
                return ApiResult<bool>.Failure(["Smena topilmadi"]);

            shift.Status = ShiftStatus.Cancelled;
            shift.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }
    }
}
