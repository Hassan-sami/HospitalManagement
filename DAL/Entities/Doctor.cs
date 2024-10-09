using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Entities
{
    public class Doctor : ApplicationUser
    {
        [Required]
        [Range(15000,20000)]
        public decimal Salary { get; set; }

        [MaxLength(100, ErrorMessage = "Specialization cannot exceed 100 characters.")]
        public string? Specialization { get; set; }

        public List<Appointment>? Appointments { get; set; }
        public List<MedicalRecord>? MedicalRecords { get; set; }
    }
}
