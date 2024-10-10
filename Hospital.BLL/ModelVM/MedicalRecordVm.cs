using System.ComponentModel.DataAnnotations;

public class MedicalRecordVm
{
    public int MedicalRecordID { get; set; }

    [MaxLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters.")]
    public string? Diagnosis { get; set; }

    [MaxLength(500, ErrorMessage = "Treatment cannot exceed 500 characters.")]
    public string? Treatment { get; set; }

    [Required]
    public DateTime RecordDate { get; set; }

    [Required]
    public string PatientId { get; set; }

    [Required]
    public string DoctorId { get; set; }
}
