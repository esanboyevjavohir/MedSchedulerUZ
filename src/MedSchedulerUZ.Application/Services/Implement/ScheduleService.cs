using AutoMapper;
using MedSchedulerUZ.Application.Models.ScheduleModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.Core.Enums;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class ScheduleService : IScheduleService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public ScheduleService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateScheduleResponseModel>> CreateAsync(CreateScheduleModel model)
        {
            var hospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == model.HospitalId);
            if (hospital is null)
                return ApiResult<CreateScheduleResponseModel>.Failure(["Kasalxona topilmadi"]);

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == model.DepartmentId);
            if (department is null)
                return ApiResult<CreateScheduleResponseModel>.Failure(["Bo'lim topilmadi"]);

            var exists = await _context.Schedules.AnyAsync(s =>
                s.DepartmentId == model.DepartmentId &&
                s.WeekStart == model.WeekStart);
            if (exists)
                return ApiResult<CreateScheduleResponseModel>.Failure(["Bu hafta uchun jadval allaqachon mavjud"]);

            var schedule = _mapper.Map<Schedule>(model);
            schedule.Status = ScheduleStatus.Draft;

            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();

            return ApiResult<CreateScheduleResponseModel>.Success(new CreateScheduleResponseModel { Id = schedule.Id });
        }

        public async Task<ApiResult<UpdateScheduleResponseModel>> UpdateAsync(Guid id, UpdateScheduleModel model)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule is null)
                return ApiResult<UpdateScheduleResponseModel>.Failure(["Jadval topilmadi"]);

            schedule.WeekStart = model.WeekStart;
            schedule.WeekEnd = model.WeekEnd;
            schedule.Status = model.Status;
            schedule.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResult<UpdateScheduleResponseModel>.Success(new UpdateScheduleResponseModel { Id = schedule.Id });
        }

        public async Task<ApiResult<ScheduleResponseModel>> GetByIdAsync(Guid id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Hospital)
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule is null)
                return ApiResult<ScheduleResponseModel>.Failure(["Jadval topilmadi"]);

            var response = _mapper.Map<ScheduleResponseModel>(schedule);
            response.HospitalName = schedule.Hospital.Name;
            response.DepartmentName = schedule.Department.Name;
            return ApiResult<ScheduleResponseModel>.Success(response);
        }

        public async Task<ApiResult<List<ScheduleResponseModel>>> GetAllAsync()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Hospital)
                .Include(s => s.Department)
                .ToListAsync();

            var response = schedules.Select(s =>
            {
                var dto = _mapper.Map<ScheduleResponseModel>(s);
                dto.HospitalName = s.Hospital.Name;
                dto.DepartmentName = s.Department.Name;
                return dto;
            }).ToList();

            return ApiResult<List<ScheduleResponseModel>>.Success(response);
        }

        public async Task<ApiResult<List<ScheduleResponseModel>>> GetByDepartmentIdAsync(Guid departmentId)
        {
            var schedules = await _context.Schedules
                .Include(s => s.Hospital)
                .Include(s => s.Department)
                .Where(s => s.DepartmentId == departmentId)
                .ToListAsync();

            var response = schedules.Select(s =>
            {
                var dto = _mapper.Map<ScheduleResponseModel>(s);
                dto.HospitalName = s.Hospital.Name;
                dto.DepartmentName = s.Department.Name;
                return dto;
            }).ToList();

            return ApiResult<List<ScheduleResponseModel>>.Success(response);
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule is null)
                return ApiResult<bool>.Failure(["Jadval topilmadi"]);

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }
    }
}
