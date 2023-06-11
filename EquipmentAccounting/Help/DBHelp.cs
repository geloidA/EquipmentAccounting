using EquipmentAccounting.DataBase;

namespace EquipmentAccounting.Help
{
    internal static class DBHelp
    {
        /// <summary>
        /// Remove equip and change deliveries and destributions links on it by substitute.
        /// Doesn't save db changes
        /// </summary>
        /// <param name="toRemove">Equip to remove</param>
        /// <param name="insteadEquip">Substitute</param>
        public static void RemoveEquipment(Equipments toRemove, Equipments insteadEquip)
        {
            foreach (var delivery in toRemove.Deliveries)
                delivery.Equipments = insteadEquip;
            foreach (var distribution in toRemove.Distributions)
                distribution.Equipments = insteadEquip;
            Entities.Context.Equipments.Remove(toRemove);
        }
    }
}
