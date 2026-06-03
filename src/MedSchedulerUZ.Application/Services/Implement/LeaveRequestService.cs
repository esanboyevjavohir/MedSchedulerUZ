using AutoMapper;
using MedSchedulerUZ.Application.Models.LeaveRequestModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.Core.Enums;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public LeaveRequestService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<CreateLeaveRequestResponseModel>> CreateAsync(Guid userId, CreateLeaveRequestModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
            if (user is null)
                return ApiResult<CreateLeaveRequestResponseModel>.Failure(["Xodim topilmadi"]);

            // Sanalar to'g'rimi
            if (model.StartDate >= model.EndDate)
                return ApiResult<CreateLeaveRequestResponseModel>.Failure(["Boshlanish sanasi tugash sanasidan oldin bo'lishi kerak"]);

            if (model.StartDate < DateTime.Today)
                return ApiResult<CreateLeaveRequestResponseModel>.Failure(["O'tgan sana uchun ta'til so'rovi yuborib bo'lmaydi"]);

            // Shu davr uchun allaqachon so'rov bormi
            var conflict = await _context.LeaveRequests.AnyAsync(l =>
                l.UserId == userId &&
                l.Status == LeaveStatus.Pending &&
                l.StartDate < model.EndDate &&
                l.EndDate > model.StartDate);
            if (conflict)
                return ApiResult<CreateLeaveRequestResponseModel>.Failure(["Bu davr uchun ta'til so'rovi allaqachon mavjud"]);

            var leaveRequest = new LeaveRequest
            {
                UserId = userId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                LeaveType = model.LeaveType,
                Reason = model.Reason,
                Status = LeaveStatus.Pending,
                CreatedOn = DateTime.UtcNow
            };

            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();

            return ApiResult<CreateLeaveRequestResponseModel>.Success(
                new CreateLeaveRequestResponseModel { Id = leaveRequest.Id });
        }

        public async Task<ApiResult<UpdateLeaveRequestResponseModel>> RespondAsync(Guid id, Guid approverId, 
            UpdateLeaveRequestModel model)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(l => l.Id == id);
            if (leaveRequest is null)
                return ApiResult<UpdateLeaveRequestResponseModel>.Failure(["Ta'til so'rovi topilmadi"]);

            if (leaveRequest.Status != LeaveStatus.Pending)
                return ApiResult<UpdateLeaveRequestResponseModel>.Failure(["Bu so'rov allaqachon ko'rib chiqilgan"]);

            leaveRequest.Status = model.Status;
            leaveRequest.ApprovedBy = approverId;
            leaveRequest.RespondedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResult<UpdateLeaveRequestResponseModel>.Success(
                new UpdateLeaveRequestResponseModel { Id = leaveRequest.Id });
        }

        public async Task<ApiResult<LeaveRequestResponseModel>> GetByIdAsync(Guid id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.User)
                .Include(l => l.Approver)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leaveRequest is null)
                return ApiResult<LeaveRequestResponseModel>.Failure(["Ta'til so'rovi topilmadi"]);

            return ApiResult<LeaveRequestResponseModel>.Success(
                _mapper.Map<LeaveRequestResponseModel>(leaveRequest));
        }

        public async Task<ApiResult<List<LeaveRequestResponseModel>>> GetByUserIdAsync(Guid userId)
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(l => l.User)
                .Include(l => l.Approver)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();

            return ApiResult<List<LeaveRequestResponseModel>>.Success(
                _mapper.Map<List<LeaveRequestResponseModel>>(leaveRequests));
        }

        public async Task<ApiResult<List<LeaveRequestResponseModel>>> GetAllAsync()
        {
            var leaves = await _context.LeaveRequests
                .Include(l => l.User)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();
            return ApiResult<List<LeaveRequestResponseModel>>.Success(
                _mapper.Map<List<LeaveRequestResponseModel>>(leaves));
        }

        public async Task<ApiResult<List<LeaveRequestResponseModel>>> GetPendingAsync()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(l => l.User)
                .Include(l => l.Approver)
                .Where(l => l.Status == LeaveStatus.Pending)
                .OrderBy(l => l.CreatedOn)
                .ToListAsync();

            return ApiResult<List<LeaveRequestResponseModel>>.Success(
                _mapper.Map<List<LeaveRequestResponseModel>>(leaveRequests));
        }
    }
}
