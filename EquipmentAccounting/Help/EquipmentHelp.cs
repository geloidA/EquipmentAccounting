using EquipmentAccounting.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentAccounting.Help
{
    public class EquipmentHelp
    {
        public Equipments Equipment { get; set; }
        public int SelectedCount { get; set; } = 1;
        public bool IsSelected { get; set; }
    }
}
