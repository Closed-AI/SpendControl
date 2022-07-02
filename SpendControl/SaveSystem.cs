using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpendControl
{
    static class SaveSystem
    {
        // Тип для Сериализации и Десериализации.
        private static XmlSerializer serializer = new XmlSerializer(typeof(Model));

        public static void SaveData(Model model)
        {
            FileStream stream = new FileStream("Serialization.xml", FileMode.Create, FileAccess.Write, FileShare.Read);

            // Сохраняем объект в XML-файле на диске(СЕРИАЛИЗАЦИЯ).
            serializer.Serialize(stream, model);
            stream.Close();
        }

        public static Model LoadData()
        {
            Model model = new Model();

            try
            {
                FileStream stream = new FileStream("Serialization.xml", FileMode.Open, FileAccess.Read, FileShare.Read);

                // Восстанавливаем объект из XML-файла.
                model = serializer.Deserialize(stream) as Model;
            }
            catch (Exception)
            {
                model = new Model();
                model.AddStartCategories();
            }

            return model;
        }
    }
}
