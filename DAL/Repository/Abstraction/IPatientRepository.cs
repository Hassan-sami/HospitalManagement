using Hospital.DAL.Entities;
using System.Collections.Generic;

namespace Hospital.DAL.Repository.Abstraction
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Task<Patient> GetPatientById(string id);
        Task AddPatient(Patient patient);
        Task UpdatePatient(Patient patient);
        Task DeletePatient(string id);

        void Dispose();
    }
}
