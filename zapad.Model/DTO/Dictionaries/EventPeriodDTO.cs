using System;
using System.Xml.Linq;
using zapad.Model.DTO.Dictionaries.Base;

namespace zapad.Model.DTO.Dictionaries
{
    public class EventPeriodDTO : BaseDictionaryDTO<Int32>
    {
        private static readonly string _dictionaryName = "EventPeriod";

        private static readonly string _arrayName = "EventPeriods";

        public EventPeriodDTO() : base(_dictionaryName)
        {

        }

        /// <summary>
        /// Преобразует экземпляр EventPeriodDTO в XElement для передачи в запросах
        /// </summary>
        /// <param name="eventPeriod">Экземпляр EventPeriodDTO</param>
        /// <returns>XElement с данными</returns>
        public static XElement ToXElement(EventPeriodDTO eventPeriod)
        {
            return BaseDictionaryDTO<Int32>.ToXElement(_dictionaryName, eventPeriod);
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public static EventPeriodDTO FromXElement(XElement xml)
        {
            return (EventPeriodDTO)BaseDictionaryDTO<Int32>.FromXElement(_dictionaryName, xml);
        }

        /// <summary>
        /// Преобразует массив экземпляров EventPeriodDTO в XElement
        /// </summary>
        /// <param name="eventPeriods">Исходный массив экземпляров EventPeriodDTO</param>
        /// <returns>Результирующий XElement</returns>
        public static XElement ArrayToXElement(EventPeriodDTO[] eventPeriods)
        {
            return BaseDictionaryDTO<Int32>.ArrayToXElement(_arrayName, _dictionaryName, eventPeriods);
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров EventPeriodDTO
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров EventPeriodDTO</returns>
        public static EventPeriodDTO[] ArrayFromXElement(XElement xml)
        {
            return Array.ConvertAll<BaseDictionaryDTO<Int32>, EventPeriodDTO>(BaseDictionaryDTO<Int32>.ArrayFromXElement(_arrayName, _dictionaryName, xml), new Converter<BaseDictionaryDTO<Int32>, EventPeriodDTO>(BaseToEventResultType));
        }

        public static EventPeriodDTO BaseToEventResultType(BaseDictionaryDTO<Int32> @base)
        {
            return new EventPeriodDTO() { Id = @base.Id, Name = @base.Name };
        }
    }
}
