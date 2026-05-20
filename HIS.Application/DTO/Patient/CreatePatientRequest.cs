using HIS.Application.DTO.Insurrance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.DTO.Patient
{
    public sealed class CreatePatientRequest
    {
        public string FirstName { get; init; } = default!;

        public string LastName { get; init; } = default!;
        public string FullName => $"{FirstName} {LastName}".Trim();

        public string NationalIdentity { get; init; } = default!;

        public string IdentityType { get; init; } = default!;

        public string PhoneNumber { get; init; } = default!;

        public string Street { get; init; } = default!;

        public string City { get; init; } = default!;

        public string Country { get; init; } = default!;
       // public string Address => $"{Street}, {City}, {Country}".Trim().Trim(',');

        public DateOnly DateOfBirth { get; init; }

        public int Gender { get; init; } = default!;

        // Optional Insurance

        public InsurancePolicyDto? InsurancePolicy { get; init; }
    }
}
