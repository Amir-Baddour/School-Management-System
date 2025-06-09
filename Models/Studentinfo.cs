using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Testing1.Models
{
    public class Studentinfo
    {
        public int Id { get; set; }
        public DateTime DateofBirth { get; set; }
        public string email { get; set; }
        public int phone_num { get; set; }
        public string Class { get; set; }
        public string emergency_contract { get; set; }

    }
}