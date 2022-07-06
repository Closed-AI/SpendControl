using System.Collections.Generic;

namespace SpendControl
{
    public class Model
    {
        private const string PATH = "data.db";

        private List<Operation> _operations;
        private List<string> _gainCategories;
        private List<string> _spendCategories;

        public Operation Buff { get; set; } // буфер для новых операций

        public Model()
        {
            _operations = new List<Operation>();
            _gainCategories  = new List<string>();
            _spendCategories = new List<string>();

            Buff = null;
        }

        public List<Operation> Operations
        { get => _operations; set => _operations = value; }

        public List<string> GainCategories
        { get => _gainCategories; set => _gainCategories = value; }

        public List<string> SpendCategories
        { get => _spendCategories; set => _spendCategories = value; }

        public void AddOperation(Operation operation)
        {
            _operations.Add(operation);

            int SIZE = _operations.Count - 1;
            int id;

            for (id = SIZE - 1; id >= 0; id--)
            {
                if (_operations[SIZE].OperationDate >= _operations[id].OperationDate)
                    break;
            }

            if (id < 0) id = 0;

            var buff = _operations[id];
            _operations[id] = _operations[SIZE];
            _operations[SIZE] = buff;
        }

        public void AddStartCategories()
        {
            // начальные категории
            //
            // доходов =>
            _gainCategories.Add("Зарплата");
            _gainCategories.Add("Инвестиции");
            //
            // расходов =>
            _spendCategories.Add("Продукты");
            _spendCategories.Add("Жильё");
            _spendCategories.Add("Связь");
            _spendCategories.Add("Одежда");
            _spendCategories.Add("Транспорт");
            _spendCategories.Add("Хозяйственные товары");
            _spendCategories.Add("Развлечения");
            _spendCategories.Add("Техника");
            _spendCategories.Add("Товары для дома");
            _spendCategories.Add("Медицина");
        }
    }
}