
using Hospital.DAL.Entities.OwnedTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Entities
{
    [Table(name:"Shifts")]
    public class Shift
    {
        public int ShiftId { get; set; }

        public ShiftType ShiftType { get; set; }

        public TimeSpan StartTIme { get; set; }

        public TimeSpan EndTIme { get; set; }

    }
}
