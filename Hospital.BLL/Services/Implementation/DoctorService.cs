using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository) 
        {
            this.doctorRepository = doctorRepository;
        }
        public List<Doctor> GetAllDoctors()
        {
           return doctorRepository.GetAllDoctors();
        }

        public List<DoctorVm> GetDoctorVms()
        {
            return doctorRepository.GetAllDoctors().Select(d =>
            
                new DoctorVm { Id = d.Id,FirstName = d.FirstName, LastName = d.LastName, Salary = d.Salary, Image = d.Image ,Specialization = d.Specialization?.Name ?? "no sepcialization" }
            ).ToList();
        }

        public async Task<Doctor> DoctorByIdAsync(string id)
        {
            return await doctorRepository.GetDoctorById(id);
        }

        public async Task<Doctor> GetDoctorAndSchedulesById(string id)
        {
           return await  doctorRepository.GetDoctorAndSchedulesById(id);
        }
    }
}
