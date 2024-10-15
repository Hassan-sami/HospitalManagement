using Hospital.BLL.ModelVM;
using Hospital.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Abstraction
{
    public interface IAppointmentService
    {
        Task<List<AppointmentVm>> GetAppointmentsByPatientId(string patientId);
        Task<bool> CreateAppointment(Appointment appointment);
        Task<AppointmentVm> GetAppointmentById(int id);
        Task<bool> UpdateAppointment(AppointmentVm appointmentVm);
        Task<bool> DeleteAppointment(int id);
    }
}
