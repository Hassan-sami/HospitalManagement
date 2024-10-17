using Hospital.DAL.Entities;
using System.ComponentModel.DataAnnotations;

public class AppointmentVm
{
    
    public int? Id { get; set; }

    [Required(ErrorMessage = "Appointment date is required.")]
    [Display(Name = "Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime AppointmentDate { get; set; }

    

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }

    //[Required(ErrorMessage = "Patient is required.")]
    //[Display(Name = "Patient")]
    //public string PatientId { get; set; }

    [Required(ErrorMessage = "Doctor is required.")]
    [Display(Name = "Doctor")]
    public string DoctorId { get; set; }

    public List<Doctor>? Doctors { get; set; }

    //public List<Patient>? Patients { get; set; }
}
