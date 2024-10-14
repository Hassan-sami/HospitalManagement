using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Services.Abstraction
{
    public interface ISepcializationService
    {
        public IEnumerable<Specialization> GetSpecializations();

        public Task<bool> AddSepcialization(Specialization specialization);
        public Task<Specialization> GetSpecialization(int id);
    }
}
