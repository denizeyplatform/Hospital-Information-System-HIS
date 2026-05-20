using HIS.Domain.Aggregates.PatientAggregate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValueObj = HIS.Domain.Common.ValueObject;

namespace HIS.Domain.Aggregates.PatientAggregate.ValueObject
{

    public sealed class NationalIdentity : ValueObj
    {
        public string Value { get; }

        public IdentityDocumentType Type { get; }

        private NationalIdentity()
        {
        }

        public NationalIdentity(string value,IdentityDocumentType type)
        {
            Value = value;
            Type = type;
        }

        public static NationalIdentity CreateNationalId(string nationalId)
        {
            nationalId = nationalId.Trim();

            ValidateEgyptianNationalId(nationalId);

            return new NationalIdentity(
                nationalId,
                IdentityDocumentType.NationalId);
        }

        public static NationalIdentity CreatePassport(string passportNumber)
        {
            passportNumber = passportNumber.Trim().ToUpper();

            ValidatePassport(passportNumber);

            return new NationalIdentity(
                passportNumber,
                IdentityDocumentType.Passport);
        }

        private static void ValidateEgyptianNationalId(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                throw new Exception(
                    "National ID is required.");

            if (!Regex.IsMatch(nationalId, @"^\d{14}$"))
                throw new Exception(
                    "Egyptian National ID must contain 14 digits.");

            var centuryDigit = nationalId[0];

            if (centuryDigit != '2' && centuryDigit != '3')
                throw new Exception(
                    "Invalid Egyptian National ID century digit.");
        }

        private static void ValidatePassport(string passportNumber)
        {
            if (string.IsNullOrWhiteSpace(passportNumber))
                throw new Exception(
                    "Passport number is required.");

            if (passportNumber.Length < 6 ||
                passportNumber.Length > 20)
            {
                throw new Exception(
                    "Passport number length invalid.");
            }

            if (!Regex.IsMatch(passportNumber, @"^[A-Z0-9]+$"))
            {
                throw new Exception(
                    "Passport format invalid.");
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Type;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
