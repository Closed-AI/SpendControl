using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SpendControl
{
    class Prezentor
    {
        private MainWindow _window;
        private Model _model;

        // Тип для Сериализации и Десериализации.
        readonly XmlSerializer serializer = new XmlSerializer(typeof(Model));

        public Prezentor(MainWindow window)
        {
            LoadData();

            _window = window;
            _window.newOperationEvent += new EventHandler(window_newOperationEvent);
            _window.applicationCloseEvent += new EventHandler(window_applicationCloseEvent);

            UpdateViev();
        }

        private void window_newOperationEvent(object sender, EventArgs e)
        {
            OperationWindow operationWindow = new OperationWindow(_model);
            operationWindow.Owner = _window;
            operationWindow.ShowDialog();

            if (_model.Buff == null) return;

            _model.AddOperation(_model.Buff);
            _model.Buff = null;

            UpdateViev();
        }

        private void window_applicationCloseEvent(object sender, EventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            FileStream stream = new FileStream("Serialization.xml", FileMode.Create, FileAccess.Write, FileShare.Read);

            // Сохраняем объект в XML-файле на диске(СЕРИАЛИЗАЦИЯ).
            serializer.Serialize(stream, _model);
            stream.Close();
        }

        private void LoadData()
        {
            try
            {
                FileStream stream = new FileStream("Serialization.xml", FileMode.Open, FileAccess.Read, FileShare.Read);

                // Восстанавливаем объект из XML-файла.
                _model = serializer.Deserialize(stream) as Model;
            }
            catch (Exception)
            {
                _model = new Model();
                _model.AddStartCategories();
            }
        }

        private void UpdateViev()
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
                history.Add(item.Category + "\n" + item.Value);
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