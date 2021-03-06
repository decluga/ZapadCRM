﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace zapad.Public.WebInterface.Models.Tools
{
    public class KendoFilterDescription
    {
        public string @operator { get; set; }
        public string field { get; set; }
        public string value { get; set; }

        public List<KendoFilterDescription> filters { get; set; }
        public string logic { get; set; }
    }

    public class KendoFilter
    {
        public List<KendoFilterDescription> filters { get; set; }
        public string logic { get; set; }

        private static void AddFiltersNode(ref XElement request)
        {
            if (!request.Elements("filters").Any())
                request.Add(new XElement("filters"));
        }

        public static void AddRequestFilters(ref XElement request, KendoFilter kendoFilter)
        {
            AddFiltersNode(ref request);
            if (kendoFilter != null && kendoFilter.filters != null)
            {
                foreach (var filter in kendoFilter.filters)
                {
                    if (filter.filters == null)
                    {
                        request.Element("filters").Add(new XElement("filter"
                            , new XElement("field", filter.field)
                            , new XElement("operator", filter.@operator)
                            , new XElement("value", filter.value)
                            )
                        );
                    }
                    else
                    {
                        AddRequestFilters(ref request, filter.filters);
                    }
                }
            }
        }

        private static void AddRequestFilters(ref XElement request, List<KendoFilterDescription> filters)
        {
            if (filters.Count > 0)
            {
                var filterValuesElement = new XElement("values");

                foreach (var filter in filters)
                {
                    filterValuesElement.Add(new XElement("value", filter.value));
                }

                request.Element("filters").Add(new XElement("filter"
                        , new XElement("field", filters[0].field)
                        , new XElement("operator", filters[0].@operator)
                        , filterValuesElement
                    )
                );
            }
        }

        public static void AddRequestFilters(ref XElement request, string filterName, string filterValue)
        {
            throw new NotImplementedException();
        }
    }

    public class KendoSort
    {
        public string field { get; set; }
        public string dir { get; set; }
    }
}