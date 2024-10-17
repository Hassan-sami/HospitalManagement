using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Abstraction
{
    public interface IAppointmentService
    {
        Task<bool> AddAppointment(Appointment appointment);
        IEnumerable<Appointment> GetAppointments(Expression<Func<Appointment, bool>> predicate);

        Task<Appointment> GetAppointmentById(int id);

        Task<bool> DeleteAppointment(int id);
        Task<bool> UpdateAppointment(Appointment appointment);

    }
}
