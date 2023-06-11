using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
        public ObservableCollection<Locations> Locations { get; set; }
        public Locations SelectedLocation { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Equipments = new ObservableCollection<Equipments>(Entities.Context.Equipments);
            Locations = new ObservableCollection<Locations>(Entities.Context.Locations);
            dgEquipments.ItemsSource = Equipments;
            cmbLocations.ItemsSource = Locations;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedLocation = Locations.First(x => x.Name == "Склад");
            cmbLocations.SelectedItem = SelectedLocation;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Owner.Show();
            (Owner as LoginWindow).txtBLogin.Clear();
            (Owner as LoginWindow).txtBPassword.Clear();
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
                .Where(x => x.Locations == SelectedLocation)
                .Where(x => $"{x.Name}{x.EquipmentTypes.Name}{x.Count}".ToLower().Contains(text));
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

                // Установка значения ячейки для заголовка
                worksheet.Cells["A1"].Value = "Отчет";

                // Применение форматирования для заголовка
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Size = 14;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells["B1"].Value = $"Дата формирования отчета: {DateTime.Now:dd.MM.yyyy}";
                worksheet.Cells["B1"].Style.Font.Bold = true;
                worksheet.Cells["B1"].Style.Font.Italic = true;
                worksheet.Cells["B1"].Style.Font.Size = 10;
                worksheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Заголовки столбцов
                worksheet.Cells[2, 1].Value = "Имя оборудования";
                worksheet.Cells[2, 2].Value = "Тип оборудования";
                worksheet.Cells[2, 3].Value = "Расположение";
                worksheet.Cells[2, 4].Value = "Кол-во всего, шт.";

                int i = 0;
                // Заполнение данных
                for (; i < equipments.Count; i++)
                {
                    Equipments equipment = equipments[i];
                    worksheet.Cells[i + 3, 1].Value = equipment.Name;
                    worksheet.Cells[i + 3, 2].Value = equipment.EquipmentTypes.Name;
                    worksheet.Cells[i + 3, 3].Value = equipment.Locations.Name;
                    worksheet.Cells[i + 3, 4].Value = equipment.Count;
                }

                worksheet.Cells[i + 3, 1].Value = "Подпись:";

                // Автонастройка ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }

        private void ComboBoxLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEquipments();
        }

        private void btnShowEquipmentBuilds_Click(object sender, RoutedEventArgs e)
        {
            new EquipmentBuildsWindow().ShowDialog();
        }
    }
}
