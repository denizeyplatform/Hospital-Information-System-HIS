using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.ValueObject;
using HIS.Domain.Interfaces.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Infrastructure.Presestance.Repository;
using HIS.Infrastructure.Presestance;

namespace HIS.Testing.Integration
{
    public class PatientTesting
    {
        public readonly PatientRepository _repository;
        public ApplicationDbContext context;
       
        public PatientTesting()
        {
            _repository = new PatientRepository(context);
        }


        [Fact]
        public void create_patient_test() 
        {
            // AAA

            // Arrange
           var p = Patient.Create(
                    fullName: new PersonName("John", "Doe"),
                    nationalIdentity: new NationalIdentity("29801011234567", Domain.Aggregates.PatientAggregate.Enums.IdentityDocumentType.NationalId),
                    phoneNumber: new PhoneNumber("+201012345688"),
                    address: new Address("123 Main St", "Anytown", "State"),
                    dateOfBirth: new DateOnly(1990, 1, 1) , 1
                    );

            // Act
             var result =  _repository.AddAsync(p);

            // Assert

           result.Wait();
           


        }
    }
}
