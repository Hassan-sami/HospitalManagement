using AutoMapper;
using Hospital.BLL.ModelVM;
using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Implementation
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        public async Task<List<MedicalRecordVm>> GetMedicalRecordsByPatientId(string patientId)
        {
            var medicalRecords = _medicalRecordRepository.GetAllMedicalRecords();
            return _mapper.Map<List<MedicalRecordVm>>(medicalRecords.FindAll(m => m.PatientID == patientId));
        }

        public async Task<MedicalRecordVm> GetMedicalRecordById(int id)
        {
            var medicalRecord = _medicalRecordRepository.GetMedicalRecordById(id);
            return _mapper.Map<MedicalRecordVm>(medicalRecord);
        }

        public async Task<bool> CreateMedicalRecord(MedicalRecord medicalRecord)
        {
            _medicalRecordRepository.AddMedicalRecord(medicalRecord);
            return true;
        }

        public async Task<bool> UpdateMedicalRecord(MedicalRecordVm medicalRecordVm)
        {
            var medicalRecord = _mapper.Map<MedicalRecord>(medicalRecordVm);
            _medicalRecordRepository.UpdateMedicalRecord(medicalRecord);
            return true;
        }

        public async Task<bool> DeleteMedicalRecord(int id)
        {
            _medicalRecordRepository.DeleteMedicalRecord(id);
            return true;
        }
    }
}
