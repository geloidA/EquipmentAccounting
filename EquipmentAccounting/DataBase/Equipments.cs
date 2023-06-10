//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Equipments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipments()
        {
            this.Deliveries = new HashSet<Deliveries>();
            this.Distributions = new HashSet<Distributions>();
            this.EquipmentBuild = new HashSet<EquipmentBuild>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int EquipmentTypeID { get; set; }
        public int Count { get; set; }
        public int LocationID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deliveries> Deliveries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Distributions> Distributions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EquipmentBuild> EquipmentBuild { get; set; }
        public virtual EquipmentTypes EquipmentTypes { get; set; }
        public virtual Locations Locations { get; set; }

        internal Equipments Copy()
        {
            return new Equipments
            {
                Name = Name,
                EquipmentTypes = EquipmentTypes,
                Count = Count,
                Locations = Locations
            };
        }
    }
}
