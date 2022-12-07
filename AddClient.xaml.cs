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
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Page
    {
        private bool ValidMail;
        private Client currentClient = new Client();
        public AddClient(Client selectedClient)
        {
            InitializeComponent();
            
            if (selectedClient != null)
                currentClient = selectedClient;
                

            DataContext = currentClient;
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
        public bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
        private void AddClientBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentClient.Name) || currentClient.Name.Length < 2 || currentClient.Name.Length > 30)
                errors.AppendLine("Имя должно содержать от 2 до 30 букв.");
            if (string.IsNullOrWhiteSpace(currentClient.Surname) || currentClient.Surname.Length < 2 || currentClient.Surname.Length > 40)
                errors.AppendLine("Фамилия должно содержать от 2 до 40 букв.");
            if (string.IsNullOrWhiteSpace(currentClient.Surname) || currentClient.Patronym.Length < 2 || currentClient.Patronym.Length > 30)
                errors.AppendLine("Отчество должно содержать от 2 до 30 букв.");
            if (string.IsNullOrWhiteSpace(currentClient.Phone) || currentClient.Phone.Length != 11)
                errors.AppendLine("Телефон состоит из 11 цифр");
            if (ValidMail == false)
                errors.AppendLine("Эл.Почта введена некорректно!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (currentClient.IdClient == 0)
                NeonBoardEntities.GetContext().Client.Add(currentClient);
            try
            {
                NeonBoardEntities.GetContext().SaveChanges();
                MessageBox.Show("Клиент был сохранен!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void MailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidMail = IsValidEmailAddress(MailBox.Text);
        }
    }
}
