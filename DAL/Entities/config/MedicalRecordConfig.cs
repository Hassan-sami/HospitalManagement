using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Entities.config
{
    internal class MedicalRecordConfig : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasOne<Patient>().WithMany(x => x.MedicalRecords).HasForeignKey(x => x.PatientID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne<Doctor>().WithMany(x => x.MedicalRecords).HasForeignKey(x => x.DoctorID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
