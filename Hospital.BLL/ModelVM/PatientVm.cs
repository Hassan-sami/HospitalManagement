using System.ComponentModel.DataAnnotations;

public class PatientVm
{
    public string Id { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; }

    [MaxLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
    public string? Address { get; set; }

    [MaxLength(100, ErrorMessage = "Emergency contact cannot exceed 100 characters.")]
    public string? EmergencyContact { get; set; }
}
