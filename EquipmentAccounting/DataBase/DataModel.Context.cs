﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EquipmentAccounting.DataBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public static Entities Context { get; } = new Entities();

        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Deliveries> Deliveries { get; set; }
        public virtual DbSet<Distributions> Distributions { get; set; }
        public virtual DbSet<EquipmentBuild> EquipmentBuild { get; set; }
        public virtual DbSet<Equipments> Equipments { get; set; }
        public virtual DbSet<EquipmentTypes> EquipmentTypes { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UnitNumbers> UnitNumbers { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
