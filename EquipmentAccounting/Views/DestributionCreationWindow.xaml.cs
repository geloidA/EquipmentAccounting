﻿using EquipmentAccounting.DataBase;
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
        public string Description { get; set; }

        public DestributionCreationWindow()
        {
            InitializeComponent();
            Equipments = new ObservableCollection<EquipmentHelp>(Entities.Context.Equipments.Select(x => new EquipmentHelp { Equipment = x }));
            DataContext = this;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (!IsSelectedEquipmentsValid)
            {
                MessageBox.Show("Оборудования выбрано больше чем возможно");
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
                    EquipmentCount = item.SelectedCount
                });
            }
            Entities.Context.SaveChanges();
            Close();
        }

        private bool IsSelectedEquipmentsValid => Equipments
            .Where(x => x.IsSelected)
            .All(x => x.Equipment.CountInStock >= x.SelectedCount);

        private void Cancel(object sender, RoutedEventArgs e) => Close();
    }
}
