using Hospital.DAL.Entities;
using System.Collections.Generic;

namespace Hospital.DAL.Repository.Abstraction
{
    public interface IMedicalRecordRepository
    {
        List<MedicalRecord> GetAllMedicalRecords();
        MedicalRecord GetMedicalRecordById(int id);
        void AddMedicalRecord(MedicalRecord medicalRecord);
        void UpdateMedicalRecord(MedicalRecord medicalRecord);
        void DeleteMedicalRecord(int id);
        void Dispose();
    }
}
