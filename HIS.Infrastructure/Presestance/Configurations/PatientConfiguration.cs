using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Presestance.Configurations
{
    public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.DateOfBirth)
                .IsRequired();

            builder.Property(x => x.Gender)
                .HasConversion<int>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.Ignore(x => x.IsActive);

            builder.Ignore("DomainEvents");

            // =========================
            // MRN Value Object
            // =========================

            builder.OwnsOne(x => x.MRN, mrn =>
            {
                mrn.Property(x => x.Value)
                    .HasColumnName("MRN")
                    .HasMaxLength(50)
                    .IsRequired();

                mrn.HasIndex(x => x.Value)
                    .IsUnique();
            });

            // =========================
            // PersonName Value Object
            // =========================

            builder.OwnsOne(x => x.FullName, name =>
            {
                name.Property(x => x.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(100)
                    .IsRequired();

                name.Property(x => x.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            // =========================
            // NationalIdentity Value Object
            // =========================

            builder.OwnsOne(x => x.NationalIdentity, identity =>
            {
                identity.Property(x => x.Value)
                    .HasColumnName("NationalIdentity")
                    .HasMaxLength(20)
                    .IsRequired();

                identity.Property(x => x.Type)
                    .HasColumnName("IdentityType")
                    .HasConversion<int>()
                    .HasMaxLength(20)
                    .IsRequired();

                identity.HasIndex(x => x.Value)
                    .IsUnique();
            });

            // =========================
            // PhoneNumber Value Object
            // =========================

            builder.OwnsOne(x => x.PhoneNumber, phone =>
            {
                phone.Property(x => x.Value)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20)
                    .IsRequired();
            });

            // =========================
            // Address Value Object
            // =========================

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(x => x.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(250)
                    .IsRequired();

                address.Property(x => x.City)
                    .HasColumnName("City")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(x => x.Country)
                    .HasColumnName("Country")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            // =========================
            // InsurancePolicy One-To-One
            // =========================

            builder.HasOne(x => x.InsurancePolicy)
                .WithOne()
                .HasForeignKey<InsurancePolicy>(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // MedicalRecords One-To-Many
            // =========================

            builder.HasMany(x => x.MedicalRecords)
                .WithOne()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Documents One-To-Many
            // =========================

            builder.HasMany(x => x.Documents)
                .WithOne()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // AuditLogs One-To-Many
            // =========================

            builder.Metadata
                .FindNavigation(nameof(Patient.MedicalRecords))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(Patient.Documents))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
