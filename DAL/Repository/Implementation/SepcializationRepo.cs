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
    public class SepcializationRepo : ISepcializationRepo
    {
        private readonly HospitalDbContext context;

        public SepcializationRepo(HospitalDbContext context) 
        {
            this.context = context;
        }
        public  async Task<bool> AddSepcialization(Specialization specialization)
        {
            try
            {
                await context.Set<Specialization>().AddAsync(specialization);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
            
        }

        public async Task<Specialization> GetSpecialization(int id)
        {
           return await context.Set<Specialization>().FindAsync(id);
        }

        public  IEnumerable<Specialization> GetSpecializations()
        {
            return  context.Set<Specialization>().AsEnumerable();
        }
    }
}
