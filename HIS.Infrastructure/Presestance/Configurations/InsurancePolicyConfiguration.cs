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
    public sealed class InsurancePolicyConfiguration : IEntityTypeConfiguration<InsurancePolicy>
    {
        public void Configure(EntityTypeBuilder<InsurancePolicy> builder)
        {
            builder.ToTable("InsurancePolicies");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProviderName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.PolicyNumber)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.HasIndex(x => x.PolicyNumber)
                .IsUnique();
        }
    }
}
