//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Testing1
{
    using System;
    using System.Collections.Generic;
    
    public partial class PasswordResetCodes
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Code { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
