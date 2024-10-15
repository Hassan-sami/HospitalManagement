using AutoMapper;
using Hospital.BLL.ModelVM;
using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<List<AppointmentVm>> GetAppointmentsByPatientId(string patientId)
        {
            var appointments = _appointmentRepository.GetAllAppointments();
            return _mapper.Map<List<AppointmentVm>>(appointments.FindAll(a => a.PatientID == patientId));
        }

        public async Task<bool> CreateAppointment(Appointment appointment)
        {
            await _appointmentRepository.AddAppointment(appointment);
            return true;
        }

        public async Task<AppointmentVm> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            return _mapper.Map<AppointmentVm>(appointment);
        }

        public async Task<bool> UpdateAppointment(AppointmentVm appointmentVm)
        {
            var appointment = _mapper.Map<Appointment>(appointmentVm);
            await _appointmentRepository.UpdateAppointment(appointment);
            return true;
        }

        public async Task<bool> DeleteAppointment(int id)
        {
            await _appointmentRepository.DeleteAppointment(id);
            return true;
        }
    }
}
