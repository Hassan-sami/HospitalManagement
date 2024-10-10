using AutoMapper;
using Hospital.DAL.Entities;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
       
        CreateMap<Doctor, DoctorVm>();
        CreateMap<Admin, AdminVm>();
        CreateMap<Patient, PatientVm>();
        CreateMap<MedicalRecord, MedicalRecordVm>();
        CreateMap<Appointment, AppointmentVm>();


        CreateMap<DoctorVm, Doctor>();
        CreateMap<AdminVm, Admin>();
        CreateMap<PatientVm, Patient>();
        CreateMap<MedicalRecordVm, MedicalRecord>();
        CreateMap<AppointmentVm, Appointment>(); 
    }
}
