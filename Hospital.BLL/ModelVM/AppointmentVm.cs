using System.ComponentModel.DataAnnotations;

public class AppointmentVm
{
    public int AppointmentID { get; set; }

    [Required(ErrorMessage = "Appointment date is required.")]
    public DateTime AppointmentDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string Status { get; set; }

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Patient is required.")]
    public string PatientId { get; set; }

    [Required(ErrorMessage = "Doctor is required.") ]       
    public string DoctorId { get; set; }
}
