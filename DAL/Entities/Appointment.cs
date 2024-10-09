using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.DAL.Entities
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        
        [ForeignKey("Patient")]
        public string? PatientID { get; set; }

        public Patient? Patient { get; set; }

        
        [ForeignKey("Doctor")]
        public string? DoctorID { get; set; }

        public Doctor? Doctor { get; set; }
    }
}
