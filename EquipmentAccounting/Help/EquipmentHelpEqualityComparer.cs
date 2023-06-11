using System.Collections.Generic;

namespace EquipmentAccounting.Help
{
    internal class EquipmentHelpEqualityComparer : IEqualityComparer<EquipmentHelp>
    {
        private static readonly EquipmentsEqualityComparer equipmentsEquality = new EquipmentsEqualityComparer();

        public bool Equals(EquipmentHelp x, EquipmentHelp y)
        {
            return equipmentsEquality.Equals(x.Equipment, y.Equipment);
        }

        public int GetHashCode(EquipmentHelp obj)
        {
            return obj.GetHashCode();
        }
    }
}
