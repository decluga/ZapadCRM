using System;
using System.Xml.Linq;
using zapad.Model.DTO.Dictionaries.Base;

namespace zapad.Model.DTO.Dictionaries
{
    public class EventResultTypeDTO : BaseDictionaryDTO<Int32>
    {
        private static readonly string _dictionaryName = "EventResultType";

        private static readonly string _arrayName = "EventResultTypes";

        public EventResultTypeDTO() : base(_dictionaryName)
        {

        }

        /// <summary>
        /// Преобразует экземпляр EventResultTypeDTO в XElement для передачи в запросах
        /// </summary>
        /// <param name="eventResultType">Экземпляр EventResultTypeDTO</param>
        /// <returns>XElement с данными</returns>
        public static XElement ToXElement(EventResultTypeDTO eventResultType)
        {
            return BaseDictionaryDTO<Int32>.ToXElement(_dictionaryName, eventResultType);
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public static EventResultTypeDTO FromXElement(XElement xml)
        {
            return (EventResultTypeDTO)BaseDictionaryDTO<Int32>.FromXElement(_dictionaryName, xml);
        }

        /// <summary>
        /// Преобразует массив экземпляров EventResultTypeDTO в XElement
        /// </summary>
        /// <param name="eventResultTypes">Исходный массив экземпляров EventResultTypeDTO</param>
        /// <returns>Результирующий XElement</returns>
        public static XElement ArrayToXElement(EventResultTypeDTO[] eventResultTypes)
        {
            return BaseDictionaryDTO<Int32>.ArrayToXElement(_arrayName, _dictionaryName, eventResultTypes);
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров EventResultTypeDTO
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров EventResultTypeDTO</returns>
        public static EventResultTypeDTO[] ArrayFromXElement(XElement xml)
        {
            return Array.ConvertAll<BaseDictionaryDTO<Int32>, EventResultTypeDTO>(BaseDictionaryDTO<Int32>.ArrayFromXElement(_arrayName, _dictionaryName, xml), new Converter<BaseDictionaryDTO<Int32>, EventResultTypeDTO>(BaseToEventResultType));
        }

        public static EventResultTypeDTO BaseToEventResultType(BaseDictionaryDTO<Int32> @base)
        {
            return new EventResultTypeDTO() { Id = @base.Id, Name = @base.Name };
        }
    }
}