using Hospital.DAL.DataBase;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Hospital.DAL.Repository.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalDbContext _context;
        private bool disposed;

        public DoctorRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors.Include(d => d.Specialization).ToList();
        }

        public async Task<Doctor> GetDoctorById(string id)
        {
            return await _context.Doctors.FindAsync(id);
        }

        public async Task AddDoctor(Doctor Doctor)
        {
            _context.Doctors.Add(Doctor);
           await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctor(Doctor Doctor)
        {
            _context.Doctors.Update(Doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctor(string id)
        {
            var Doctor = await GetDoctorById(id);
            if (Doctor != null)
            {
                _context.Doctors.Remove(Doctor);
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }

        public async Task<Doctor> GetDoctorAndSchedulesById(string id)
        {
           return await _context.Doctors.Include(d => d.Schedules).FirstOrDefaultAsync(doc => doc.Id == id);

        }
    }
}
