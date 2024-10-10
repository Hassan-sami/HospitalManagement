using System.ComponentModel.DataAnnotations;

public class DoctorVm
{
    public string Id { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; }

    [Required]
    [Range(15000, 20000)]
    public decimal Salary { get; set; }

    [MaxLength(100, ErrorMessage = "Specialization cannot exceed 100 characters.")]
    public string? Specialization { get; set; }
}
