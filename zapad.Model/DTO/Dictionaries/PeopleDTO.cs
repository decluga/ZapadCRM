using System;
using System.Xml.Linq;
using zapad.Model.DTO.Dictionaries.Base;

namespace zapad.Model.DTO.Dictionaries
{
    public class PeopleDTO : BaseDictionaryDTO<Guid>
    {
        private static readonly string _dictionaryName = "People";

        private static readonly string _arrayName = "Peoples";

        public PeopleDTO() : base(_dictionaryName)
        {

        }

        /// <summary>
        /// Преобразует экземпляр PeopleDTO в XElement для передачи в запросах
        /// </summary>
        /// <param name="people">Экземпляр PeopleDTO</param>
        /// <returns>XElement с данными</returns>
        public static XElement ToXElement(PeopleDTO people)
        {
            return BaseDictionaryDTO<Guid>.ToXElement(_dictionaryName, people);
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public static PeopleDTO FromXElement(XElement xml)
        {
            return (PeopleDTO)BaseDictionaryDTO<Guid>.FromXElement(_dictionaryName, xml);
        }

        /// <summary>
        /// Преобразует массив экземпляров PeopleDTO в XElement
        /// </summary>
        /// <param name="peoples">Исходный массив экземпляров PeopleDTO</param>
        /// <returns>Результирующий XElement</returns>
        public static XElement ArrayToXElement(PeopleDTO[] peoples)
        {
            return BaseDictionaryDTO<Guid>.ArrayToXElement(_arrayName, _dictionaryName, peoples);
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров PeopleDTO
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров PeopleDTO</returns>
        public static PeopleDTO[] ArrayFromXElement(XElement xml)
        {
            return Array.ConvertAll<BaseDictionaryDTO<Guid>, PeopleDTO>(BaseDictionaryDTO<Guid>.ArrayFromXElement(_arrayName, _dictionaryName, xml), new Converter<BaseDictionaryDTO<Guid>, PeopleDTO>(BaseToPeople));
        }

        public static PeopleDTO BaseToPeople(BaseDictionaryDTO<Guid> @base)
        {
            return new PeopleDTO() { Id = @base.Id, Name = @base.Name };
        }

    }
}
