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
    public class SepcializationService : ISepcializationService
    {
        private readonly ISepcializationRepo sepcializationRepo;

        public SepcializationService(ISepcializationRepo sepcializationRepo)
        {
            this.sepcializationRepo = sepcializationRepo;
        }
        public async Task<bool> AddSepcialization(Specialization specialization)
        {
           return await sepcializationRepo.AddSepcialization(specialization);
        }

        public IEnumerable<Specialization> GetSpecializations()
        {
            return sepcializationRepo.GetSpecializations();
        }
        public async Task<Specialization> GetSpecialization(int id)
        {
            return await sepcializationRepo.GetSpecialization(id);
        }
    }
}
