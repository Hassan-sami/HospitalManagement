using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class PatientVm
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public List<AppointmentVm> Appointments { get; set; } = new List<AppointmentVm>();

    public List<MedicalRecordVm> MedicalRecords { get; set; } = new List<MedicalRecordVm>();
}
