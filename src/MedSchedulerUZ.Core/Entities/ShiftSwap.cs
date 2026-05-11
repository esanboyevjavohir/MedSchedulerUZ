using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedSchedulerUZ.Core.Entities
{
    public class ShiftSwap : BaseEntity // Smena almashish so'rovi
    {
        public Guid RequesterId { get; set; } // smena almashishni so'ragan xodim
        public Guid? AcceptorId { get; set; } // smena almashishni qabul qilgan xodim
        public Guid ShiftId { get; set; } // qaysi smena almashtirilmoqda
        public SwapStatus Status { get; set; } = SwapStatus.Pending;
        public string Reason { get; set; } // smena almashish sababi. Masalan: "Kasal bo'lib qoldim"
        public Guid? ApprovedBy { get; set; } // so'rovni kim tasdiqlagan — HospitalAdmin yoki DeptHead
        public DateTime Deadline { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ApprovedAt { get; set; } // tasdiqlangan vaqt

        // Navigation properties
        [ForeignKey("RequesterId")]
        public User Requester { get; set; }

        [ForeignKey("AcceptorId")]
        public User? Acceptor { get; set; }

        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

        [ForeignKey("ApprovedBy")]
        public User? Approver { get; set; }
    }
}
