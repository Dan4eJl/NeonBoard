using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Page
    {
        public Clients()
        {
            InitializeComponent();
            
            UpdateData();
        }
        private void UpdateData()
        {
            var currentClient = NeonBoardEntities.GetContext().Client.ToList();
            var AllClients = currentClient;
            currentClient = currentClient.Where(item => item.Surname.ToLower().Contains(SurnameBox.Text.ToLower())).ToList(); // Фильтрация по фамилии    
            currentClient = currentClient.Where(item => item.Name.ToLower().Contains(NameBox.Text.ToLower())).ToList(); // Фильтрация по имени
            currentClient = currentClient.Where(item => item.Patronym.ToLower().Contains(PatronymBox.Text.ToLower())).ToList(); // Фильтрация по отчеству
            ListClients.ItemsSource = currentClient;
            ResultCount.Content = "Найдено " + currentClient.Count.ToString() + " из " + AllClients.Count.ToString();

        }
        private void LettersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-ZА-Яа-я]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void SurnameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void PatronymBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                NeonBoardEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(item => item.Reload());
                UpdateData();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddClient(null));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListClients.SelectedItem == null) // Отслеживание выбранного элемента
                MessageBox.Show("Выберите клиента для изменения."); 
            else
                Manager.MainFrame.Navigate(new AddClient(ListClients.SelectedItem as Client)); //Передача данных выбранного клиента на страницу добавления
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListClients.SelectedItem == null)
                MessageBox.Show("Выберите клиента(ов) для удаления.");
            else
            {
                var DeleteClients = ListClients.SelectedItems.Cast<Client>().ToList();
                if (MessageBox.Show($"Вы уверены, что хотите удалить {DeleteClients.Count()} клиента(ов)?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        NeonBoardEntities.GetContext().Client.RemoveRange(DeleteClients);
                        NeonBoardEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные клиента(ов) были удалены!");
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
