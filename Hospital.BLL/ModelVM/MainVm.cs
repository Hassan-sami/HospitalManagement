using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class MainVm
    {
        public IEnumerable<Doctor>? Doctors { get; set; }
    }
}
