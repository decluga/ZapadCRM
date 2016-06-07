using System;
using System.Xml.Linq;
using zapad.Model.DTO.Dictionaries.Base;

namespace zapad.Model.DTO.Dictionaries
{
    public class EventStatusDTO : BaseDictionaryDTO<Int32>
    {
        private static readonly string _dictionaryName = "EventStatus";

        private static readonly string _arrayName = "EventStatuses";

        public EventStatusDTO() : base(_dictionaryName)
        {

        }

        /// <summary>
        /// Преобразует экземпляр EventStatusDTO в XElement для передачи в запросах
        /// </summary>
        /// <param name="eventStatus">Экземпляр EventStatusDTO</param>
        /// <returns>XElement с данными</returns>
        public static XElement ToXElement(EventStatusDTO eventStatus)
        {
            return BaseDictionaryDTO<Int32>.ToXElement(_dictionaryName, eventStatus);
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public static EventStatusDTO FromXElement(XElement xml)
        {
            return (EventStatusDTO)BaseDictionaryDTO<Int32>.FromXElement(_dictionaryName, xml);
        }

        /// <summary>
        /// Преобразует массив экземпляров EventStatusDTO в XElement
        /// </summary>
        /// <param name="eventStatuses">Исходный массив экземпляров EventStatusDTO</param>
        /// <returns>Результирующий XElement</returns>
        public static XElement ArrayToXElement(EventStatusDTO[] eventStatuses)
        {
            return BaseDictionaryDTO<Int32>.ArrayToXElement(_arrayName, _dictionaryName, eventStatuses);
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров EventStatusDTO
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров EventStatusDTO</returns>
        public static EventStatusDTO[] ArrayFromXElement(XElement xml)
        {
            return Array.ConvertAll<BaseDictionaryDTO<Int32>, EventStatusDTO>(BaseDictionaryDTO<Int32>.ArrayFromXElement(_arrayName, _dictionaryName, xml), new Converter<BaseDictionaryDTO<Int32>, EventStatusDTO>(BaseToEventStatus));
        }

        public static EventStatusDTO BaseToEventStatus(BaseDictionaryDTO<Int32> @base)
        {
            return new EventStatusDTO() { Id = @base.Id, Name = @base.Name };
        }
    }
}