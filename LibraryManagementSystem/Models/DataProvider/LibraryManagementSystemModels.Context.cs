﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagementSystem.Models.DataProvider
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LMSEntities1 : DbContext
    {
        public LMSEntities1()
            : base("name=LMSEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<BBFORM> BBFORMs { get; set; }
        public virtual DbSet<BILL> BILLs { get; set; }
        public virtual DbSet<BOOK> BOOKs { get; set; }
        public virtual DbSet<BuildVersion> BuildVersions { get; set; }
        public virtual DbSet<CART> CARTs { get; set; }
        public virtual DbSet<CTHD> CTHDs { get; set; }
        public virtual DbSet<DETAIIMPORT> DETAIIMPORTs { get; set; }
        public virtual DbSet<DETAIL_BBFORM> DETAIL_BBFORM { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<IMPORT> IMPORTs { get; set; }
    }
}
