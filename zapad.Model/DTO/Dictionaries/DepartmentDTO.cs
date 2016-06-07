using System;
using System.Xml.Linq;
using zapad.Model.DTO.Dictionaries.Base;

namespace zapad.Model.DTO.Dictionaries
{
    public class DepartmentDTO : BaseDictionaryDTO<Guid>
    {
        private static readonly string _dictionaryName = "Department";

        private static readonly string _arrayName = "Departments";

        public DepartmentDTO() : base(_dictionaryName)
        {

        }

        /// <summary>
        /// Преобразует экземпляр DepartmentDTO в XElement для передачи в запросах
        /// </summary>
        /// <param name="department">Экземпляр DepartmentDTO</param>
        /// <returns>XElement с данными</returns>
        public static XElement ToXElement(DepartmentDTO department)
        {
            return BaseDictionaryDTO<Guid>.ToXElement(_dictionaryName, department);
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public static DepartmentDTO FromXElement(XElement xml)
        {
            return (DepartmentDTO)BaseDictionaryDTO<Guid>.FromXElement(_dictionaryName, xml);
        }

        /// <summary>
        /// Преобразует массив экземпляров DepartmentDTO в XElement
        /// </summary>
        /// <param name="departments">Исходный массив экземпляров DepartmentDTO</param>
        /// <returns>Результирующий XElement</returns>
        public static XElement ArrayToXElement(DepartmentDTO[] departments)
        {
            return BaseDictionaryDTO<Guid>.ArrayToXElement(_arrayName, _dictionaryName, departments);
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров DepartmentDTO
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров DepartmentDTO</returns>
        public static DepartmentDTO[] ArrayFromXElement(XElement xml)
        {
            return Array.ConvertAll<BaseDictionaryDTO<Guid>, DepartmentDTO>(BaseDictionaryDTO<Guid>.ArrayFromXElement(_arrayName, _dictionaryName, xml), new Converter<BaseDictionaryDTO<Guid>, DepartmentDTO>(BaseToPeople));
        }

        public static DepartmentDTO BaseToPeople(BaseDictionaryDTO<Guid> @base)
        {
            return new DepartmentDTO() { Id = @base.Id, Name = @base.Name };
        }
    }
}
