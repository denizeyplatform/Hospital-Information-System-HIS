using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Presestance
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("UpdatedAt");
            }
        }




        public DbSet<Patient> Patients => Set<Patient>();
       // public DbSet<Appointment> Appointments => Set<Appointment>();

        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

        public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();

        public DbSet<DocumentRecord> Documents => Set<DocumentRecord>();

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
    }
}
