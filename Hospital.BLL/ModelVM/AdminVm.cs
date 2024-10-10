using System.ComponentModel.DataAnnotations;

public class AdminVm
{
    public string Id { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; }

    [Required]
    [Range(20000, 25000)]
    public decimal Salary { get; set; }
}
