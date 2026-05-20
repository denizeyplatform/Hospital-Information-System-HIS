using AutoMapper;
using FluentAssertions;
using HIS.Application.DTO.Patient;
using HIS.Application.Service;
using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Enums;
using HIS.Domain.Aggregates.PatientAggregate.ValueObject;
using HIS.Domain.Interfaces.Repository;
using HIS.Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace HIS.TEST
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock = new Mock<IPatientRepository>();
        private readonly Mock<IFileStorageService> _fileStorageMock = new Mock<IFileStorageService>();
        private readonly Mock<IAntivirusScanner> _antivirusMock = new Mock<IAntivirusScanner>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();

            _service = new PatientService(
                _patientRepositoryMock.Object,
                _fileStorageMock.Object,
                _antivirusMock.Object,
                _mapperMock.Object);
        }
        [Fact]
        public async Task CreateAsync_ShouldCreatePatient_WhenPatientDoesNotExist()
        {
            // Arrange
            var request = new CreatePatientRequest
            {
                FirstName = "Ahmed",
                LastName = "Farag",
                NationalIdentity = "29801011234567",
                PhoneNumber = "+201012345688",
                Street = "Street 1",
                City = "Cairo",
                Country = "Egypt",
                DateOfBirth = new DateOnly(1998, 1, 1),
                Gender = 1
            };

            var patient = Patient.Create(
                new PersonName("Ahmed", "Farag"),
                new NationalIdentity("29801011234567", IdentityDocumentType.NationalId),
                new PhoneNumber("+201012345688"),
                new Address("Street 1", "Cairo", "Egypt"),
                new DateOnly(1998, 1, 1),
                1);

            _patientRepositoryMock
                .Setup(x => x.ExistsDuplicateActivePatientAsync(
                    request.NationalIdentity,
                    request.PhoneNumber))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map<Patient>(request))
                .Returns(patient);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            result.Should().NotBeEmpty();

            result.Should().Be(patient.Id);

            _patientRepositoryMock.Verify(
                x => x.AddAsync(patient),Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenDuplicatePatientExists()
        {
            // Arrange
            var request = new CreatePatientRequest
            {
                FirstName = "Aya",
                LastName = "Farag",
                NationalIdentity = "29801011234567",
                PhoneNumber = "+201012345678",
                Street = "Street 1",
                City = "Cairo",
                Country = "Egypt",
                DateOfBirth = new DateOnly(1998, 1, 1),
                Gender = 1
            };

            _patientRepositoryMock
                .Setup(x => x.ExistsDuplicateActivePatientAsync(
                    request.NationalIdentity,
                    request.PhoneNumber))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () =>
                await _service.CreateAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Duplicate active patient exists.");

            _patientRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatientDto_WhenPatientExists()
        {
            // Arrange
            var patientId = Guid.NewGuid();

            var patient = Patient.Create(
                new PersonName("Aya", "Farag"),
                new NationalIdentity("29801011234567", IdentityDocumentType.NationalId),
                new PhoneNumber("+201012345678"),
                new Address("Street 1", "Cairo", "Egypt"),
                new DateOnly(1998, 1, 1),
                1,
                null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    patientId))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.GetByIdAsync(patientId);

            // Assert
            result.Should().NotBeNull();

            result!.Id.Should().Be(patient.Id);

            result.FullName.Should().Be("Aya Farag");
            patient.FullName.FirstName.Should().Be("Aya");

            patient.FullName.LastName.Should().Be("Farag");

            result.PhoneNumber.Should().Be("+201012345678");
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldThrowException_WhenPatientNotFound()
        {
            // Arrange
            var patientId = Guid.NewGuid();

            var request = new UpdatePatientRequest
            {
                FirstName = "Aya",
                LastName = "Farag",
                PhoneNumber = "+201012345678",

                Street = "Street",
                City = "Cairo",
                Country = "Egypt",

                Gender = Gender.Female.ToString()
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    patientId))
                .ReturnsAsync((Patient?)null);

            // Act
            Func<Task> act = async () =>
                await _service.UpdatePatientAsync(
                    patientId,
                    request);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient is not exist");
        }


        [Fact]
        public async Task UpdatePatientAsync_ShouldThrowException_WhenPatientInactive()
        {
            // Arrange
            var patientId = Guid.NewGuid();

            var patient = Patient.Create(
                new PersonName("Aya", "Farag"),
                new NationalIdentity("29801011234567", IdentityDocumentType.NationalId),
                new PhoneNumber("+201012345678"),
                new Address("Street", "Cairo", "Egypt"),
                new DateOnly(1998, 1, 1),
                1,
                null);

            patient.Deactivate(false);

            var request = new UpdatePatientRequest
            {
                FirstName = "New",
                LastName = "Name",
                PhoneNumber = "+201012345678",

                Street = "New Street",
                City = "Giza",
                Country = "Egypt",

                Gender = Gender.Female.ToString(),
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    patientId))
                .ReturnsAsync(patient);

            // Act
            Func<Task> act = async () =>
                await _service.UpdatePatientAsync(
                    patientId,
                    request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Inactive patient.");
        }


    }
}
