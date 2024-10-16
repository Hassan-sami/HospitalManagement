using Hospital.DAL.Entities.OwnedTypes;
using System.ComponentModel.DataAnnotations;

namespace Hospital.BLL.ModelVM
{
    public class CreateShiftVm
    {
        [Required]
        [Display(Name = "Shift type")]
        public ShiftType ShiftType { get; set; }
        [Required]
        [Display(Name = "Start time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [Required]
        [Display(Name = "End time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
    }
}
