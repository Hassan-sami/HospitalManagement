using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Entities
{
    [Table("Admins")]
    public class Admin : ApplicationUser
    {
        [Required]
        [Range(20000, 25000)]
        public decimal Salary { get; set; }
    }
}
