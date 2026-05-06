using MedSchedulerUZ.Core.Common;
using MedSchedulerUZ.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedSchedulerUZ.Core.Entities
{
    public class ShiftSwap : BaseEntity, IAuditedEntity
    {
        public Guid RequesterId { get; set; }
        public Guid? AcceptorId { get; set; }
        public Guid ShiftId { get; set; }
        public SwapStatus Status { get; set; } = SwapStatus.Pending;
        public string Reason { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

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
