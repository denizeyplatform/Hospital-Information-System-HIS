using HIS.Domain.Aggregates.PatientAggregate.DomainEvents;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using HIS.Domain.Aggregates.PatientAggregate.Enums;
using HIS.Domain.Aggregates.PatientAggregate.Specifications;
using HIS.Domain.Aggregates.PatientAggregate.ValueObject;
using HIS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIS.Domain.Aggregates.PatientAggregate.Entities
{
    public sealed class Patient : AggregateRoot<Guid>
    {
      
        public MRN MRN { get; private set; }
        public PersonName FullName { get; private set; }
        public NationalIdentity NationalIdentity { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }

        public DateOnly DateOfBirth { get; private set; }
        public int Gender { get; private set; }

        //[NotMapped]
        public PatientStatus Status { get; private set; }
        public bool IsActive => Status == PatientStatus.Active; // true // false
        public InsurancePolicy? InsurancePolicy { get; private set; }

        private readonly List<MedicalRecord> _medicalRecords = new();
        private readonly List<DocumentRecord> _documents = new();
        private readonly List<AuditLog> _auditLogs = new();

        public IReadOnlyCollection<MedicalRecord> MedicalRecords => _medicalRecords;
        public IReadOnlyCollection<DocumentRecord> Documents => _documents;

        
        private Patient() { }

        public static Patient Create(PersonName fullName,NationalIdentity nationalIdentity,PhoneNumber phoneNumber,
                    Address address,DateOnly dateOfBirth,int gender, InsurancePolicy? insurancePolicy = null)
        {
            ValidateAge(dateOfBirth);

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                MRN = MRN.Generate(),
                FullName = fullName,
                NationalIdentity = nationalIdentity,
                PhoneNumber = phoneNumber,
                Address = address,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                InsurancePolicy = insurancePolicy,
                Status = PatientStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            patient.RaiseDomainEvent(new PatientCreatedDomainEvent(patient.Id));

            return patient;
        }

        public void UpdatePersonalInfo(PersonName fullName,PhoneNumber phoneNumber,Address address,int gender)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Address = address;
            Gender = gender;

            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new PatientUpdatedDomainEvent(Id));
        }

        public void UpdateInsurance(InsurancePolicy insurancePolicy)
        {
            InsurancePolicy = insurancePolicy;

            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new PatientUpdatedDomainEvent(Id));
        }

        public void AttachDocument(DocumentRecord document)
        {
            _documents.Add(document);

            RaiseDomainEvent(new DocumentUploadedDomainEvent(Id, document.Id));
        }

        public void AddMedicalHistoryEntry(MedicalRecord medicalRecord)
        {
            _medicalRecords.Add(medicalRecord);

            RaiseDomainEvent(new MedicalHistoryAddedDomainEvent(Id, medicalRecord.Id));
        }

        public void Deactivate(bool hasActiveAppointments)
        {
            if (hasActiveAppointments)
                throw new Exception();

            Status = PatientStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new PatientDeactivatedDomainEvent(Id));
        }

        private static void ValidateAge(DateOnly dateOfBirth)
        {
            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new Exception("Date of birth must be in the past.");
        }
        private void Validate(string fullName, DateTime dateOfBirth)
       {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Patient name is required");

            if (dateOfBirth > DateTime.UtcNow)
                throw new ArgumentException("Invalid birth date");
       }

        public int GetAge()
        {
            return DateTime.UtcNow.Year - DateOfBirth.Year;
        }

        public void ChangeAddress(Address address)
        {
            Address = address;
        }

        public void ChangePhone(PhoneNumber phone)
        {
            if (phone == null)
                throw new ArgumentException("Phone is required");

            PhoneNumber = phone;
        }

      
    }
}
