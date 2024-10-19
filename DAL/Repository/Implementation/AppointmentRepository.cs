using Hospital.DAL.DataBase;
using Hospital.DAL.Entities;
using Hospital.DAL.Entities.OwnedTypes;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hospital.DAL.Repository.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalDbContext _context;

        private bool disposed;
        public AppointmentRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments.Include(a => a.Patient).Include(a => a.Doctor).ToList();
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            return await _context.Appointments.Include(a => a.Patient).Include(a => a.Doctor).Include(a => a.Schedule).SingleOrDefaultAsync(ap => ap.AppointmentID == id);
        }

        public async Task AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointment(int id)
        {
            var appointment = await GetAppointmentById(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
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

        public IEnumerable<Appointment> GetAppointments(Expression<Func<Appointment, bool>> predicate)
        {
             return _context.Appointments.Include(a => a.Doctor).Include(ap => ap.Patient).Include(app => app.Schedule)?.
                ThenInclude(sch => sch.Shift).Where(predicate);
        }

        public void UpdateAppointmentStatus(Expression<Func<Appointment, bool>> predicate, AppointStatus Status)
        {
            
                 _context.Appointments.Where(predicate).ExecuteUpdate(c => c.SetProperty(a => a.Status, app => Status));
                
            
        }
    }
}
