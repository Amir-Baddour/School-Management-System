using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Testing1.Models
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; }

        public virtual Student Student { get; set; }
    }
}