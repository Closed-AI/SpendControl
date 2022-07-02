using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SpendControl
{
    /// <summary>
    /// Логика взаимодействия для OperationWindow.xaml
    /// </summary>
    public partial class OperationWindow : Window
    {
        private Model _model;

        public OperationWindow(Model model)
        {
            InitializeComponent();

            _model = model;

            OperationTypeComboBox.SelectedIndex = 0;
            CategoryComboBox.SelectedItem = 0;
        }

        private void OperationTypeIndexChanged(object sender, RoutedEventArgs e)
        {
            List<string> source =
                OperationTypeComboBox.SelectedIndex == 0 ? // индекс доходов
                _model.GainCategories : // доходы
                _model.SpendCategories; // расходы

            var CategoryList = CategoryComboBox.Items;
            CategoryList.Clear();

            foreach (var item in source)
            {
                CategoryList.Add(item);
            }

            CategoryList.Add("Новая категория");
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedIndex != CategoryComboBox.Items.Count - 1) return;

            // если выбрано "новая категория"


        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var description = DiscribtionTextBox.Text;

            //--------------------проверка типа операции---------------------------
            if (OperationTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите тип операции!");
                return;
            }
            int id = OperationTypeComboBox.SelectedIndex;
            string type = id == 0 ? "Доход" : "Раход";
            //---------------------------------------------------------------------


            //--------------------проверка суммы-----------------------------------
            float price;
            try
            {
                price = float.Parse(PriceTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректный формат суммы!");
                return;
            }
            //---------------------------------------------------------------------


            //--------------------проверка категории-------------------------------
            List<string> categorys = new List<string>();

            foreach (var item in CategoryComboBox.Items)
            {
                categorys.Add(item.ToString());
            }

            if (CategoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите категорию!");
                return;
            }

            var category = CategoryComboBox.SelectedItem.ToString();
            var LAST = CategoryComboBox.Items.Count - 1;


            if (!categorys.Contains(category) || category == CategoryComboBox.Items[LAST].ToString())
            {
                MessageBox.Show("Неизвестная категория!");
                return;
            }
            //---------------------------------------------------------------------


            //--------------------проверка даты------------------------------------
            var date = datePicker.SelectedDate;
            if (date==null)
            {
                MessageBox.Show("Укажите дату!");
                return;
            }
            else if (date.Value.Date>DateTime.Now.Date)
            {
                MessageBox.Show("Дата не может быть больше текущей!\n" +
                    "(нельзя создавать будущие операции)");
                return;
            }
            //---------------------------------------------------------------------
            //category = GetCategory(category);
            _model.Buff = new Operation(price, description, category, type, (DateTime)date);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool CorrectOperation(string Ftype, float Fprice, string Fcategory, DateTime? Fdate)
        {
            //string errorDiscription = "";
            //
            //
            //
            //if (Ftype)
            //    errorDiscription = "Неверный тип операции";
            //else if (Fdate == null)
            //    errorDiscription = "Не указана дата";
            //else if (!categorys.Contains(Fcategory) || Fcategory == "Новая категория")
            //    errorDiscription = "Укажите категорию";
            //
            //if (errorDiscription.Length != 0)
            //{
            //    MessageBox.Show(errorDiscription);
            //    return false;
            //}
            //
            return true;
        }

        private string GetCategory(string str)
        {
            var ans = "";
            int i;
            for (i=str.Length-1;i>=0;i--)
            {
                if (str[i] == ' ') break;
            }

            ans = str.Substring(i + 1, str.Length - 1 - i);

            return str;
        }
    }
}