using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedSchedulerUZ.Core.Entities
{
    public class LeaveRequest : BaseEntity, IAuditedEntity
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        public Guid? ApprovedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("ApprovedBy")]
        public User? Approver { get; set; }
    }
}
