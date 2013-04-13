using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS.Toolbox.DynamicObjects
{
    internal static class Converter
    {
        #region CSV

        /// <summary>
        /// get csv value for this element
        /// </summary>
        /// <param name="item"></param>
        /// <param name="useQuotesForFields"></param>
        /// <returns></returns>
        internal static string ToCsvValue(object item, bool useQuotesForFields)
        {
            //is null?            
            if (item == null)
            {
                if (useQuotesForFields == true)
                {
                    return "\"\"";
                }
                else
                {
                    return "";
                }
            }

            //is string?
            if (item is string)
            {
                if (useQuotesForFields == true)
                {
                    return string.Format("\"{0}\"", item.ToString().Replace("\"", "\"\""));
                }
                else
                {
                    return item.ToString();
                }
            }

            //is datetime?
            if (item is DateTime)
            {
                return string.Format("{0:u}", item);    //format: 2013-01-20 12:49:56Z
            }

            //is a number?
            double dummy;
            if (double.TryParse(item.ToString(), out dummy))
                return string.Format("{0}", item);

            //standard value
            return string.Format("{0}", item);
        }

        /// <summary>
        /// split the csv line content to values
        /// </summary>
        /// <param name="line"></param>
        /// <param name="delimiter"></param>
        /// <param name="useQuotes"></param>
        /// <returns></returns>
        internal static List<string> CsvLineToValues(string line, char delimiter, bool useQuotes)
        {
            List<string> lineValues = new List<string>();

            bool elementStarted = false;
            bool previouslyValueInsideElementWasQuote = false;
            StringBuilder partialElement = new StringBuilder();

            //go over all chars
            // - single quote? then start or finish element
            // - double quote? convert to single quote and use it as normal char
            // - separator: use this a splitter, except if it is inside a element
            foreach (char value in line.ToArray())
            {
                //quote found at previously element? (and we are inside an element)
                // - single quote, then ignore quote and close element
                // - double quote, then add single quote
                if (previouslyValueInsideElementWasQuote == true)
                {
                    //reset
                    previouslyValueInsideElementWasQuote = false;

                    //actual value is quote? then we have a double quote
                    if (value == '\"')
                    {
                        partialElement.Append('\"');
                        continue;
                    }
                    //no quote, then the previously was an end quote for the element
                    else
                    {
                        elementStarted = false;
                    }
                }

                //separator found? then add temp element to list
                if ((value == delimiter) && (elementStarted == false))
                {
                    lineValues.Add(partialElement.ToString());

                    partialElement.Clear();
                    continue;
                }

                //quote found and actually we are not inside an element, then ignore quote and get ino the element
                if ((value == '\"') && (elementStarted == false))
                {
                    elementStarted = true;
                    continue;
                }

                //quote found and actually we are inside an element
                // - remember that a quote was found because next value is importand to see whether it was a single or double quote
                if ((value == '\"') && (elementStarted == true))
                {
                    previouslyValueInsideElementWasQuote = true;
                    continue;
                }

                //add value to temp element
                partialElement.Append(value);
            }

            //add last element to list            
            lineValues.Add(partialElement.ToString());

            //return
            return lineValues;
        }

        /// <summary>
        /// get the header element names
        /// 
        /// check if the file contains a header? 
        /// - yes: import the header and use the names for the parameters + check if the names are unique and equal to the defined column names
        /// - no: use the defined element names
        /// </summary>
        /// <param name="firstLine"></param>
        /// <param name="withHeader"></param>
        /// <returns></returns>
        internal static List<string> CsvLineToHeaderElements(
            string firstLine,
            List<IDynamicTableColumn> elementDefinitions,
            bool withHeader,
            char delimiter)
        {
            List<string> elementNames = new List<string>();

            //without header, then use defined names
            if (withHeader == false)
            {
                foreach (IDynamicTableColumn element in elementDefinitions)
                {
                    elementNames.Add(element.Name);
                }

                return elementNames;
            }

            //with header, then extract names
            elementNames.AddRange(firstLine.Split(new char[] { delimiter }, StringSplitOptions.None));

            //check if the names are unique and equal to the defined element names
            if (elementDefinitions.Count != elementNames.Count)
            {
                throw new FormatException("The number of defined elements does not match with the number of elements found in the file");
            }

            foreach (IDynamicTableColumn element in elementDefinitions)
            {
                if (elementNames.Contains(element.Name) == false)
                {
                    throw new FormatException(string.Format("The defined element {0} does not exist in the file", element.Name));
                }
            }

            //return
            return elementNames;
        }

        #endregion

        #region XML

        /// <summary>
        /// get the xml value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal static string ToXmlValue(object item)
        {
            //is null?            
            if (item == null)
            {
                return "";
            }

            //is string?
            if (item is string)
            {
                return item.ToString();
            }

            //is datetime?
            if (item is DateTime)
            {
                return string.Format("{0:u}", item);    //format: 2013-01-20 12:49:56Z
            }

            //is a number?
            double dummy;
            if (double.TryParse(item.ToString(), out dummy))
                return string.Format("{0}", item);

            //standard value
            return string.Format("{0}", item);
        }

        #endregion
    }
}
