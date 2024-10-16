using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class AddmedicalRecordVM
    {
        public string Treatment { get; set; }
        public string Diagnosis { get; set;}

        [Display(Name = "Date")]
        public DateTime RecordDate { get; set; }

        [Display(Name ="Patient")]
        public int PatientId { get; set; }

        [Display(Name = "Doctor")]
        public int DoctorId {  get; set; }

        public IEnumerable<Patient>? patients { get; set; }

        public IEnumerable<Doctor>? Doctors { get; set; }

    }
}
