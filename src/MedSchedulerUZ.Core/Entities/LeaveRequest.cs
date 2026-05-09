using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedSchedulerUZ.Core.Entities
{
    public class LeaveRequest : BaseEntity // Ta'til/(Dam olish so'rovi)
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; } // ta'til turi
        public string Reason { get; set; } // ta'til sababi. Masalan: "Tizzam og'riyapti", "Oilaviy sharoit"
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending; // so'rovning holati
        public Guid? ApprovedBy { get; set; } // so'rovni kim tasdiqlagan yoki rad etgan — HospitalAdmin yoki DeptHead
        public DateTime CreatedOn { get; set; }
        public DateTime? RespondedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("ApprovedBy")]
        public User? Approver { get; set; }
    }
}
