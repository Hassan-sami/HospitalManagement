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
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepo shiftRepo;

        public ShiftService(IShiftRepo shiftRepo)
        {
            this.shiftRepo = shiftRepo;
        }

        public async Task<bool> AddShift(Shift shift)
        {
            return await shiftRepo.AddShift(shift);
        }

        public IEnumerable<Shift> GetShifts()
        {
            return shiftRepo.GetShifts();
        }
    }
}
