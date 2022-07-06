using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendControl
{
    class ViewController
    {
        private Model _model;
        private MainWindow _window;

        public ViewController(Model model ,MainWindow window)
        {
            _model = model;
            _window = window;
        }

        public void UpdateView()
        {
            UpdateHistory();
            UpdateBalance();
            UpdateCharts();
        }

        private void UpdateHistory()
        {
            var history = _window.OperationListBox.Items;
            history.Clear();

            foreach (var item in _model.Operations)
            {
                history.Insert(0, item.Category + "\n" + item.Value);
            }
        }

        private void UpdateBalance()
        {
            float sumGain = 0;
            float sumSpend = 0;

            foreach (var item in _model.Operations)
            {
                if (item.Type == "Доход")
                    sumGain += item.Value;
                else
                    sumSpend += item.Value;
            }

            _window.GainTextBox.Text = "Доходы\n" + sumGain.ToString();
            _window.SpendTextBox.Text = "Расходы\n" + sumSpend.ToString();
            _window.BalanceTextBox.Text = "Баланс\n" + (sumGain - sumSpend).ToString();
        }

        private void UpdateCharts()
        {
            Dictionary<string, float> percentageGain = new Dictionary<string, float>();
            Dictionary<string, float> percentageSpend = new Dictionary<string, float>();

            foreach (var item in _model.Operations)
            {
                if (item.Type == "Доход")
                {
                    if (!percentageGain.ContainsKey(item.Category))
                        percentageGain[item.Category] = item.Value;
                    else
                        percentageGain[item.Category] += item.Value;
                }
                else
                {
                    if (!percentageSpend.ContainsKey(item.Category))
                        percentageSpend[item.Category] = item.Value;
                    else
                        percentageSpend[item.Category] += item.Value;
                }
            }

            string[] gainKeys = new string[percentageGain.Count];
            string[] spendKeys = new string[percentageSpend.Count];

            double[] gainValues = new double[percentageGain.Count];
            double[] spendValues = new double[percentageSpend.Count];

            int id = 0;

            foreach (var item in percentageGain)
            {
                gainKeys[id] = item.Key;
                gainValues[id] = item.Value;
                id++;
            }

            id = 0;

            foreach (var item in percentageSpend)
            {
                spendKeys[id] = item.Key;
                spendValues[id] = item.Value;
                id++;
            }

            _window.InitPlots(gainValues, gainKeys, spendValues, spendKeys);
        }
    }
}
