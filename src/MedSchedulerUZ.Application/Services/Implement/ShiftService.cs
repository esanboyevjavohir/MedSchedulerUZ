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

        public async Task<ApiResult<string>> GetQrTokenAsync(Guid shiftId)
        {
            var shift = await _context.Shifts
                .FirstOrDefaultAsync(s => s.Id == shiftId);
            if (shift is null)
                return ApiResult<string>.Failure(["Smena topilmadi"]);

            return ApiResult<string>.Success(shift.QrToken);
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

            if (model.ShiftDate.Date < DateTime.UtcNow.Date)
                return ApiResult<CreateShiftResponseModel>.Failure(["O'tgan kunlarga smena tayinlab bo'lmaydi"]);

            // Xodim shu kunda boshqa smenasi bormi tekshir
            var conflict = await _context.Shifts.AnyAsync(s =>
                s.UserId == model.UserId &&
                s.ShiftDate == model.ShiftDate &&
                s.Status != ShiftStatus.Cancelled);
            if (conflict)
                return ApiResult<CreateShiftResponseModel>.Failure(["Xodim bu kunda allaqachon smenaga tayinlangan"]);

            var shift = _mapper.Map<Shift>(model);
            shift.QrToken = Guid.NewGuid().ToString("N"); 
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
            await _context.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
        }

        public async Task<ApiResult<AutoGenerateShiftResponseModel>> AutoGenerateAsync(AutoGenerateShiftModel model)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.Id == model.ScheduleId);
            if (schedule is null)
                return ApiResult<AutoGenerateShiftResponseModel>.Failure(["Jadval topilmadi"]);

            var employees = await _context.Users
                .Where(u => u.DepartmentId == model.DepartmentId
                         && u.IsActive
                         && u.RoleType == UserRole.Employee)
                .ToListAsync();

            if (!employees.Any())
                return ApiResult<AutoGenerateShiftResponseModel>.Failure(
                    ["Bo'limda faol xodimlar topilmadi"]);

            var weekStart = model.WeekStart.Date;
            var allDays = Enumerable.Range(0, 7).Select(i => weekStart.AddDays(i)).ToList();
            var workDays = allDays.Where(d => d.DayOfWeek != DayOfWeek.Saturday &&
                                               d.DayOfWeek != DayOfWeek.Sunday).ToList();
            var saturday = allDays.First(d => d.DayOfWeek == DayOfWeek.Saturday);
            var sunday = allDays.First(d => d.DayOfWeek == DayOfWeek.Sunday);

            var approvedLeaves = await _context.LeaveRequests
                .Where(l => l.Status == LeaveStatus.Approved
                         && l.StartDate.Date <= allDays.Last().Date
                         && l.EndDate.Date >= allDays.First().Date)
                .Select(l => new { l.UserId, l.StartDate, l.EndDate })
                .ToListAsync();

            var existingShifts = await _context.Shifts
                .Where(s => s.DepartmentId == model.DepartmentId
                         && s.ShiftDate >= allDays.First()
                         && s.ShiftDate <= allDays.Last()
                         && s.Status != ShiftStatus.Cancelled)
                .Select(s => new { s.UserId, s.ShiftDate })
                .ToListAsync();

            var created = 0;
            var skipped = 0;
            var warnings = new List<string>();
            var newShifts = new List<Shift>();

            bool IsOnLeave(Guid userId, DateTime day) => approvedLeaves.Any(l =>
                l.UserId == userId &&
                l.StartDate.Date <= day.Date &&
                l.EndDate.Date >= day.Date);

            bool HasExistingShift(Guid userId, DateTime day) =>
                existingShifts.Any(s => s.UserId == userId && s.ShiftDate.Date == day.Date);

            // Har xodimning dam olish kuni (Du-Ju orasidan random)
            var rng = new Random();
            var empDayOff = new Dictionary<Guid, DateTime>();

            // ── 1. Shanba/Yakshanba uchun navbatchilarni belgilash ──────────────────
            // Shanba: 2 navbatchi, Yakshanba: 2 navbatchi
            // Har xodim faqat 1 ta dam olish kuniga ega bo'lishi uchun:
            // Shanba navbatchisi → Yakshanba bo'sh
            // Yakshanba navbatchisi → Shanba bo'sh
            // Ikkala kuni ham navbatchi bo'lmaganlarga → 5 ish kuni (dam olish yo'q)

            var saturdayOnCallIds = new HashSet<Guid>();
            var sundayOnCallIds = new HashSet<Guid>();

            // Shanba uchun 2 ta navbatchi tanlash
            var satExisting = existingShifts
                .Where(s => s.ShiftDate.Date == saturday.Date)
                .Select(s => s.UserId).ToHashSet();

            int navbatchiPerDay = employees.Count <= 3 ? 1 : 2;
            int satNeed = Math.Max(0, navbatchiPerDay - satExisting.Count);
            var satCandidates = employees
                .Where(e => !satExisting.Contains(e.Id) && !IsOnLeave(e.Id, saturday))
                .OrderBy(_ => rng.Next())
                .Take(satNeed)
                .ToList();

            foreach (var emp in satCandidates)
            {
                saturdayOnCallIds.Add(emp.Id);
                newShifts.Add(new Shift
                {
                    Id = Guid.NewGuid(),
                    ScheduleId = model.ScheduleId,
                    UserId = emp.Id,
                    DepartmentId = model.DepartmentId,
                    ShiftDate = saturday,
                    StartTime = TimeSpan.Parse("08:00"),
                    EndTime = TimeSpan.Parse("20:00"),
                    ShiftType = ShiftType.OnCall,
                    Status = ShiftStatus.Scheduled,
                    QrToken = Guid.NewGuid().ToString("N"),
                    IsOnCall = true,
                });
                created++;
            }

            if (satNeed > satCandidates.Count)
                warnings.Add($"Shanba kuni faqat {satExisting.Count + satCandidates.Count} ta navbatchi topildi ({navbatchiPerDay} ta kerak)");

            // Yakshanba uchun 2 ta navbatchi tanlash
            // (Shanba navbatchilaridan BOSHQA xodimlarni afzal ko'ramiz)
            var sunExisting = existingShifts
                .Where(s => s.ShiftDate.Date == sunday.Date)
                .Select(s => s.UserId).ToHashSet();

            int sunNeed = Math.Max(0, navbatchiPerDay - sunExisting.Count);
            var sunCandidates = employees
                .Where(e => !sunExisting.Contains(e.Id)
                         && !saturdayOnCallIds.Contains(e.Id)   // Shanbada navbatchi bo'lmaganlar
                         && !IsOnLeave(e.Id, sunday))
                .OrderBy(_ => rng.Next())
                .Take(sunNeed)
                .ToList();

            // Agar yetarli xodim yo'q bo'lsa, shanbadan ham olish
            if (sunCandidates.Count < sunNeed)
            {
                var extra = employees
                    .Where(e => !sunExisting.Contains(e.Id)
                             && !sundayOnCallIds.Contains(e.Id)
                             && !sunCandidates.Any(c => c.Id == e.Id)
                             && !IsOnLeave(e.Id, sunday))
                    .OrderBy(_ => rng.Next())
                    .Take(sunNeed - sunCandidates.Count)
                    .ToList();
                sunCandidates.AddRange(extra);
            }

            foreach (var emp in sunCandidates)
            {
                sundayOnCallIds.Add(emp.Id);
                newShifts.Add(new Shift
                {
                    Id = Guid.NewGuid(),
                    ScheduleId = model.ScheduleId,
                    UserId = emp.Id,
                    DepartmentId = model.DepartmentId,
                    ShiftDate = sunday,
                    StartTime = TimeSpan.Parse("08:00"),
                    EndTime = TimeSpan.Parse("20:00"),
                    ShiftType = ShiftType.OnCall,
                    Status = ShiftStatus.Scheduled,
                    QrToken = Guid.NewGuid().ToString("N"),
                    IsOnCall = true,
                });
                created++;
            }

            if (sunNeed > sunCandidates.Count)
                warnings.Add($"Yakshanba kuni faqat {sunExisting.Count + sunCandidates.Count} ta navbatchi topildi ({navbatchiPerDay} ta kerak)");

            // ── 2. Har navbatchi uchun Du-Ju orasidan random dam olish kuni ──────────
            // Shanba navbatchisi: Yakshanba dam oladi (allaqachon), shuning uchun 
            // Du-Ju'dan 1 kun dam olishi kerak YO'Q — u shanbani hisobga olib 5 kun ishlaydi
            // Lekin Shanba 1 smena + Du-Ju 5 smena = 6 ta bo'lib qoladi!
            // Shuning uchun navbatchi xodim Du-Ju dan 1 kuni dam olishi KERAK.

            var allOnCallIds = new HashSet<Guid>(saturdayOnCallIds.Union(sundayOnCallIds));

            foreach (var empId in allOnCallIds)
            {
                // Ta'tilda bo'lmagan ish kunlaridan random 1 tani dam olish kuni sifatida belgilash
                var availableWorkDays = workDays
                    .Where(d => !IsOnLeave(empId, d) && !HasExistingShift(empId, d))
                    .ToList();

                if (availableWorkDays.Any())
                {
                    var dayOff = availableWorkDays[rng.Next(availableWorkDays.Count)];
                    empDayOff[empId] = dayOff;
                }
            }

            // ── 3. Du-Ju: har xodimga smena ──────────────────────────────────────────
            foreach (var day in workDays)
            {
                var shiftType = (day.DayOfWeek == DayOfWeek.Monday ||
                                 day.DayOfWeek == DayOfWeek.Wednesday ||
                                 day.DayOfWeek == DayOfWeek.Friday)
                    ? ShiftType.Day
                    : ShiftType.Night;

                var alreadyAssigned = existingShifts
                    .Where(s => s.ShiftDate.Date == day.Date)
                    .Select(s => s.UserId)
                    .ToHashSet();

                foreach (var emp in employees)
                {
                    if (alreadyAssigned.Contains(emp.Id)) { skipped++; continue; }

                    if (IsOnLeave(emp.Id, day))
                    {
                        warnings.Add($"{emp.FullName} - {day:dd.MM} kuni ta'tilda");
                        continue;
                    }

                    // Navbatchi xodim: belgilangan dam olish kunida ishlaydi
                    if (empDayOff.TryGetValue(emp.Id, out var dayOff) &&
                        dayOff.Date == day.Date)
                    {
                        // Dam olish kuni — smena qo'shmaymiz
                        continue;
                    }

                    newShifts.Add(new Shift
                    {
                        Id = Guid.NewGuid(),
                        ScheduleId = model.ScheduleId,
                        UserId = emp.Id,
                        DepartmentId = model.DepartmentId,
                        ShiftDate = day,
                        StartTime = shiftType == ShiftType.Day
                                           ? TimeSpan.Parse("08:00")
                                           : TimeSpan.Parse("20:00"),
                        EndTime = shiftType == ShiftType.Day
                                           ? TimeSpan.Parse("20:00")
                                           : TimeSpan.Parse("08:00"),
                        ShiftType = shiftType,
                        Status = ShiftStatus.Scheduled,
                        QrToken = Guid.NewGuid().ToString("N"),
                        IsOnCall = false,
                    });

                    alreadyAssigned.Add(emp.Id);
                    created++;
                }
            }

            if (newShifts.Any())
            {
                await _context.Shifts.AddRangeAsync(newShifts);
                await _context.SaveChangesAsync();
            }

            return ApiResult<AutoGenerateShiftResponseModel>.Success(new AutoGenerateShiftResponseModel
            {
                CreatedCount = created,
                SkippedCount = skipped,
                Warnings = warnings
            });
        }
    }
}
