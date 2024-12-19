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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BusinessPartners.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartnerHistoryPage.xaml
    /// </summary>
    public partial class PartnerHistoryPage : Page
    {
        private readonly int _partnerId;

        public PartnerHistoryPage(int partnerId)
        {
            InitializeComponent();
            _partnerId = partnerId;
            LoadHistory();
        }

        private void LoadHistory()
        {
            try
            {
                using (var context = new Entityes())
                {
                    var history = context.PartnerProducts
                        .Where(pp => pp.PartnerID == _partnerId)
                        .Join(context.Products,
                              pp => pp.ProductID,
                              product => product.ProductID,
                              (pp, product) => new ProductHistory
                              {
                                  ProductName = product.ProductName,
                                  Quantity = pp.Quantity,
                                  SaleDate = pp.SaleDate
                              }).ToList();

                    DataGridHistory.ItemsSource = history;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCalculateMaterial_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Entityes())
            {
                if (DataGridHistory.SelectedItem is ProductHistory selectedRow)
                {
                    var product = context.Products.FirstOrDefault(p => p.ProductName == selectedRow.ProductName);
                    if (product == null)
                    {
                        MessageBox.Show("Продукт не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int productTypeId = product.ProductTypeID;
                    int materialTypeId = context.MaterialType.FirstOrDefault()?.MaterialTypeID ?? -1;
                    if (materialTypeId == -1)
                    {
                        MessageBox.Show("Тип материала не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    double param1 = 1.5;
                    double param2 = 2.0;

                    int requiredMaterial = MaterialCalculator.CalculateMaterialRequirement(
                        productTypeId: productTypeId,
                        materialTypeId: materialTypeId,
                        quantity: selectedRow.Quantity,
                        param1: param1,
                        param2: param2);

                    if (requiredMaterial == -1)
                    {
                        MessageBox.Show("Ошибка: некорректные данные для расчёта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Для производства {selectedRow.Quantity} единиц продукции требуется {requiredMaterial} материала.",
                                        "Результат расчёта", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите запись в таблице.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public class ProductHistory
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public DateTime SaleDate { get; set; }
        }
    }
}
