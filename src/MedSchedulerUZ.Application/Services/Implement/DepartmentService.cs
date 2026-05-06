using AutoMapper;
using MedSchedulerUZ.Application.Models.DepartmentModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class DepartmentService : IDepartmentService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateDepartmentResponseModel>> CreateAsync(CreateDepartmentModel model)
        {
            var hospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == model.HospitalId);
            if (hospital is null)
                return ApiResult<CreateDepartmentResponseModel>.Failure(["Kasalxona topilmadi"]);

            var exists = await _context.Departments
                .AnyAsync(d => d.Code == model.Code && d.HospitalId == model.HospitalId);
            if (exists)
                return ApiResult<CreateDepartmentResponseModel>.Failure(["Bu kodli bo'lim allaqachon mavjud"]);

            var department = _mapper.Map<Department>(model);
            department.IsActive = true;

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            var response = new CreateDepartmentResponseModel { Id = department.Id };
            return ApiResult<CreateDepartmentResponseModel>.Success(response);
        }

        public async Task<ApiResult<UpdateDepartmentResponseModel>> UpdateAsync(Guid id, UpdateDepartmentModel model)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department is null)
                return ApiResult<UpdateDepartmentResponseModel>.Failure(["Bo'lim topilmadi"]);

            department.Name = model.Name;
            department.Code = model.Code;
            department.MinStaffRequired = model.MinStaffRequired;
            department.IsActive = model.IsActive;
            department.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new UpdateDepartmentResponseModel { Id = department.Id };
            return ApiResult<UpdateDepartmentResponseModel>.Success(response);
        }

        public async Task<ApiResult<DepartmentResponseModel>> GetByIdAsync(Guid id)
        {
            var department = await _context.Departments
                .Include(d => d.Hospital)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department is null)
                return ApiResult<DepartmentResponseModel>.Failure(["Bo'lim topilmadi"]);

            var response = _mapper.Map<DepartmentResponseModel>(department);
            response.HospitalName = department.Hospital.Name;
            return ApiResult<DepartmentResponseModel>.Success(response);
        }

        public async Task<ApiResult<List<DepartmentResponseModel>>> GetAllAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.Hospital)
                .Where(d => d.IsActive)
                .ToListAsync();

            var response = departments.Select(d =>
            {
                var dto = _mapper.Map<DepartmentResponseModel>(d);
                dto.HospitalName = d.Hospital.Name;
                return dto;
            }).ToList();

            return ApiResult<List<DepartmentResponseModel>>.Success(response);
        }

        public async Task<ApiResult<List<DepartmentResponseModel>>> GetByHospitalIdAsync(Guid hospitalId)
        {
            var departments = await _context.Departments
                .Include(d => d.Hospital)
                .Where(d => d.HospitalId == hospitalId && d.IsActive)
                .ToListAsync();

            var response = departments.Select(d =>
            {
                var dto = _mapper.Map<DepartmentResponseModel>(d);
                dto.HospitalName = d.Hospital.Name;
                return dto;
            }).ToList();

            return ApiResult<List<DepartmentResponseModel>>.Success(response);
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department is null)
                return ApiResult<bool>.Failure(["Bo'lim topilmadi"]);

            department.IsActive = false;
            department.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }
    }
}
