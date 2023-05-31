using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for DeliveryCreationWindow.xaml
    /// </summary>
    public partial class DeliveryCreationWindow : Window
    {
        public ObservableCollection<Equipments> Equipments { get; set; }
        public ObservableCollection<Suppliers> Suppliers { get; set; }
        public DateTime SelectedDeliveryTime { get; set; } = DateTime.Now;
        public Equipments SelectedEquipment { get; set; }
        public Suppliers SelectedSupplier { get; set; }

        public DeliveryCreationWindow()
        {
            InitializeComponent();
            Equipments = new ObservableCollection<Equipments>
            {
                new Equipments { CountInStock = 1 }
            };
            Suppliers = new ObservableCollection<Suppliers>(Entities.Context.Suppliers);
            DataContext = this;
        }

        private void NewEquipment(object sender, RoutedEventArgs e)
        {
            Equipments.Add(new Equipments { CountInStock = 1 });
        }

        private void RemoveEquipment(object sender, RoutedEventArgs e)
        {
            if (SelectedEquipment != null)
                Equipments.Remove(SelectedEquipment);
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!IsEquipmentsValid || SelectedSupplier == null)
            {
                MessageBox.Show("Ошибка валидации");
                return;
            }
            var equips = Entities.Context.Equipments;
            Entities.Context.SaveChanges();
            foreach (var eq in Equipments)
            {
                var possibleDuplicate = equips.FirstOrDefault(x => x.Name.ToLower() == eq.Name.ToLower());
                if (possibleDuplicate != null)
                {
                    possibleDuplicate.CountInStock += eq.CountInStock;
                    possibleDuplicate.CountAll += eq.CountInStock;
                }
                else
                {
                    eq.CountAll = eq.CountInStock;
                }
                Entities.Context.Deliveries.Add(new Deliveries
                {
                    Date = SelectedDeliveryTime,
                    Equipments = possibleDuplicate ?? eq,
                    Count = eq.CountInStock,
                    Suppliers = SelectedSupplier
                });
            }
            Entities.Context.SaveChanges();
            DialogResult = true;
        }

        private bool IsEquipmentsValid => Equipments.All(x => x.CountInStock > 0
                && !string.IsNullOrEmpty(x.Type)
                && !string.IsNullOrEmpty(x.Name));


        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ShowSuppliersWindow(object sender, RoutedEventArgs e)
        {
            var result = new SupplierCreationWindow { Owner = this }.ShowDialog();
            if (result == true)
            {
                Suppliers.Clear();
                Suppliers.AddRange(Entities.Context.Suppliers);
            }
        }
    }
}
