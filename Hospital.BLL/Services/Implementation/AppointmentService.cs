using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Hospital.DAL.Entities.OwnedTypes;
using Hospital.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }
        public async Task<bool> AddAppointment(Appointment appointment)
        {
            try
            {
                await appointmentRepository.AddAppointment(appointment);
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAppointment(int id)
        {
            try
            {
                await appointmentRepository.DeleteAppointment(id);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            return await appointmentRepository.GetAppointmentById(id);
        }

        public IEnumerable<Appointment> GetAppointments(Expression<Func<Appointment, bool>> predicate)
        {
            return appointmentRepository.GetAppointments(predicate);
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            try
            {
               await appointmentRepository.UpdateAppointment(appointment);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateAppointmentStatus(AppointStatus Status)
        {
             appointmentRepository.UpdateAppointmentStatus(p => p.AppointmentDate < DateTime.Now, Status);
        }
    }
}
