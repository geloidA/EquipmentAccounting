using EquipmentAccounting.DataBase;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DistributionsWindow.xaml
    /// </summary>
    public partial class DistributionsWindow : Window
    {
        public DistributionsWindow()
        {
            InitializeComponent();
        }

        public Users User { get; internal set; }
    }
}
