using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Abstraction
{
    public interface IDoctorService
    {
        List<Doctor> GetAllDoctors();
        List<DoctorVm> GetDoctorVms();
        public Task<Doctor> DoctorByIdAsync(string id);
    }
}
