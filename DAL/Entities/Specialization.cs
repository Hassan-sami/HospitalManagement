using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL.Entities
{
    public class Specialization
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Specialization cannot exceed 100 characters.")]
        public string Name { get; set; }
    }
}
