using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HIS.Infrastructure.Presestance.Configurations
{
    public sealed class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.ToTable("MedicalRecords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Diagnosis)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Treatment)
                .HasMaxLength(1000);

            builder.Property(x => x.TreatingPhysician)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.RecordDate)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}
