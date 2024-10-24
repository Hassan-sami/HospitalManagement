﻿using Hospital.DAL.DataBase;
using Hospital.DAL.Entities;
using Hospital.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public IEnumerable<MedicalRecord> GetAllMedicalRecords()
        {
            return _context.MedicalRecords.Include(m => m.Patient).Include(m => m.Doctor)?.ThenInclude(d => d.Specialization);
        }

        public MedicalRecord GetMedicalRecordById(int id)
        {
            return _context.MedicalRecords.Include(m => m.Patient).Include(m => m.Doctor)
                                           .FirstOrDefault(m => m.MedicalRecordID == id);
        }

        public async Task<bool> AddMedicalRecord(MedicalRecord medicalRecord)
        {
            try
            {
                await _context.MedicalRecords.AddAsync(medicalRecord);
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
            
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

        public IEnumerable<MedicalRecord> GetDoctorMedicalRecords(Expression<Func<MedicalRecord, bool>> predicate)
        {
            return _context.MedicalRecords.Include(m => m.Patient).Where(predicate);
        }
    }
}
