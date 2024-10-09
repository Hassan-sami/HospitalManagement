using Hospital.DAL.Entities;
using System.Collections.Generic;

namespace Hospital.DAL.Repository.Abstraction
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAllAppointments();
        Task<Appointment> GetAppointmentById(int id);
        Task AddAppointment(Appointment appointment);
        Task UpdateAppointment(Appointment appointment);
        Task DeleteAppointment(int id);
        void Dispose();
    }
}
