using AutoMapper;
using MedSchedulerUZ.Application.Models.ResponseModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class RoleService : IRoleService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public RoleService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateRoleResponseModel>> CreateAsync(CreateRoleModel model)
        {
            var exists = await _context.Roles
                .AnyAsync(r => r.Name == model.Name);
            if (exists)
                return ApiResult<CreateRoleResponseModel>.Failure(["Bu nomli role allaqachon mavjud"]);

            var role = _mapper.Map<Role>(model);

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return ApiResult<CreateRoleResponseModel>.Success(
                new CreateRoleResponseModel { Id = role.Id });
        }

        public async Task<ApiResult<UpdateRoleResponseModel>> UpdateAsync(Guid id, UpdateRoleModel model)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null)
                return ApiResult<UpdateRoleResponseModel>.Failure(["Role topilmadi"]);

            role.Name = model.Name;
            role.RoleType = model.RoleType;
            role.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResult<UpdateRoleResponseModel>.Success(
                new UpdateRoleResponseModel { Id = role.Id });
        }

        public async Task<ApiResult<RoleResponseModel>> GetByIdAsync(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null)
                return ApiResult<RoleResponseModel>.Failure(["Role topilmadi"]);

            return ApiResult<RoleResponseModel>.Success(_mapper.Map<RoleResponseModel>(role));
        }

        public async Task<ApiResult<List<RoleResponseModel>>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();

            return ApiResult<List<RoleResponseModel>>.Success(
                _mapper.Map<List<RoleResponseModel>>(roles));
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null)
                return ApiResult<bool>.Failure(["Role topilmadi"]);

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }
    }
}
