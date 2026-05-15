using AutoMapper;
using MedSchedulerUZ.Application.Models.HospitalModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class HospitalService : IHospitalService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public HospitalService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateHospitalResponseModel>> CreateAsync(CreateHospitalModel model)
        {
            var exists = await _context.Hospitals
                .AnyAsync(h => h.Name == model.Name && h.IsActive);
            if (exists)
                return ApiResult<CreateHospitalResponseModel>.Failure(["Bu nomli kasalxona allaqachon mavjud"]);

            var hospital = _mapper.Map<Hospital>(model);
            hospital.IsActive = true;

            await _context.Hospitals.AddAsync(hospital);
            await _context.SaveChangesAsync();

            return ApiResult<CreateHospitalResponseModel>.Success(
                new CreateHospitalResponseModel { Id = hospital.Id });
        }

        public async Task<ApiResult<UpdateHospitalResponseModel>> UpdateAsync(Guid id, UpdateHospitalModel model)
        {
            var hospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
            if (hospital is null)
                return ApiResult<UpdateHospitalResponseModel>.Failure(["Kasalxona topilmadi"]);

            hospital.Name = model.Name;
            hospital.Address = model.Address;
            hospital.Phone = model.Phone;
            hospital.Type = model.Type;
            hospital.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return ApiResult<UpdateHospitalResponseModel>.Success(
                new UpdateHospitalResponseModel { Id = hospital.Id });
        }

        public async Task<ApiResult<HospitalResponseModel>> GetByIdAsync(Guid id)
        {
            var hospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
            if (hospital is null)
                return ApiResult<HospitalResponseModel>.Failure(["Kasalxona topilmadi"]);

            return ApiResult<HospitalResponseModel>.Success(_mapper.Map<HospitalResponseModel>(hospital));
        }

        public async Task<ApiResult<List<HospitalResponseModel>>> GetAllAsync()
        {
            var hospitals = await _context.Hospitals
                .Where(h => h.IsActive)
                .ToListAsync();

            return ApiResult<List<HospitalResponseModel>>.Success(
                _mapper.Map<List<HospitalResponseModel>>(hospitals));
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id)
        {
            var hospital = await _context.Hospitals.FirstOrDefaultAsync(h => h.Id == id);
            if (hospital is null)
                return ApiResult<bool>.Failure(["Kasalxona topilmadi"]);

            hospital.IsActive = false;
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }
    }
}
