using AutoMapper;
using MedSchedulerUZ.Application.Models.ShiftSwapModel;
using MedSchedulerUZ.Application.Models;
using MedSchedulerUZ.Application.Services.Interface;
using MedSchedulerUZ.Core.Entities;
using MedSchedulerUZ.Core.Enums;
using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.Application.Services.Implement
{
    public class ShiftSwapService : IShiftSwapService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public ShiftSwapService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<RequestSwapResponseModel>> RequestSwapAsync(Guid requesterId, RequestSwapModel model)
        {
            var shift = await _context.Shifts
                .FirstOrDefaultAsync(s => s.Id == model.ShiftId && s.UserId == requesterId);
            if (shift is null)
                return ApiResult<RequestSwapResponseModel>.Failure(["Smena topilmadi yoki bu smena sizga tegishli emas"]);

            if (shift.Status == ShiftStatus.Completed || shift.Status == ShiftStatus.Cancelled)
                return ApiResult<RequestSwapResponseModel>.Failure(["Bu smena uchun almashish so'rovi yuborib bo'lmaydi"]);

            var exists = await _context.ShiftSwaps
                .AnyAsync(s => s.ShiftId == model.ShiftId &&
                              s.Status != SwapStatus.Rejected);
            if (exists)
                return ApiResult<RequestSwapResponseModel>.Failure(["Bu smena uchun allaqachon so'rov mavjud"]);

            var shiftStart = shift.ShiftDate.Add(shift.StartTime);
            var deadline = shiftStart.AddHours(-3);

            if (DateTime.UtcNow >= deadline)
                return ApiResult<RequestSwapResponseModel>.Failure(["Smena boshlanishiga 3 soatdan kam vaqt qoldi, so'rov yuborib bo'lmaydi"]);

            var swap = new ShiftSwap
            {
                RequesterId = requesterId,
                ShiftId = model.ShiftId,
                Reason = model.Reason,
                Status = SwapStatus.Pending,
                Deadline = deadline,
                CreatedOn = DateTime.UtcNow
            };

            await _context.ShiftSwaps.AddAsync(swap);
            await _context.SaveChangesAsync();

            return ApiResult<RequestSwapResponseModel>.Success(
                new RequestSwapResponseModel { Id = swap.Id });
        }

        public async Task<ApiResult<AcceptSwapResponseModel>> AcceptSwapAsync(Guid swapId, Guid acceptorId)
        {
            var swap = await _context.ShiftSwaps
                .Include(s => s.Shift)
                .FirstOrDefaultAsync(s => s.Id == swapId);
            if (swap is null)
                return ApiResult<AcceptSwapResponseModel>.Failure(["So'rov topilmadi"]);

            if (swap.Status != SwapStatus.Pending)
                return ApiResult<AcceptSwapResponseModel>.Failure(["Bu so'rov allaqachon ko'rib chiqilgan"]);

            if (swap.RequesterId == acceptorId)
                return ApiResult<AcceptSwapResponseModel>.Failure(["O'z so'rovingizni qabul qila olmaysiz"]);

            if (DateTime.UtcNow >= swap.Deadline)
                return ApiResult<AcceptSwapResponseModel>.Failure(["So'rov muddati tugagan"]);

            // Acceptor shu kunda boshqa smenasi bormi
            var conflict = await _context.Shifts.AnyAsync(s =>
                s.UserId == acceptorId &&
                s.ShiftDate == swap.Shift.ShiftDate &&
                s.Status != ShiftStatus.Cancelled);
            if (conflict)
                return ApiResult<AcceptSwapResponseModel>.Failure(["Siz bu kunda allaqachon smenaga tayinlangansiz"]);

            swap.AcceptorId = acceptorId;
            swap.Status = SwapStatus.Accepted;
            await _context.SaveChangesAsync();

            return ApiResult<AcceptSwapResponseModel>.Success(
                new AcceptSwapResponseModel { Id = swap.Id });
        }

        public async Task<ApiResult<ApproveSwapResponseModel>> ApproveSwapAsync(Guid swapId, Guid approverId)
        {
            var swap = await _context.ShiftSwaps
                .Include(s => s.Shift)
                .FirstOrDefaultAsync(s => s.Id == swapId);
            if (swap is null)
                return ApiResult<ApproveSwapResponseModel>.Failure(["So'rov topilmadi"]);

            if (swap.Status != SwapStatus.Accepted)
                return ApiResult<ApproveSwapResponseModel>.Failure(["So'rov hali xodim tomonidan qabul qilinmagan"]);

            // Shiftni yangi xodimga o'tkazish
            swap.Shift.UserId = swap.AcceptorId!.Value;
            swap.Shift.Status = ShiftStatus.Swapped;

            swap.Status = SwapStatus.Approved;
            swap.ApprovedBy = approverId;
            swap.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResult<ApproveSwapResponseModel>.Success(
                new ApproveSwapResponseModel { Id = swap.Id });
        }

        public async Task<ApiResult<ApproveSwapResponseModel>> AssignSwapAsync(Guid swapId, Guid acceptorId, Guid approverId)
        {
            var swap = await _context.ShiftSwaps
                .Include(s => s.Shift)
                .FirstOrDefaultAsync(s => s.Id == swapId);
            if (swap is null)
                return ApiResult<ApproveSwapResponseModel>.Failure(["So'rov topilmadi"]);

            if (swap.Status != SwapStatus.Pending)
                return ApiResult<ApproveSwapResponseModel>.Failure(["Bu so'rov allaqachon ko'rib chiqilgan"]);

            if (DateTime.UtcNow < swap.Deadline)
                return ApiResult<ApproveSwapResponseModel>.Failure(["Muddat hali tugamagan, xodimlar qabul qilishini kuting"]);

            // Acceptor shu kunda boshqa smenasi bormi
            var conflict = await _context.Shifts.AnyAsync(s =>
                s.UserId == acceptorId &&
                s.ShiftDate == swap.Shift.ShiftDate &&
                s.Status != ShiftStatus.Cancelled);
            if (conflict)
                return ApiResult<ApproveSwapResponseModel>.Failure(["Bu xodim shu kunda allaqachon smenaga tayinlangan"]);

            swap.Shift.UserId = acceptorId;
            swap.Shift.Status = ShiftStatus.Swapped;

            swap.AcceptorId = acceptorId;
            swap.Status = SwapStatus.Approved;
            swap.ApprovedBy = approverId;
            swap.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ApiResult<ApproveSwapResponseModel>.Success(
                new ApproveSwapResponseModel { Id = swap.Id });
        }

        public async Task<ApiResult<List<ShiftSwapResponseModel>>> GetPendingAsync()
        {
            var swaps = await _context.ShiftSwaps
                .Include(s => s.Requester)
                .Include(s => s.Acceptor)
                .Include(s => s.Approver)
                .Include(s => s.Shift)
                .Where(s => s.Status == SwapStatus.Pending || s.Status == SwapStatus.Accepted)
                .OrderBy(s => s.Deadline)
                .ToListAsync();

            return ApiResult<List<ShiftSwapResponseModel>>.Success(
                _mapper.Map<List<ShiftSwapResponseModel>>(swaps));
        }

        public async Task<ApiResult<List<ShiftSwapResponseModel>>> GetByUserIdAsync(Guid userId)
        {
            var swaps = await _context.ShiftSwaps
                .Include(s => s.Requester)
                .Include(s => s.Acceptor)
                .Include(s => s.Approver)
                .Include(s => s.Shift)
                .Where(s => s.RequesterId == userId || s.AcceptorId == userId)
                .OrderByDescending(s => s.CreatedOn)
                .ToListAsync();

            return ApiResult<List<ShiftSwapResponseModel>>.Success(
                _mapper.Map<List<ShiftSwapResponseModel>>(swaps));
        }
    }
}
