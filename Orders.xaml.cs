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
using System.Globalization;
using static System.Windows.Data.IValueConverter;
using System.Text.RegularExpressions;

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
            var AllOrders = currentOrders;

            if (PriceBox.SelectedIndex == 1)// Сортировка по цене
            {
                currentOrders = currentOrders.OrderBy(item => item.FullPrice).ToList();
            }
            else if (PriceBox.SelectedIndex == 2)
            {
                currentOrders = currentOrders.OrderByDescending(item => item.FullPrice).ToList();
            }

             currentOrders = currentOrders.Where(item => item.Client.Surname.ToLower().Contains(FindClientBox.Text.ToLower())).ToList(); // Фильтрация по клиенту      
             currentOrders = currentOrders.Where(item => item.IdOrder.ToString().Contains(FindOrderBox.Text)).ToList(); // Фильтрация по номеру заказа

            ListOrders.ItemsSource = currentOrders;

            ResultCount.Content = "Найдено " + currentOrders.Count.ToString() + " из " + AllOrders.Count.ToString();
            
            
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void LettersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-ZА-Яа-я]+");
            e.Handled = regex.IsMatch(e.Text);
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

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddOrder(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                NeonBoardEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(item => item.Reload());
                UpdateData();
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListOrders.SelectedItem == null)
                MessageBox.Show("Выберите заказ для изменения.");
            else
                Manager.MainFrame.Navigate(new AddOrder(ListOrders.SelectedItem as Order));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListOrders.SelectedItem == null)
                MessageBox.Show("Выберите заказ(ы) для удаления.");
            else
            {
                var DeleteOrder = ListOrders.SelectedItems.Cast<Order>().ToList();
                if (MessageBox.Show($"Вы уверены, что хотите удалить следующие {DeleteOrder.Count()} заказы?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        NeonBoardEntities.GetContext().Order.RemoveRange(DeleteOrder);
                        NeonBoardEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные заказа(ов) были удалены!");
                        UpdateData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
    }
}
