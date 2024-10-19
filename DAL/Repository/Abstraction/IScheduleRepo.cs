using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Repository.Abstraction
{
    public interface IScheduleRepo
    {
        Task<bool> AddSchedule(Schedule schedule);
        IEnumerable<Schedule> GetDoctorSchedulesById(string docId);
    }
}
