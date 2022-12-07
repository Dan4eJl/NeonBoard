using NeonBoard.AddPages;
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
    /// Логика взаимодействия для Borders.xaml
    /// </summary>
    public partial class Borders : Page
    {
        public Borders()
        {
            InitializeComponent();
            var AllTypes = NeonBoardEntities.GetContext().BaseType.ToList();
            var AllMaterials = NeonBoardEntities.GetContext().BaseMaterial.ToList();

            AllTypes.Insert(0, new BaseType
            {
                TypeName = "Все основания"
            });
            AllMaterials.Insert(0, new BaseMaterial
            {
                MaterialName = "Все материалы"
            });
            TypeBox.ItemsSource = AllTypes;
            MaterialBox.ItemsSource = AllMaterials;
            TypeBox.SelectedIndex = 0;
            MaterialBox.SelectedIndex = 0;

            UpdateData();
        }
        private void UpdateData()
        {
            var currentBoard = NeonBoardEntities.GetContext().Signboard.ToList();
            var AllBoards = currentBoard;
            if (TypeBox.SelectedIndex > 0)
            {
                currentBoard = currentBoard.Where(item=>item.BaseType.IdType == TypeBox.SelectedIndex).ToList(); // Фильтрация по типу основания
            }
            
            if (MaterialBox.SelectedIndex > 0)
            {
                currentBoard = currentBoard.Where(item => item.BaseMaterial.IdMaterial == MaterialBox.SelectedIndex).ToList(); // Фильтрация по материалу
            }
            currentBoard = currentBoard.Where(item => item.IdBoard.ToString().Contains(FindBoardBox.Text)).ToList(); // Фильтрация по номеру вывески

            ListBoard.ItemsSource = currentBoard;
            ResultCount.Content = "Найдено " + currentBoard.Count.ToString() + " из " + AllBoards.Count.ToString();
        }
        private void FindBoardBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void MaterialBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            
            Manager.MainFrame.Navigate(new AddBoard(null));
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
            if (ListBoard.SelectedItem == null)
                MessageBox.Show("Выберите вывеску для изменения.");
            else
            Manager.MainFrame.Navigate(new AddBoard(ListBoard.SelectedItem as Signboard));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoard.SelectedItem == null)
                MessageBox.Show("Выберите вывеску(и) для удаления.");
            else
            {
                var DeleteBoard = ListBoard.SelectedItems.Cast<Signboard>().ToList();
                if (MessageBox.Show($"Вы уверены, что хотите удалить {DeleteBoard.Count()} вывески(ок)?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        NeonBoardEntities.GetContext().Signboard.RemoveRange(DeleteBoard);
                        NeonBoardEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные по вывеске(ам) были удалены!");
                        UpdateData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
