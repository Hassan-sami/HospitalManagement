using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Repository.Abstraction
{
    public interface IShiftRepo 
    {
        public IEnumerable<Shift> GetShifts();
        Task<bool> AddShift(Shift shift);
    }
}
