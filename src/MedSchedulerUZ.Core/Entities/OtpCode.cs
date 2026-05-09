using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;

namespace MedSchedulerUZ.Core.Entities
{
    public class OtpCode : BaseEntity // Bir martalik tasdiqlash kodi
    {
        public OtpCode() { }
        public string Code { get; set; } = null!; // yuborilgan tasdiqlash kodi. Masalan: "4829"
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public OtpCodeStatus Status { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
