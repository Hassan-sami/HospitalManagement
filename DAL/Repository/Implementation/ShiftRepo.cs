using Hospital.DAL.DataBase;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Repository.Implementation
{
    public class ShiftRepo : IShiftRepo
    {
        private readonly HospitalDbContext hospitalDbContext;

        public ShiftRepo(HospitalDbContext hospitalDbContext)
        {
            this.hospitalDbContext = hospitalDbContext;
        }

        public async Task<bool> AddShift(Shift shift)
        {
            try
            {
                await hospitalDbContext.Shifts.AddAsync(shift);
                await hospitalDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public  IEnumerable<Shift> GetShifts()
        {
            return hospitalDbContext.Shifts.AsEnumerable();
        }
    }
}
