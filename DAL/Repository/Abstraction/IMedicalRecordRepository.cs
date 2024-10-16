using Hospital.DAL.Entities;
using System.Collections.Generic;

namespace Hospital.DAL.Repository.Abstraction
{
    public interface IMedicalRecordRepository
    {
        IEnumerable<MedicalRecord> GetAllMedicalRecords();
        MedicalRecord GetMedicalRecordById(int id);
        Task<bool> AddMedicalRecord(MedicalRecord medicalRecord);
        void UpdateMedicalRecord(MedicalRecord medicalRecord);
        void DeleteMedicalRecord(int id);
        void Dispose();
    }
}
