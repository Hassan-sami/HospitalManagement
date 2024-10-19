using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Implementation
{
    public class MedicalRecordService : ImedicalRecordService
    {
        private readonly IMedicalRecordRepository medicalRecordRepository;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
        {
            this.medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<bool> AddMedicalRecord(MedicalRecord record)
        {
            
             return await medicalRecordRepository.AddMedicalRecord(record);
            
            
        }

        public IEnumerable<MedicalRecord> GetDoctorMedicalRecords(Expression<Func<MedicalRecord, bool>> predicate)
        {
            return medicalRecordRepository.GetDoctorMedicalRecords(predicate);
        }

        public IEnumerable<MedicalRecord> GetMedicalRecordsWithPatientAndDoctor()
        {
            return medicalRecordRepository.GetAllMedicalRecords();   
        }
    }
}
