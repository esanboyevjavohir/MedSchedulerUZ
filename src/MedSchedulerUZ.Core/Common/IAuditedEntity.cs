namespace MedSchedulerUZ.Core.Common
{
    public interface IAuditedEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
