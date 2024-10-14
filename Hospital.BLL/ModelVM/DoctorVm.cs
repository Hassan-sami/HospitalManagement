using Hospital.DAL.Entities;
using System.ComponentModel.DataAnnotations;

public class DoctorVm
{
    
    public string Id {  get; set; }
    
    public string FirstName { get; set; }

    
    public string LastName { get; set; }

    
    public decimal Salary { get; set; }

    public string? Image {  get; set; }

    [Display(Name = "Specialization")]
    public int SpecializationId { get; set; }

    public string? Specialization {  get; set; }


    public IEnumerable<Specialization>? specializations { get; set; }
}
