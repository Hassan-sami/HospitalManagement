using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Abstraction
{
    public interface IShiftService
    {
        public IEnumerable<Shift> GetShifts();
        Task<bool> AddShift(Shift shift);
    }
}
