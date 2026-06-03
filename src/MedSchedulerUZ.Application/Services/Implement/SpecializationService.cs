using AutoMapper;
using MedSchedulerUZ.Application.Models.SpecializationModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class SpecializationService : ISpecializationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public SpecializationService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateSpecializationResponseModel>> CreateAsync(CreateSpecializationModel model)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == model.DepartmentId);
            if (department is null)
                return ApiResult<CreateSpecializationResponseModel>.Failure(["Bo'lim topilmadi"]);

            var exists = await _context.Specializations
                .AnyAsync(s => s.Name == model.Name && s.IsActive);
            if (exists)
                return ApiResult<CreateSpecializationResponseModel>.Failure(["Bu nomli mutaxassislik allaqachon mavjud"]);

            var specialization = _mapper.Map<Specialization>(model);
            specialization.IsActive = true;

            await _context.Specializations.AddAsync(specialization);
            await _context.SaveChangesAsync();

            return ApiResult<CreateSpecializationResponseModel>.Success(
                new CreateSpecializationResponseModel { Id = specialization.Id });
        }

        public async Task<ApiResult<UpdateSpecializationResponseModel>> UpdateAsync(Guid id, UpdateSpecializationModel model)
        {
            var specialization = await _context.Specializations.FirstOrDefaultAsync(s => s.Id == id);
            if (specialization is null)
                return ApiResult<UpdateSpecializationResponseModel>.Failure(["Mutaxassislik topilmadi"]);

            specialization.Name = model.Name;
            specialization.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return ApiResult<UpdateSpecializationResponseModel>.Success(
                new UpdateSpecializationResponseModel { Id = specialization.Id });
        }

        public async Task<ApiResult<SpecializationResponseModel>> GetByIdAsync(Guid id)
        {
            var specialization = await _context.Specializations
                .Include(s => s.Department)
                .ThenInclude(d => d.Hospital)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (specialization is null)
                return ApiResult<SpecializationResponseModel>.Failure(["Mutaxassislik topilmadi"]);

            return ApiResult<SpecializationResponseModel>.Success(
                _mapper.Map<SpecializationResponseModel>(specialization));
        }

        public async Task<ApiResult<List<SpecializationResponseModel>>> GetAllAsync()
        {
            var specializations = await _context.Specializations
                .Include(s => s.Department)
                .ThenInclude(d => d.Hospital)
                .Where(s => s.IsActive)
                .ToListAsync();

            return ApiResult<List<SpecializationResponseModel>>.Success(
                _mapper.Map<List<SpecializationResponseModel>>(specializations));
        }

        public async Task<ApiResult<List<SpecializationResponseModel>>> GetByDepartmentAsync(Guid departmentId)
        {
            var specs = await _context.Specializations
                .Include(d => d.Department)
                .ThenInclude(h => h.Hospital)
                .Where(s => s.DepartmentId == departmentId && s.IsActive)
                .ToListAsync();
            return ApiResult<List<SpecializationResponseModel>>.Success(
                _mapper.Map<List<SpecializationResponseModel>>(specs));
        }
    }
}
