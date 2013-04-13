using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Globalization;
using System.Xml.Linq;

namespace OS.Toolbox.DynamicObjects
{
    public static class ExpandoObjectSerializer
    {
        #region CSV Export

        /// <summary>
        /// convert the content of the object into a string in csv format
        /// - no header
        /// - delimiter: comma
        /// - field quotes: quotes will be used
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AsCsv(dynamic value)
        {
            return AsCsv(value, false, ',', true);
        }

        /// <summary>
        /// convert the objet into a string in csv format  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="withHeader"></param>
        /// <param name="delimiter"></param>
        /// <param name="useQuotesForFields"></param>
        /// <returns></returns>
        public static string AsCsv(dynamic value,
            bool withHeader, char delimiter, bool useQuotesForFields)
        {
            var csvBuilder = new StringBuilder();
            string[] values;
            int index;

            //check input
            if (value == null)
            {
                return "";
            }

            //convert
            IDictionary<string, object> valueDictionary = value;

            if (valueDictionary.Count < 1)
            {
                return "";
            }

            //header
            if (withHeader == true)
            {
                csvBuilder.AppendLine(string.Join(new string(delimiter, 1),
                    (from c in valueDictionary.Keys select c).ToArray()));
            }

            //content
            values = new string[valueDictionary.Count];

            index = 0;
            foreach (object element in valueDictionary.Values)
            {
                values[index] = Converter.ToCsvValue(element, useQuotesForFields);
                index++;
            }

            csvBuilder.AppendLine(string.Join(new string(delimiter, 1), values.ToArray()));

            //return
            return csvBuilder.ToString();
        }

        #endregion

        #region CSV Import

        /// <summary>
        /// sets all values according to the csv file content
        /// 
        /// the standard csv format will be used
        /// - no header
        /// - delimiter: comma
        /// - field quotes: quotes will be used
        /// 
        /// Exceptions
        ///     see 'FromCsv(IEnumerable<string> content, bool withHeader, string delimiter, bool useQuotesForFields)'
        /// </summary>
        /// <param name="content"></param>
        public static dynamic FromCsv(IEnumerable<string> content, List<IDynamicTableColumn> elementDefinitions)
        {
            return FromCsv(content, elementDefinitions, false, ',', true);
        }

        /// <summary>
        /// sets all values according to the csv file content
        /// 
        /// Exceptions
        ///     ArgumentException: input parameter is wrong (null, empty, missing)
        ///     FormatException: input parameter has wrong format
        ///     ArgumentNullException: no columns or column name is null
        ///     ArgumentException: invalid column name (empty name or name not unique)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="withHeader"></param>
        /// <param name="delimiter"></param>
        /// <param name="useQuotesForFields"></param>
        public static dynamic FromCsv(
            IEnumerable<string> content, 
            List<IDynamicTableColumn> elementDefinitions, 
            bool withHeader, 
            char delimiter, 
            bool useQuotesForFields)
        {
            dynamic element = new ExpandoObject();

            List<string> elementNames = null;
            List<string> lineValues = null;

            int index;
            IDynamicTableColumn elementDefinition;
            object convertedElement;

            //check input
            if (content == null)
            {
                return element;
            }

            CheckElementDefinitions(elementDefinitions);

            //loop over lines
            foreach (string line in content)
            {
                //ignore null, empty, whitespace
                if (string.IsNullOrEmpty(line) == true)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line) == true)
                {
                    continue;
                }

                //first row? then get column names
                if (elementNames == null)
                {
                    elementNames = Converter.CsvLineToHeaderElements(line, elementDefinitions, withHeader, delimiter);

                    //first row with header? then continue with the next line
                    if (withHeader == true)
                    {
                        continue;
                    }
                }

                //get line value - check if already a line was read
                if (lineValues == null)
                {
                    lineValues = Converter.CsvLineToValues(line, delimiter, useQuotesForFields);
                }
                else
                {
                    throw new FormatException("The file contains more than one line");
                }

                //compare element count with column count
                if (lineValues.Count != elementDefinitions.Count)
                {
                    throw new FormatException(string.Format("The number of elements in the line does not match with the number of element definitions. (line: '{0}'", line));
                }

                //add the elements and convert the parameters to the specific type
                IDictionary<string, object> elementDictionary = element;

                for (index = 0; index < elementNames.Count; index++)
                {
                    //get definitions
                    elementDefinition = GetElementDefinition(elementDefinitions, elementNames[index]);

                    if (elementDefinition == null)
                    {
                        throw new FormatException("The header element names do not match with the element definitions");
                    }

                    //convert value and add to row
                    if (elementDefinition.ValueType == typeof(DateTime))
                    {
                        convertedElement = DateTime.ParseExact(lineValues[index], "u", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        convertedElement = Convert.ChangeType(lineValues[index], elementDefinition.ValueType);
                    }

                    //add to row
                    elementDictionary.Add(elementNames[index], convertedElement);
                }
            }

            //return
            return element;
        }

        #endregion

        #region XML Export

        /// <summary>
        /// convert the object content into a string in xml format        
        /// </summary>
        /// <returns></returns>
        public static string AsXml(dynamic value)
        {
            XElement rootElement;
            XElement propertiesElement;

            //check input
            if (value == null)
            {
                return "";
            }

            //convert
            IDictionary<string, object> valueDictionary = value;

            if (valueDictionary.Count < 1)
            {
                return "";
            }
            
            //create root element
            rootElement = new XElement("DynamicObject");

            //create rows element
            propertiesElement = new XElement("Properties");
            rootElement.Add(propertiesElement);

            //content
            foreach (KeyValuePair<string, object> element in valueDictionary)
            {
                propertiesElement.Add(new XElement(element.Key, new XAttribute("value", Converter.ToXmlValue(element.Value))));                    
            }

            return rootElement.ToString();            
        }
        
        #endregion

        #region XML Import

        /// <summary>
        /// sets all values according to the xml file content
        /// 
        /// Exceptions
        ///     ArgumentException: input parameter is wrong (null, empty, missing)
        ///     FormatException: input parameter has wrong format
        ///     ArgumentNullException: no properties or property name is null
        ///     ArgumentException: invalid property name (empty name or name not unique)
        ///     XmlException: wrong format 
        /// </summary>
        /// <param name="content"></param>
        public static dynamic FromXml(IEnumerable<string> content, List<IDynamicTableColumn> elementDefinitions)
        {
            dynamic element = new ExpandoObject();

            string fileContent;

            XDocument document;
            XElement propertiesElement;        
            XAttribute propertyAttribute;

            List<string> propertyNames = null;
            List<string> propertyValues = null;

            int index;
            IDynamicTableColumn elementDefinition;
            object convertedElement;

            //check input
            if (content == null)
            {
                return element;
            }
            
            CheckElementDefinitions(elementDefinitions);

            //load document
            fileContent = string.Concat(content);

            if (string.IsNullOrEmpty(fileContent))
            {
                return element;
            }

            document = XDocument.Parse(fileContent);

            //get all properties
            var propertiesElements = (from property in document.Descendants("Properties")
                                select property).ToList();

            if (propertiesElements.Count == 0)
            {
                throw new FormatException("The 'Properties' tag was not found");
            }

            if (propertiesElements.Count > 1)
            {
                throw new FormatException("The 'Properties' tag was found multiple times");
            }

            //get values of property
            propertyNames = new List<string>();
            propertyValues = new List<string>();

            propertiesElement = propertiesElements[0];

            foreach (XElement propertyElement in propertiesElement.Elements())
            {
                propertyAttribute = propertyElement.Attribute("value");
                if (propertyAttribute == null)
                {
                    throw new FormatException("The 'value' attribute was not found");
                }

                propertyNames.Add(propertyElement.Name.ToString());
                propertyValues.Add(propertyAttribute.Value);
            }
            
            //compare element count with column count
            if (propertyValues.Count != elementDefinitions.Count)
            {
                throw new FormatException("The number of properties does not match with the number of element definitions");
            }

            //add the elements and convert the parameters to the specific type
            IDictionary<string, object> elementDictionary = element;

            for (index = 0; index < propertyNames.Count; index++)
            {
                //get definitions
                elementDefinition = GetElementDefinition(elementDefinitions, propertyNames[index]);

                if (elementDefinition == null)
                {
                    throw new FormatException("The properties names do not match with the element definitions");
                }

                //convert value and add to row
                if (elementDefinition.ValueType == typeof(DateTime))
                {
                    convertedElement = DateTime.ParseExact(propertyValues[index], "u", CultureInfo.InvariantCulture);
                }
                else
                {
                    convertedElement = Convert.ChangeType(propertyValues[index], elementDefinition.ValueType);
                }

                //add to row
                elementDictionary.Add(propertyNames[index], convertedElement);
            }
            
            //return
            return element;
        }

        #endregion

        #region internal helper - column definitions

        /// <summary>
        /// check whether the element defintions are valid
        /// 
        /// Exceptions
        ///     ArgumentNullException: no columns or column name is null
        ///     ArgumentException: invalid column name (empty name or name not unique)
        /// </summary>
        /// <param name="elementDefinitions"></param>
        private static void CheckElementDefinitions(List<IDynamicTableColumn> elementDefinitions)
        {
            List<string> names;

            //check if list is null
            if (elementDefinitions == null)
            {
                throw new ArgumentNullException("elementDefinitions");
            }

            //check if all names are valid and if a name is used twice
            names = new List<string>();

            foreach (IDynamicTableColumn element in elementDefinitions)
            {
                //valid name?
                if (element.Name == null)
                {
                    throw new ArgumentNullException("IDynamicTableColumn.Name");
                }

                if (string.IsNullOrEmpty(element.Name))
                {
                    throw new ArgumentException("Element name is empty");
                }

                if (string.IsNullOrWhiteSpace(element.Name))
                {
                    throw new ArgumentException("Element name contains only white space");
                }

                //already used
                if (names.Contains(element.Name) == true)
                {
                    throw new ArgumentException("Element name is not unique");
                }

                //add name
                names.Add(element.Name);
            }
        }

        /// <summary>
        /// get the element definition by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>column or 'null' if no column with thisname exist</returns>
        private static IDynamicTableColumn GetElementDefinition(List<IDynamicTableColumn> elementDefinitions, string name)
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                if (elementDefinitions != null)
                {
                    foreach (IDynamicTableColumn elementDefinition in elementDefinitions)
                    {
                        if (string.Equals(elementDefinition.Name, name, StringComparison.Ordinal) == true)
                        {
                            return elementDefinition;
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
