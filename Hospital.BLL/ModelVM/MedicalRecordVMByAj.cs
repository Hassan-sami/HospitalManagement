using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.ModelVM
{
    public class MedicalRecordVMByAj
    {
        public  string? Diagnosis {  get; set; }

        public string? Treatment { get; set; }

        public DateTime RecordDate { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone {  get; set; }
    }
}
