using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
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
        private Locations previousLocationTo;
        private Locations previousLocationFrom;

        public Users User { get; set; }
        public ObservableCollection<EquipmentHelp> Equipments { get; set; } = new ObservableCollection<EquipmentHelp>();
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        public DateTime SelectedInvoiceDate { get; set; } = DateTime.Now;
        public Locations SelectedLocationTo { get; set; }
        public Locations SelectedLocationFrom { get; set; }
        public ObservableCollection<Locations> Locations { get; set; }
        public int InvoiceNumber { get; set; }
        public string Description { get; set; }

        public DestributionCreationWindow()
        {
            InitializeComponent();
            Locations = new ObservableCollection<Locations>(Entities.Context.Locations);
            DataContext = this;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedEquipmentsValid)
            {
                MessageBox.Show("Оборудования выбрано больше чем возможно");
                return;
            }
            if (SelectedLocationTo == null)
            {
                MessageBox.Show("Выберите место распределения");
                return;
            }
            var lastID = Entities.Context.Distributions
                .ToList()
                .LastOrDefault()?.ID ?? 0;
            lastID++;
            foreach (var item in Equipments.Where(x => x.IsSelected))
            {
                item.Equipment.Count -= item.SelectedCount;
                var possibleDublicate = SelectedLocationTo.Equipments.FirstOrDefault(x => x.Name == item.Equipment.Name);
                if (possibleDublicate != null)
                {
                    possibleDublicate.Count += item.SelectedCount;
                }
                else
                {
                    var copy = item.Equipment.Copy();
                    copy.Count = item.SelectedCount;
                    copy.Locations = SelectedLocationTo;
                    Entities.Context.Equipments.Add(copy);
                }
                Entities.Context.Distributions.Add(new Distributions
                {
                    ID = lastID,
                    Date = SelectedDate,
                    Equipments = item.Equipment,
                    Description = Description,
                    Users = User,
                    EquipmentCount = item.SelectedCount,
                    Locations = SelectedLocationTo,
                    InvoiceDate = SelectedInvoiceDate,
                    InvoiceNumber = InvoiceNumber
                });
            }
            Entities.Context.SaveChanges();
            DialogResult = true;
        }

        private bool IsSelectedEquipmentsValid => Equipments
            .Where(x => x.IsSelected)
            .All(x => x.Equipment.Count >= x.SelectedCount);

        private void Cancel(object sender, RoutedEventArgs e) => DialogResult = false;

        private void cmbLocationsFrom_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SelectedLocationFrom == SelectedLocationTo)
            {
                MessageBox.Show("Места не могут быть одинаковыми");
                e.Handled = false;
                cmbLocationFrom.SelectedItem = previousLocationFrom;
                return;
            }
            Equipments.Clear();
            Equipments.AddRange(Entities.Context.Equipments
                .ToList()
                .Where(x => x.Locations == SelectedLocationFrom)
                .Select(x => new EquipmentHelp { Equipment = x }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedLocationFrom = Locations.First(x => x.Name == "Склад");
            cmbLocationFrom.SelectedItem = SelectedLocationFrom;
        }

        private void cmbLocationTo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SelectedLocationFrom == SelectedLocationTo)
            {
                MessageBox.Show("Места не могут быть одинаковыми");
                e.Handled = false;
                cmbLocationTo.SelectedItem = previousLocationTo;
                return;
            }
        }

        private void cmbLocationTo_DropDownOpened(object sender, EventArgs e)
        {
            previousLocationTo = SelectedLocationTo;
        }

        private void cmbLocationFrom_DropDownOpened(object sender, EventArgs e)
        {
            previousLocationFrom = SelectedLocationFrom;
        }
    }
}
