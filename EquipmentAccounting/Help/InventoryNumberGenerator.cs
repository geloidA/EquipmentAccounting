using EquipmentAccounting.DataBase;
using System;
using System.Linq;

namespace EquipmentAccounting.Help
{
    public class InventoryNumberGenerator
    {
        private static readonly Random random = new Random();

        public static int GenerateInventoryNumber()
        {
            // Генерируем случайное число от 1 до 10000
            int inventoryNumber = random.Next(1, 10001);

            // Проверяем, что сгенерированный номер не дублируется
            while (IsDuplicateInventoryNumber(inventoryNumber))
            {
                inventoryNumber = random.Next(1, 10001);
            }

            return inventoryNumber;
        }

        private static bool IsDuplicateInventoryNumber(int number)
        {
            return Entities.Context.EquipmentBuild
                .ToList()
                .Any(x => x.InventoryNumber == number);
        }
    }
}