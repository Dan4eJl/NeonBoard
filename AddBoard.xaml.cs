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

namespace NeonBoard.AddPages
{
    /// <summary>
    /// Логика взаимодействия для AddBoard.xaml
    /// </summary>
    public partial class AddBoard : Page
    {
        private Signboard currentBoard = new Signboard();
        public AddBoard(Signboard selectedBoard)
        {
            InitializeComponent();
            if (selectedBoard != null)
                currentBoard = selectedBoard;
            BoardPrice();
            IdBaseMaterialBox.ItemsSource = NeonBoardEntities.GetContext().BaseMaterial.ToList();
            IdBaseTypeBox.ItemsSource = NeonBoardEntities.GetContext().BaseType.ToList();
            DataContext = currentBoard;
        }

        private void AddBoardBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (currentBoard.CountOfElements < 1)
                errors.AppendLine("Количество элементов не может быть меньше 1.");
            if (currentBoard.NeonLength < 1)
                errors.AppendLine("Длина неона не может быть меньше 1 метра.");
            if (currentBoard.BaseWidth < 10)
                errors.AppendLine("Ширина основания не может быть меньше 10см.");
            if (currentBoard.BaseHeight < 10)
                errors.AppendLine("Высота основания не может быть меньше 10см.");
            if (currentBoard.BaseType == null)
                errors.AppendLine("Выберите тип основания.");
            if (currentBoard.BaseMaterial == null)
                errors.AppendLine("Выберите материал основания.");
            if (currentBoard.PriceForBoard < 1)
                errors.AppendLine("Цена ниже 1");
            if (String.IsNullOrWhiteSpace(currentBoard.Sketch))
            {
                currentBoard.Sketch = null;
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (currentBoard.IdBoard == 0)
                NeonBoardEntities.GetContext().Signboard.Add(currentBoard);
            try
            {
                NeonBoardEntities.GetContext().SaveChanges();
                MessageBox.Show("Вывеска была сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
       
        private void BoardPrice()
        {
            int BoardPrice = 0;
            if (BaseHeightBox != null)
            {
                BoardPrice += currentBoard.BaseHeight * 10; // Высотка вывески * Рублей за каждый см
            }
            if (BaseWidthBox != null)
            {
                BoardPrice += currentBoard.BaseWidth * 10; // Ширина вывески * Рублей за каждый см
            }
            if (NeonLengthBox != null)
            {
                BoardPrice += currentBoard.NeonLength * 50; // Длина неона * Рублей за каждый м
            }
            if (CountOfElementsBox != null)
            {
                BoardPrice += currentBoard.CountOfElements * 50; // Количество элементов * Рублей за каждый элемент
            }
            if (IdBaseMaterialBox.SelectedIndex > -1)
            {
                BoardPrice += currentBoard.BaseMaterial.PriceForMaterial; // Рублей за вид материала из таблицы материалов
            }
            if (IdBaseTypeBox.SelectedIndex > -1)
            {
                BoardPrice += currentBoard.BaseType.PriceForType; // Рублей за тип основания из таблицы оснований
            }
            currentBoard.PriceForBoard = BoardPrice;
            PriceForBoardText.Text = BoardPrice.ToString();

        }
        private void IdBaseTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoardPrice();
        }

        private void IdBaseMaterialBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoardPrice();
        }

        private void BaseHeightBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BoardPrice();
        }

        private void BaseWidthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BoardPrice();
        }

        private void NeonLengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BoardPrice();
        }

        private void CountOfElementsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BoardPrice();
        }

        private void BaseHeightBox_MouseLeave(object sender, MouseEventArgs e)
        {
            BoardPrice();
        }

        private void BaseWidthBox_MouseLeave(object sender, MouseEventArgs e)
        {
            BoardPrice();
        }

        private void NeonLengthBox_MouseLeave(object sender, MouseEventArgs e)
        {
            BoardPrice();
        }

        private void CountOfElementsBox_MouseLeave(object sender, MouseEventArgs e)
        {
            BoardPrice();
        }

        private void SketchIm_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            currentBoard.Sketch = null;
            MessageBox.Show("Запрашиваемого эскиза нет в папке. Проверьте имя и путь.");
            SketchIm.Source = null;
        }
        public static bool boardPriceTest(int BaseHeightBox, int BaseWidthBox, int NeonLengthBox, int CountOfElementsBox, int MaterialPriceBox, int TypePriceBox)
        {
            try
            {
                int BoardPrice = 0;
                if (BaseHeightBox > 0)
                {
                    BoardPrice += BaseHeightBox * 10; // Высотка вывески * Рублей за каждый см
                }
                else return false;
                if (BaseWidthBox > 0)
                {
                    BoardPrice += BaseWidthBox * 10; // Ширина вывески * Рублей за каждый см
                }
                else return false;
                if (NeonLengthBox > 0)
                {
                    BoardPrice += NeonLengthBox * 50; // Длина неона * Рублей за каждый м
                }
                else return false;
                if (CountOfElementsBox > 0)
                {
                    BoardPrice += CountOfElementsBox * 50; // Количество элементов * Рублей за каждый элемент
                }
                else return false;
                if (MaterialPriceBox > 0)
                {
                    BoardPrice += MaterialPriceBox; // Рублей за вид материала из таблицы материалов
                }
                else return false;
                if (TypePriceBox > 0)
                {
                    BoardPrice += TypePriceBox; // Рублей за вид материала из таблицы материалов
                }
                else return false;
                return true;

            }
            catch
            {
                return false;
            }

        }
        public static bool correctAddBoard(int Type, int Material, int Height, int Width, int NeonLength, int CountOfElements, string sketch)
        {
            try
            {
                Signboard testBoard = new Signboard();
                
                testBoard.Type = Type;
                testBoard.Material = Material;
                testBoard.BaseHeight = Height;
                testBoard.BaseWidth = Width;
                testBoard.NeonLength = NeonLength;
                testBoard.CountOfElements = CountOfElements;
                testBoard.Sketch = sketch;

                NeonBoardEntities.GetContext().Signboard.Add(testBoard);
                NeonBoardEntities.GetContext().SaveChanges();
                NeonBoardEntities.GetContext().Signboard.Remove(testBoard);
                NeonBoardEntities.GetContext().SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
