using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using EquipmentAccounting.Help;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for AddEditEquipmentBuildsWindow.xaml
    /// </summary>
    public partial class AddEditEquipmentBuildsWindow : Window
    {
        public ObservableCollection<EquipmentHelp> StockEquipments { get; set; }
        public ObservableCollection<Equipments> BuildEquipments { get; set; }
        public Equipments SelectedBuildEquipment { get; set; }
        public ObservableCollection<Locations> Locations { get; set; }
        public Locations SelectedLocation { get; set; }

        private readonly Dictionary<string, int> countChangesByEquipName = new Dictionary<string, int>();
        private readonly EquipmentBuild toEditBuild;

        public AddEditEquipmentBuildsWindow(EquipmentBuild toEditBuild = null)
        {
            this.toEditBuild = toEditBuild;
            InitializeComponent();
            DataContext = this;
            StockEquipments = new ObservableCollection<EquipmentHelp>(Entities.Context.Equipments
                .ToList()
                .Where(x => x.Locations.Name == "Склад")
                .Select(x => new EquipmentHelp { Equipment = x.Copy() }));
            Locations = new ObservableCollection<Locations>(Entities.Context.Locations
                .ToList()
                .Where(x => x.Name != "Склад"));
            cmbLocs.ItemsSource = Locations;
            SelectedLocation = Locations.FirstOrDefault();
            Title = toEditBuild is null ? "Создание сборки" : "Редактирование сборки";
            BuildEquipments = toEditBuild is null
                ? new ObservableCollection<Equipments>()
                : new ObservableCollection<Equipments>(toEditBuild.Equipments);
            Loaded += (s, e) => cmbLocs.SelectedItem = SelectedLocation;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (toEditBuild is null) 
                AddBuild();
            else 
                EditBuild();
            DialogResult = true;
        }

        private void AddBuild()
        {
            var inventoryNum = InventoryNumberGenerator.GenerateInventoryNumber();
            var build = new EquipmentBuild
            {
                Locations = SelectedLocation,
                Date = DateTime.Now,
                InventoryNumber = inventoryNum,
                Equipments = BuildEquipments
            };
            Entities.Context.EquipmentBuild.Add(build);
            Entities.Context.SaveChanges();
        }

        private void EditBuild()
        {

        }

        private void btnToBuild_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSelectedEquipmentsValid)
            {
                MessageBox.Show("Оборудования выбрано больше чем возможно");
                return;
            }
            foreach (var (Build, Select) in EqualEquipments)
                Build.Count += Select.Count;
            foreach (var equip in SelectedEquipments)
            {
                if (!countChangesByEquipName.ContainsKey(equip.Name))
                    countChangesByEquipName[equip.Name] = 0;
                countChangesByEquipName[equip.Name] += equip.Count;
            }
            BuildEquipments.AddRange(SelectedEquipments.Except(BuildEquipments, new EquipmentsEqualityComparer()));
            foreach (var equip in StockEquipments.Where(x => x.IsSelected))
                equip.Equipment.Count -= equip.SelectedCount;
            StockEquipments.RemoveRange(StockEquipments.Where(x => x.Equipment.Count == 0).ToList());
        }

        private bool IsSelectedEquipmentsValid => StockEquipments
            .Where(x => x.IsSelected)
            .All(x => x.Equipment.Count >= x.SelectedCount);

        private IEnumerable<Equipments> SelectedEquipments => StockEquipments
            .Where(x => x.IsSelected)
            .Select(x =>
            {
                var copy = x.Equipment.Copy();
                copy.Count = x.SelectedCount;
                return copy;
            });

        private IEnumerable<(Equipments Build, Equipments Select)> EqualEquipments =>
            SelectedEquipments.Join(BuildEquipments,
                s => s.Name,
                b => b.Name,
                (s, b) => (Build: b, Select: s));
        
        private void BtnFromBuildOne_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBuildEquipment == null) return;
            GetBackCount(SelectedBuildEquipment, 1);
        }

        private void BtnFromBuildAll_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBuildEquipment == null) return;
            GetBackCount(SelectedBuildEquipment, SelectedBuildEquipment.Count);
        }

        private void GetBackCount(Equipments equipment, int count)
        {
            countChangesByEquipName[equipment.Name] -= count;
            if (countChangesByEquipName[equipment.Name] == 0) countChangesByEquipName.Remove(equipment.Name);
            if (!TryGetBackCount(equipment, count, out Equipments equip))
                StockEquipments.Add(new EquipmentHelp { Equipment = equip });
            equipment.Count -= count;
            if (equipment.Count == 0)
                BuildEquipments.Remove(equipment);
        }

        private bool TryGetBackCount(Equipments to, int count, out Equipments newEquip)
        {
            newEquip = null;
            var stockEquip = StockEquipments.FirstOrDefault(x => x.Equipment.Name == to.Name);
            if (stockEquip != null)
            {
                stockEquip.Equipment.Count += count;
                return true;
            }
            newEquip = to.Copy();
            newEquip.Count = count;
            newEquip.Locations = Entities.Context.Locations.First(x => x.Name == "Склад");
            return false;
        }
    }
}