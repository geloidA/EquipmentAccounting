using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for EquipmentBuildsWindow.xaml
    /// </summary>
    public partial class EquipmentBuildsWindow : Window
    {
        public ObservableCollection<EquipmentBuild> EquipmentBuilds { get; set; }

        public EquipmentBuildsWindow()
        {
            InitializeComponent();
            EquipmentBuilds = new ObservableCollection<EquipmentBuild>(Entities.Context.EquipmentBuild);
        }

        private void AddNewEquipmentBuild(object sender, RoutedEventArgs e)
        {
            var res = new AddEditEquipmentBuildsWindow().ShowDialog();
            if (res == true)
            {
                EquipmentBuilds.Clear();
                EquipmentBuilds.AddRange(Entities.Context.EquipmentBuild);
            }
        }
    }
}
