using Hospital.DAL.Entities.OwnedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class DoctorAppointmentApprovalVm
    {
        public int? Id { get; set; }
        public AppointStatus Status { get; set; }
    }
}
