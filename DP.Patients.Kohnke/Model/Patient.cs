using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DP.Patients.KK.Model
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime PositiveTestDate { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}
