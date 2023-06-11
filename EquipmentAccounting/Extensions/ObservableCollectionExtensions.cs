using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EquipmentAccounting.Extensions
{
    internal static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> values)
        {
            foreach (var value in values)
                collection.Add(value);
        }

        public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> values)
        {
            foreach (var value in values)
                collection.Remove(value);
        }
    }
}
