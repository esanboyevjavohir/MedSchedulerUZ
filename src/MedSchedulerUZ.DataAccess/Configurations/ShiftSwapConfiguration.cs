using MedSchedulerUZ.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MedSchedulerUZ.DataAccess.Configurations
{
    public class ShiftSwapConfiguration : IEntityTypeConfiguration<ShiftSwap>
    {
        public void Configure(EntityTypeBuilder<ShiftSwap> builder)
        {
            builder.HasOne(s => s.Requester)
                .WithMany()
                .HasForeignKey(s => s.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Acceptor)
                .WithMany()
                .HasForeignKey(s => s.AcceptorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Approver)
                .WithMany()
                .HasForeignKey(s => s.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Shift)
                .WithMany()
                .HasForeignKey(s => s.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
