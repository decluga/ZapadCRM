using System.Collections.Generic;
using System.Xml.Linq;
using zapad.Model.Tools;

namespace zapad.Model.DTO.Dictionaries.Base
{
    public class BaseDictionaryDTO<T> where T : new()
    {
        public T Id { get; set; }
        public System.String Name { get; set; }

        private System.String _dictionaryName;

        public BaseDictionaryDTO(System.String dictionaryName)
        {
            this._dictionaryName = dictionaryName;
        }

        /// <summary>
        /// Преобразует экземпляр DictionaryDTO в XElement для передачи в запросах
        /// </summary>
        /// <param name="dictionary">Экземпляр DictionaryDTO</param>
        /// <returns>XElement с данными пользователя</returns>
        public static XElement ToXElement(string dictionaryName, BaseDictionaryDTO<T> dictionary)
        {
            var xml = new XElement(dictionaryName);
            xml.Add(new XElement("Id", dictionary.Id));
            xml.Add(new XElement("Name", dictionary.Name));
            return xml;
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public static BaseDictionaryDTO<T> FromXElement(string dictionaryName, XElement xml)
        {
            var dictionary = new BaseDictionaryDTO<T>(dictionaryName);
            dictionary.Id = xml.Element("Id").GetValueByPath<T>(new T());
            dictionary.Name = xml.Element("Name").getValue("");
            return dictionary;
        }

        /// <summary>
        /// Преобразует массив экземпляров DictionaryDTO в XElement
        /// </summary>
        /// <param name="dictionaries">Исходный массив экземпляров DictionaryDTO</param>
        /// <returns>Результирующий XElement</returns>
        public static XElement ArrayToXElement(string arrayName, string dictionaryName, BaseDictionaryDTO<T>[] dictionaries)
        {
            XElement xml = new XElement(arrayName);

            foreach (var u in dictionaries)
                xml.Add(BaseDictionaryDTO<T>.ToXElement(dictionaryName,u));

            return xml;
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров DictionaryDTO
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров DictionaryDTO</returns>
        public static BaseDictionaryDTO<T>[] ArrayFromXElement(string arrayName, string dictionaryName, XElement xml)
        {
            List<BaseDictionaryDTO<T>> lst = new List<BaseDictionaryDTO<T>>();

            foreach (var e in xml.Elements(dictionaryName))
                lst.Add(BaseDictionaryDTO<T>.FromXElement(dictionaryName,e));

            return lst.ToArray();
        }
    }
}