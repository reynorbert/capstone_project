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
    
    public partial class tbl_companies
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_companies()
        {
            this.tbl_accounts = new HashSet<tbl_accounts>();
        }
    
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string company_address { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_accounts> tbl_accounts { get; set; }
    }
}
