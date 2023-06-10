using EquipmentAccounting.DataBase;
using EquipmentAccounting.Help;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for DestributionCreationWindow.xaml
    /// </summary>
    public partial class DestributionCreationWindow : Window
    {
        public Users User { get; set; }
        public ObservableCollection<EquipmentHelp> Equipments { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        public DateTime SelectedInvoiceDate { get; set; } = DateTime.Now;
        public UnitNumbers SelectedUnitNumber { get; set; }
        public ObservableCollection<UnitNumbers> UnitNumbers { get; set; }
        public int InvoiceNumber { get; set; }
        public string Description { get; set; }

        public DestributionCreationWindow()
        {
            InitializeComponent();
            Equipments = new ObservableCollection<EquipmentHelp>(Entities.Context.Equipments.Select(x => new EquipmentHelp { Equipment = x }));
            UnitNumbers = new ObservableCollection<UnitNumbers>(Entities.Context.UnitNumbers);
            DataContext = this;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedEquipmentsValid)
            {
                MessageBox.Show("Оборудования выбрано больше чем возможно");
                return;
            }
            if (SelectedUnitNumber == null)
            {
                MessageBox.Show("Выберите код подразделения");
                return;
            }
            foreach (var item in Equipments.Where(x => x.IsSelected))
            {
                item.Equipment.CountInStock -= item.SelectedCount;
                Entities.Context.Distributions.Add(new Distributions
                {
                    Date = SelectedDate,
                    Equipments = item.Equipment,
                    Description = Description,
                    Users = User,
                    EquipmentCount = item.SelectedCount,
                    UnitNumbers = SelectedUnitNumber,
                    InvoiceDate = SelectedInvoiceDate,
                    InvoiceNumber = InvoiceNumber
                });
            }
            Entities.Context.SaveChanges();
            DialogResult = true;
        }

        private bool IsSelectedEquipmentsValid => Equipments
            .Where(x => x.IsSelected)
            .All(x => x.Equipment.CountInStock >= x.SelectedCount);

        private void Cancel(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
