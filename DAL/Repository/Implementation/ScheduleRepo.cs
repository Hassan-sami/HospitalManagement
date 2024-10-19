using Hospital.DAL.DataBase;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Repository.Implementation
{
    public class ScheduleRepo : IScheduleRepo
    {
        private readonly HospitalDbContext hospitalDbContext;

        public ScheduleRepo(HospitalDbContext hospitalDbContext)
        {
            this.hospitalDbContext = hospitalDbContext;
        }
        public async Task<bool> AddSchedule(Schedule schedule)
        {
            try
            {
                await hospitalDbContext.Schedules.AddAsync(schedule);
                await hospitalDbContext.SaveChangesAsync();  
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Schedule> GetDoctorSchedulesById(string docId)
        {
            return  hospitalDbContext.Schedules.Where(sch => sch.DoctorId == docId).Include(s => s.Shift);
        }
    }
}
