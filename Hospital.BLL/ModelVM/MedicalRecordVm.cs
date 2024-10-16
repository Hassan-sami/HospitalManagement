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

    public string PatientName {  get; set; }


    public string DoctorName { get; set; }

    
}
