﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class capstone_mwdEntities : DbContext
    {
        public capstone_mwdEntities()
            : base("name=capstone_mwdEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_accounts> tbl_accounts { get; set; }
        public virtual DbSet<tbl_companies> tbl_companies { get; set; }
        public virtual DbSet<tbl_inquiries> tbl_inquiries { get; set; }
        public virtual DbSet<tbl_personalInformations> tbl_personalInformations { get; set; }
        public virtual DbSet<tbl_products> tbl_products { get; set; }
        public virtual DbSet<tbl_requirements> tbl_requirements { get; set; }
        public virtual DbSet<tbl_threads> tbl_threads { get; set; }
        public virtual DbSet<tbl_transactions> tbl_transactions { get; set; }
    }
}
