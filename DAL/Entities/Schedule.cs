using Hospital.DAL.Entities.OwnedTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Entities
{
    [Table(name: "Schedules")]
    public class Schedule
    {
        public int Id { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId {  get; set; }
        [ForeignKey("Shift")]
        public int ShiftId { get; set; }

        public Doctor Doctor { get; set; }

        public Shift Shift { get; set; }

        public DateTime Date { get; set; }

        public Status Status { get; set; }
    }
}
