using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Repository.Abstraction
{
    public interface ISepcializationRepo
    {
        public IEnumerable<Specialization> GetSpecializations();
        public Task<Specialization> GetSpecialization(int id);
        public Task<bool> AddSepcialization(Specialization specialization);

        
    }
}
