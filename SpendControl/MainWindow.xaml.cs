using System;
using System.Windows;
using System.Windows.Controls;

namespace SpendControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler newOperationEvent = null;
        public event EventHandler makeExcelReportEvent = null;
        public event EventHandler applicationCloseEvent = null;

        public MainWindow()
        {
            InitializeComponent();

            double[] values = { 0 };
            string[] labels = { "label" };
            InitPlots(values, labels, values, labels);

            new Prezentor(this);
        }

        public void InitPlots(double[] valuesGain, string[] labelsGain, double[] valuesSpend, string[] labelsSpend)
        {
            var plt = ChartGain.Plot;
            plt.Clear();
            var pie = plt.AddPie(valuesGain);
            pie.SliceLabels = labelsGain;
            pie.ShowPercentages = true;
            pie.ShowValues = true;
            pie.ShowLabels = true;

            var plt2 = ChartSpend.Plot;
            plt2.Clear();
            var pie2 = plt2.AddPie(valuesSpend);
            pie2.SliceLabels = labelsSpend;
            pie2.ShowPercentages = true;
            pie2.ShowValues = true;
            pie2.ShowLabels = true;

            try
            {
                ChartGain.Refresh();
                ChartSpend.Refresh();
            }
            catch (Exception)
            { }
        }

        // запрет выделения текста
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                e.Handled = true;
                if ((sender as TextBox).SelectionLength != 0)
                    (sender as TextBox).SelectionLength = 0;
            }
        }

        private void AddOperationButton_Click(object sender, RoutedEventArgs e)
        {
            newOperationEvent.Invoke(sender, e);
        }

        private void MakeExcelReportButton_Click(object sender, RoutedEventArgs e)
        {
            makeExcelReportEvent.Invoke(sender, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            applicationCloseEvent.Invoke(sender, e);
        }

        private void OperationListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (OperationListBox.SelectedItem != null)
            {
                // TODO: редактирование/удаление элемента
            }
        }
    }
}