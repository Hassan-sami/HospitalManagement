using Hospital.DAL.DataBase;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Hospital.DAL.Repository.Implementation
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HospitalDbContext _context;
        private bool disposed;
        public MedicalRecordRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public List<MedicalRecord> GetAllMedicalRecords()
        {
            return _context.MedicalRecords.Include(m => m.Patient).Include(m => m.Doctor).ToList();
        }

        public MedicalRecord GetMedicalRecordById(int id)
        {
            return _context.MedicalRecords.Include(m => m.Patient).Include(m => m.Doctor)
                                           .FirstOrDefault(m => m.MedicalRecordID == id);
        }

        public void AddMedicalRecord(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            _context.SaveChanges();
        }

        public void UpdateMedicalRecord(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Update(medicalRecord);
            _context.SaveChanges();
        }

        public void DeleteMedicalRecord(int id)
        {
            var medicalRecord = GetMedicalRecordById(id);
            if (medicalRecord != null)
            {
                _context.MedicalRecords.Remove(medicalRecord);
                _context.SaveChanges();
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
    }
}
