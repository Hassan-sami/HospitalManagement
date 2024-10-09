using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Hospital.DAL.Entities
{
    
    public class Patient : ApplicationUser
    {


        [MaxLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string? Address { get; set; }

        [MaxLength(100, ErrorMessage = "Emergency contact cannot exceed 100 characters.")]
        public string? EmergencyContact { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<MedicalRecord>? MedicalRecords { get; set; }
    }
}
