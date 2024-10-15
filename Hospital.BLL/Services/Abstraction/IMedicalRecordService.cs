using Hospital.BLL.ModelVM;
using Hospital.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Abstraction
{
    public interface IMedicalRecordService
    {
        Task<List<MedicalRecordVm>> GetMedicalRecordsByPatientId(string patientId);
        Task<MedicalRecordVm> GetMedicalRecordById(int id);
        Task<bool> CreateMedicalRecord(MedicalRecord medicalRecord);
        Task<bool> UpdateMedicalRecord(MedicalRecordVm medicalRecordVm);
        Task<bool> DeleteMedicalRecord(int id);
    }
}
