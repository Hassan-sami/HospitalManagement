using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class AppointVmByAj
    {
        public string FullName { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public string AppointId { get; set; }
        public int Id { get; set; }

        public string Status { get; set; }
    }
}
