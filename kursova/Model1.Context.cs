﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kursova
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class firmaEntities : DbContext
    {
        public firmaEntities()
            : base("name=firmaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<clients_servises> clients_servises { get; set; }
        public virtual DbSet<Consultations> Consultations { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<kinds_of_services> kinds_of_services { get; set; }
        public virtual DbSet<records_on_consultations> records_on_consultations { get; set; }
        public virtual DbSet<requests_documents> requests_documents { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<working_staff> working_staff { get; set; }
        public virtual DbSet<documents_data> documents_data { get; set; }
    }
}