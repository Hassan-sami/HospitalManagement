using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class AdminIndexVm
    {
        public int NoOfDoctors { get; set; }

        public int NoOfPatients { get; set; }
        public int NoOfAppoint{ get; set; }
        public int NoOfMedical { get; set; }
        public int NoOfNewAppoint { get; set; }
        public int NoOfTodayPatient { get; set; }
    }
}
