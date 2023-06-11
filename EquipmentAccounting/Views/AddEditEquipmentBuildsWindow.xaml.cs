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

        private readonly EquipmentsEqualityComparer equalityComparer = new EquipmentsEqualityComparer();
        private readonly Dictionary<string, int> countChangesByEquipName = new Dictionary<string, int>();
        private readonly EquipmentBuild toEditBuild;

        public AddEditEquipmentBuildsWindow(EquipmentBuild toEditBuild = null)
        {
            this.toEditBuild = toEditBuild;
            InitializeComponent();
            DataContext = this;
            var stockEquips = Entities.Context.Equipments
                .ToList()
                .Where(x => x.Locations.Name == "Склад");
            StockEquipments = new ObservableCollection<EquipmentHelp>(stockEquips.Select(x => new EquipmentHelp { Equipment = x.Copy() }));
            Locations = new ObservableCollection<Locations>(Entities.Context.Locations
                .ToList()
                .Where(x => x.Name != "Склад"));
            cmbLocs.ItemsSource = Locations;
            SelectedLocation = toEditBuild is null ? Locations.FirstOrDefault() : Locations.First(x => x.Name == toEditBuild.Locations.Name);
            Title = toEditBuild is null ? "Создание сборки" : "Редактирование сборки";
            btnSave.Content = toEditBuild is null ? "Создать" : "Редактировать";
            BuildEquipments = toEditBuild is null
                ? new ObservableCollection<Equipments>()
                : new ObservableCollection<Equipments>(toEditBuild.Equipments.Select(x => x.Copy()));
            Loaded += (s, e) => cmbLocs.SelectedItem = SelectedLocation;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!BuildEquipments.Any())
            {
                MessageBox.Show("Сборка не может быть пуста");
                return;
            }
            if (toEditBuild is null)
                AddBuild();
            else
                EditBuild();
            DialogResult = true;
        }

        private void AddBuild()
        {
            foreach (var eq in BuildEquipments)
                eq.Locations = SelectedLocation;
            var inventoryNum = InventoryNumberGenerator.GenerateInventoryNumber();
            ChangeEquipmentsInDBAdd();
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
            ChangeEquipmentsInDBEdit();
            toEditBuild.Locations = SelectedLocation;
            toEditBuild.Date = DateTime.Now;
            var stockLocId = StockEquipments.First().Equipment.LocationID;
            foreach (var equip in toEditBuild.Equipments)
            {
                var stockEq = Entities.Context.Equipments.First(x => x.Name == equip.Name
                    && x.LocationID == stockLocId);
                DBHelp.RemoveEquipment(equip, stockEq);
            }
            foreach (var equipment in BuildEquipments)
            {
                equipment.Locations = SelectedLocation;
                toEditBuild.Equipments.Add(equipment);
            }
            Entities.Context.SaveChanges();
        }

        private void ChangeEquipmentsInDBAdd()
        {
            foreach (var pair in countChangesByEquipName)
            {
                var equipment = Entities.Context.Equipments.First(x => x.Name == pair.Key);
                equipment.Count -= pair.Value;
                if (equipment.Count == 0)
                    DBHelp.RemoveEquipment(equipment, BuildEquipments.First(x => x.Name == pair.Key));
            }
            Entities.Context.SaveChanges();
        }

        private void ChangeEquipmentsInDBEdit()
        {
            var stockLocId = Entities.Context.Locations.First(x => x.Name == "Склад").ID;
            foreach (var equip in StockEquipments)
            {
                var dbEquip = Entities.Context.Equipments.FirstOrDefault(x => x.Name == equip.Equipment.Name
                    && x.LocationID == stockLocId);
                if (dbEquip is null)
                    Entities.Context.Equipments.Add(equip.Equipment);
                else dbEquip.Count = equip.Equipment.Count;
            }
            foreach (var e in ToDeleteEquipments(stockLocId))
            {
                var substitute = BuildEquipments.First(x => x.Name == e.Name);
                DBHelp.RemoveEquipment(e, substitute);
            }
            Entities.Context.SaveChanges();
        }

        private IEnumerable<Equipments> ToDeleteEquipments(int stockLocId) => Entities.Context.Equipments
            .ToList()
            .Where(x => x.LocationID == stockLocId)
            .Except(StockEquipments.Select(x => x.Equipment), equalityComparer);

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
            BuildEquipments.AddRange(SelectedEquipments.Except(BuildEquipments, equalityComparer));
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
            if (countChangesByEquipName.ContainsKey(equipment.Name))
            {
                countChangesByEquipName[equipment.Name] -= count;
                if (countChangesByEquipName[equipment.Name] == 0)
                    countChangesByEquipName.Remove(equipment.Name);
            }
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