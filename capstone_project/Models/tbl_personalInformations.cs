//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace capstone_project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_personalInformations
    {
        public int personal_id { get; set; }
        public string personal_firstName { get; set; }
        public string personal_lastName { get; set; }
        public Nullable<int> account_id { get; set; }
    
        public virtual tbl_accounts tbl_accounts { get; set; }
    }
}
