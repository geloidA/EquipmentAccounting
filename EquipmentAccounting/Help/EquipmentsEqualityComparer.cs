using EquipmentAccounting.DataBase;
using System.Collections.Generic;

namespace EquipmentAccounting.Help
{
    internal class EquipmentsEqualityComparer : IEqualityComparer<Equipments>
    {
        public bool Equals(Equipments x, Equipments y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Equipments obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
