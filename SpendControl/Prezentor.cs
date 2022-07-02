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
        private ViewController _view;

        public Prezentor(MainWindow window)
        {
            LoadData();

            _window = window;
            _window.newOperationEvent += new EventHandler(window_newOperationEvent);
            _window.makeExcelReportEvent += new EventHandler(window_makeExcelReportEvent);
            _window.applicationCloseEvent += new EventHandler(window_applicationCloseEvent);

            _view = new ViewController(_model, _window);

            UpdateView();
        }

        private void window_newOperationEvent(object sender, EventArgs e)
        {
            OperationWindow operationWindow = new OperationWindow(_model);
            operationWindow.Owner = _window;
            operationWindow.ShowDialog();

            if (_model.Buff == null) return;

            _model.AddOperation(_model.Buff);
            _model.Buff = null;

            UpdateView();
        }

        private void window_makeExcelReportEvent(object sender, EventArgs e)
        {
            ExcelReportGenerator.MakeReport(_model.Operations);
        }

        private void window_applicationCloseEvent(object sender, EventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            SaveSystem.SaveData(_model);
        }

        private void LoadData()
        {
            _model = SaveSystem.LoadData();
        }

        private void UpdateView()
        {
            _view.UpdateView();
        }
    }
}