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
    
    public partial class EquipmentBuild
    {
        public int InventoryNumber { get; set; }
        public int LocationID { get; set; }
        public int EquipmentID { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Equipments Equipments { get; set; }
        public virtual Locations Locations { get; set; }
    }
}
