using Hospital.BLL.Services.Abstraction;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Implementation
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepo scheduleRepo;

        public ScheduleService(IScheduleRepo scheduleRepo)
        {
            this.scheduleRepo = scheduleRepo;
        }
        public async Task<bool> AddSchedule(Schedule schedule)
        {
            return await scheduleRepo.AddSchedule(schedule);
        }
    }
}
