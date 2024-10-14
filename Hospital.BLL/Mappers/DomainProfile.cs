using AutoMapper;
using Hospital.BLL.ModelVM;
using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Mappers
{
    public class DomainProfile : Profile
    {
        public DomainProfile() 
        {
            CreateMap<RegisterViewModel, Patient>();
            CreateMap<CreateDoctorViewModel, Doctor>();
            CreateMap<Doctor, DoctorVm>();
            CreateMap<Admin, AdminVm>();
            CreateMap<Patient, PatientVm>();
            CreateMap<MedicalRecord, MedicalRecordVm>();
            CreateMap<Appointment, AppointmentVm>();

            CreateMap<Doctor, Doctor>();


            
            CreateMap<Patient, EditPatientVm>();
            CreateMap<DoctorVm, Doctor>();
            CreateMap<AdminVm, Admin>();
            CreateMap<PatientVm, Patient>();
            CreateMap<MedicalRecordVm, MedicalRecord>();
            CreateMap<AppointmentVm, Appointment>();
        }
    }
}
