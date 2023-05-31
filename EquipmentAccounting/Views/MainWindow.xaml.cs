using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Users User { get; set; }

        public ObservableCollection<Equipments> Equipments { get; set; }

        public MainWindow()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
            Equipments = new ObservableCollection<Equipments>(Entities.Context.Equipments);
            dgEquipments.ItemsSource = Equipments;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Owner.Show();
        }

        public void UpdateEquipments()
        {
            Equipments.Clear();
            Equipments.AddRange(GetEquipmentsContainedText(searchTxtBox.Text.ToLower()));
        }

        private void BtnShowDeliveries_Click(object sender, RoutedEventArgs e) => new DeliveriesWindow { Owner = this }.ShowDialog();

        private void BtnShowDistributions_Click(object sender, RoutedEventArgs e) => new DistributionsWindow { User = User, Owner = this }.ShowDialog();

        private void SearchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEquipments();
            if (!Equipments.Any())
                MessageBox.Show("Нет результатов поиска");
        }

        private IEnumerable<Equipments> GetEquipmentsContainedText(string text)
        {
            return Entities.Context.Equipments
                .ToList()
                .Where(x => $"{x.Name}{x.Type}{x.CountInStock}".ToLower().Contains(text));
        }

        private void btnCreateExcelReport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { DefaultExt = "xlsx", Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*" };
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult == true)
            {
                GenerateEquipmentReport(Equipments, dialog.FileName);
                MessageBox.Show("Отчет сохранен.");
            }
        }

        public void GenerateEquipmentReport(Collection<Equipments> equipments, string filePath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Отчет по оборудованию");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "Имя оборудования";
                worksheet.Cells[1, 2].Value = "Тип оборудования";
                worksheet.Cells[1, 3].Value = "Кол-во на складе, шт.";
                worksheet.Cells[1, 4].Value = "Кол-во всего, шт.";

                // Заполнение данных
                for (int i = 0; i < equipments.Count; i++)
                {
                    Equipments equipment = equipments[i];
                    worksheet.Cells[i + 2, 1].Value = equipment.Name;
                    worksheet.Cells[i + 2, 2].Value = equipment.Type;
                    worksheet.Cells[i + 2, 3].Value = equipment.CountInStock;
                    worksheet.Cells[i + 2, 4].Value = equipment.CountAll;
                }

                // Автонастройка ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}
