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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        public Orders()
        {
            InitializeComponent();

            UpdateData();
            PriceBox.SelectedIndex = 0;
            
        }
        private void UpdateData()
        {
            var currentOrders = NeonBoardEntities.GetContext().Order.ToList();
            

            if (PriceBox.SelectedIndex == 1)// Сортировка по цене
            {
                currentOrders = currentOrders.OrderBy(item => item.FullPrice).ToList();
            }
            else if (PriceBox.SelectedIndex == 2)
            {
                currentOrders = currentOrders.OrderByDescending(item => item.FullPrice).ToList();
            }

             //currentOrders = currentOrders.Where(item => item.IdClient.Surname.ToLower().Contains(FindClientBox.Text.ToLower())).ToList(); // Фильтрация по клиенту      
             currentOrders = currentOrders.Where(item => item.IdOrder.ToString().Contains(FindOrderBox.Text)).ToList(); // Фильтрация по номеру заказа

            ListOrders.ItemsSource = currentOrders;

        }

        private void FindOrderBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void FindClientBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void PriceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
    }
}
