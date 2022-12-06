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
            
            if (TypeBox.SelectedIndex > 0)
            {
                currentBoard = currentBoard.Where(item => item.Type.IdType == TypeBox.SelectedItem.ToString()).ToList();
            }
        
            if (MaterialBox.SelectedIndex > 0)
            {
                currentBoard = currentBoard.Where(item => item.Material.Equals(MaterialBox.SelectedItem as BaseMaterial)).ToList();
            }
            currentBoard = currentBoard.Where(item => item.IdBoard.ToString().Contains(FindBoardBox.Text)).ToList(); // Фильтрация по номеру вывески

            ListBoard.ItemsSource = currentBoard;
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
    }
}
