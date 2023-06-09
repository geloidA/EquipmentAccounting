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
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    
    public partial class Equipments : INotifyPropertyChanged
    {
        private int count;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipments()
        {
            this.Deliveries = new HashSet<Deliveries>();
            this.Distributions = new HashSet<Distributions>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int EquipmentTypeID { get; set; }
        public int Count 
        { 
            get => count; 
            set
            {
                count = value;
                OnPropertyChanged();
            }
        }
        public int LocationID { get; set; }
        public Nullable<int> InventoryNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deliveries> Deliveries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Distributions> Distributions { get; set; }
        public virtual EquipmentBuild EquipmentBuild { get; set; }
        public virtual EquipmentTypes EquipmentTypes { get; set; }
        public virtual Locations Locations { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
