using Hospital.DAL.Entities;
using Hospital.DAL.Entities.OwnedTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class CreateScheduleVM
    {
        [Required]
        [Display(Name = "Doctor")]
        public string DoctorId { get; set; }
        [Required]
        [Display(Name = "Shift")]
        public int ShiftId { get; set; }

        [Required]
        
        public DayOfWeek Day { get; set; }

        [Required]
        public Status Status { get; set; }

        public List<Doctor>? Doctors { get; set; }

        public List<Shift>? Shifts {  get; set; }
    }
}
