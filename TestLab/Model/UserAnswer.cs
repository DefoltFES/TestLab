//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestLab.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAnswer
    {
        public int Id { get; set; }
        public Nullable<int> IdUserResults { get; set; }
        public Nullable<int> Qnumber { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Nullable<bool> IsRight { get; set; }
        public Nullable<bool> IsAnswered { get; set; }
    
        public virtual UserResult UserResult { get; set; }
    }
}
