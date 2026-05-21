using AutoMapper;
using HIS.Application.DTO;
using HIS.Application.DTO.Common;
using HIS.Application.DTO.Documents;
using HIS.Application.DTO.MedicalHistory;
using HIS.Application.DTO.Patient;
using HIS.Application.Interface;
using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using HIS.Domain.Aggregates.PatientAggregate.ValueObject;
using HIS.Domain.Interfaces.Repository;
using HIS.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Service
{
    public class PatientService : IPatientService
    {
        public IPatientRepository _patientRepository;
        private readonly IFileStorageService _fileStorage;
        private readonly IAntivirusScanner _antivirus;
        public readonly IMapper _mapper;
        
        public PatientService(IPatientRepository patientRepository, 
            IFileStorageService fileStorage, IAntivirusScanner antivirus, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _fileStorage = fileStorage;
            _antivirus = antivirus;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(CreatePatientRequest request)
        {
            if (await _patientRepository.ExistsDuplicateActivePatientAsync(request.NationalIdentity, request.PhoneNumber))
            throw new InvalidOperationException("Duplicate active patient exists.");

            var patient = _mapper.Map<Patient>(request);
            await _patientRepository.AddAsync(patient);

            return patient.Id;
        }
        public async Task<PatientDetailsDto?> GetByIdAsync(Guid patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient is null)
                return null;

            return new PatientDetailsDto
            {
                Id = patient.Id,
                MRN = patient.MRN.Value,
                FullName = $"{patient.FullName.FirstName} {patient.FullName.LastName}",
                NationalIdentity = patient.NationalIdentity.Value,
                PhoneNumber = patient.PhoneNumber.Value,
                DateOfBirth = patient.DateOfBirth,
                Status = patient.Status.ToString(),
            };
        }
        public async Task UpdatePatientAsync(Guid patientId, UpdatePatientRequest request, CancellationToken cancellationToken = default)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient is null)
                throw new Exception("Patient is not exist");
            if (!patient.IsActive)
                throw new InvalidOperationException("Inactive patient.");
            var map =  _mapper.Map<Patient>(request);
            await _patientRepository.UpdateAsync(map);
            patient.UpdatePersonalInfo(patient.FullName, patient.PhoneNumber, patient.Address, patient.Gender);
        }
        public async Task UploadDocumentAsync(Guid patientId,UploadMedicalDocumentRequest request,CancellationToken cancellationToken = default)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient is null)
                throw new KeyNotFoundException("Patient not found.");

            if (!patient.IsActive)
                throw new InvalidOperationException("Inactive patient.");

            // 1. Antivirus scan (INFRA service)
            await _antivirus.ScanAsync(request.File.OpenReadStream(), cancellationToken);

            if (request.File.Length > 0)
                request.File.OpenReadStream().Position = 0;

            // 2. Upload file
            var fileUrl = await _fileStorage.UploadAsync(
                request.File.OpenReadStream(),
                request.File.FileName,
                cancellationToken);

            // 3. Create document

            string fileName = request.File.FileName;
            string mimeType = request.File.ContentType;
            long fileSize = request.File.Length;
            string storagePath = fileUrl;
            var document = new DocumentRecord(patientId, fileName, mimeType, fileSize, storagePath);

            // 4. Domain logic
            patient.AttachDocument(document);

           // await _patientRepository.Update(patient);
        }

        public Task<IReadOnlyCollection<DocumentDto>> GetPatientDocumentsAsync(Guid patientId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task AddMedicalHistoryAsync(Guid patientId,MedicalRecord record)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient is null)
                throw new KeyNotFoundException("Patient not found.");

            if (!patient.IsActive)
                throw new InvalidOperationException("Cannot add medical history to inactive patient.");

            patient.AddMedicalHistoryEntry(record);

            await _patientRepository.UpdateAsync(patient);
        }

        public async Task<PagedResult<PatientDto>> SearchAsync(
            PatientSearchRequest request,
            CancellationToken cancellationToken = default)
        {
            var patients = await _patientRepository.SearchPatientsAsync(
                request.Name,
                request.NationalIdentity,
                request.MRN,
                request.PhoneNumber,
                request.PageNumber,
                request.PageSize);

            var totalCount = patients.Count;
            var mappedPatients = patients.Select(p => new PatientDto
            {
                FullName = $"{p.FullName.FirstName} {p.FullName.LastName}",
                NationalIdentity = p.NationalIdentity.Value,
                PhoneNumber = p.PhoneNumber.Value,
                DateOfBirth = p.DateOfBirth,
            }).ToList();

            return PagedResult<PatientDto>.Create(
                mappedPatients,
                request.PageNumber,
                request.PageSize,
                totalCount);
        }
        public async Task DeactivatePatientAsync(Guid patientId, DeactivatePatientRequest request, CancellationToken cancellationToken = default)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient is null)
                throw new KeyNotFoundException("Patient not found.");

            var hasActiveAppointments =
                await _patientRepository.HasActiveAppointmentsAsync(patientId);

            patient.Deactivate(hasActiveAppointments);

            await _patientRepository.UpdateAsync(patient);
        }

       
    }
}
