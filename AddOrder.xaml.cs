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

namespace NeonBoard
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        private Order currentOrder = new Order();
        
        public AddOrder(Order selectedOrder)
        {
            if (selectedOrder != null)
                currentOrder = selectedOrder;
            
            
            currentOrder.FullPrice = 0;
            DataContext = currentOrder;

            
            InitializeComponent();
            EndDateBox.DisplayDateStart = DateTime.Today.AddDays(1);
            StartDateBox.DisplayDateStart = DateTime.Today.AddDays(-1);
            StartDateBox.DisplayDate = DateTime.Today;
            EndDateBox.DisplayDate = DateTime.Today.AddDays(1);
            BoardBox.ItemsSource = NeonBoardEntities.GetContext().Signboard.ToList();
            ClientBox.ItemsSource = NeonBoardEntities.GetContext().Client.ToList();
            if (BoardBox.SelectedIndex != -1)
            BoardPrice();
        }
        private void BoardPrice()
        {
            int BoardPrice = 0;

            BoardPrice += currentOrder.Signboard.BaseHeight * 10; // Высотка вывески * Рублей за каждый см

            BoardPrice += currentOrder.Signboard.BaseWidth * 10; // Ширина вывески * Рублей за каждый см

            BoardPrice += currentOrder.Signboard.NeonLength * 50; // Длина неона * Рублей за каждый м

            BoardPrice += currentOrder.Signboard.CountOfElements * 50; // Количество элементов * Рублей за каждый элемент

            BoardPrice += currentOrder.Signboard.BaseMaterial.PriceForMaterial; // Рублей за вид материала из таблицы материалов

            BoardPrice += currentOrder.Signboard.BaseType.PriceForType; // Рублей за тип основания из таблицы оснований
            currentOrder.FullPrice = BoardPrice;
            FullPriceBox.Text = currentOrder.FullPrice.ToString();
        }
        private void AddOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrder.Description == null)
                currentOrder.Description = "отсуствует     ";
            StringBuilder errors = new StringBuilder();
            if (currentOrder.Signboard == null)
                errors.AppendLine("Выберите вывеску заказа.");
            else
                currentOrder.FullPrice = currentOrder.FullPrice;
            if (currentOrder.Client == null)
                errors.AppendLine("Выберите клиента заказа.");
            if (currentOrder.StartDate == null)
                errors.AppendLine("Выберите дату начала работы над заказом.");
            if (currentOrder.StartDate < DateTime.Today.AddDays(-1))
                errors.AppendLine("Мы не заполняем дату начала работы задним числом!");
            if (currentOrder.EndDate <= currentOrder.StartDate)
                errors.AppendLine("Невозможно сделать вывеску за один вечер.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (currentOrder.IdOrder == 0)
                NeonBoardEntities.GetContext().Order.Add(currentOrder);
            try
            {
                NeonBoardEntities.GetContext().SaveChanges();
                MessageBox.Show("Заказ был сохранен!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BoardBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoardPrice();
            
        }
    }
}
