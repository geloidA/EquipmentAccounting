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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = Entities.Context.Users.FirstOrDefault(x => x.Login == txtBLogin.Text && x.Password == txtBPassword.Password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            new MainWindow { User = user, Owner = this }.Show();
            this.Hide();
        }
    }
}
