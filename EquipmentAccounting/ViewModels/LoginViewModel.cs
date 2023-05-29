using DevExpress.Mvvm;
using EquipmentAccounting.DataBase;
using EquipmentAccounting.Views;
using System.Linq;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        protected ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

        public string Login { get; set; }
        public string Password { get; set; }

        public ICommand LogIn => new DelegateCommand(() =>
        {
            var user = Entities.Context.Users.Where(x => x.Login == Login && x.Password == Password).FirstOrDefault();
            if (user == null) return;
            var mainVM = new MainViewModel();
            new MainWindow { DataContext = mainVM }.Show();
            CurrentWindowService.Close();
        }, () => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password));
    }
}
