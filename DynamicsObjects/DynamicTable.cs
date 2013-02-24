using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Collections;
using System.Text;
using System.Linq;
using System.Globalization;

namespace OS.Toolbox.DynamicObjects
{
    public class DynamicTable : IDynamicTable
    {
        #region Constructor

        public DynamicTable()
        {
            _tableType = DynamicTableType.Expandeable;
        }

        public DynamicTable(DynamicTableType tableType)
        {
            _tableType = tableType;
        }

        #endregion

        #region Rows and Columns

        /// <summary>
        /// get the table type     
        /// </summary>
        public DynamicTableType TableType
        {
            get
            {
                return _tableType;
            }            
        }

        /// <summary>
        /// get all columns
        /// </summary>
        public List<IDynamicTableColumn> Columns
        {
            get
            {
                return _columns;
            }
        }

        /// <summary>
        /// get the column by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>column or 'null' if no column with thisname exist</returns>
        public IDynamicTableColumn GetColumn(string name)
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                if (_columns != null)
                {
                    foreach (IDynamicTableColumn column in _columns)
                    {
                        if (string.Equals(column.Name, name, StringComparison.Ordinal) == true)
                        {
                            return column;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// get all rows
        /// </summary>
        public List<dynamic> Rows
        {
            get
            {
                return _rows;
            }
        }

        #endregion

        #region Define columns

        /// <summary>
        /// pre define columns
        /// this function is optional
        /// if the columns are not predefined then the columns will be created according to content of the first added row
        /// 
        /// Exceptions
        ///     ArgumentNullException: columns parameter is 'null' or column name is 'null'
        ///     ArgumentException: column list is empty or column name is empty or whitespace or column name is not unique
        ///     NotSupportedException: the table already contains data
        /// </summary>
        /// <param name="columns"></param>
        public void PreDefineColumns(List<IDynamicTableColumn> columns)
        {
            //check if the table is already in use
            if(_columns != null)
            {
                throw new NotSupportedException();
            }

            //check input
            if(columns == null)
            {
                throw new ArgumentNullException("columns");                            
            }

            if (columns.Count == 0)
            {
                throw new ArgumentException("Column list is empty");
            }           

            //add columns
            AddColumns(columns);

            //reset rows
            _rows = new List<dynamic>();
        }

        /// <summary>
        /// creates the columns by using the elements of the first row
        /// </summary>
        /// <param name="row"></param>
        private void CreateColumnsByFirstRow(dynamic row)
        {
            List<IDynamicTableColumn> columns;

            columns = new List<IDynamicTableColumn>();

            //loop over properties
            foreach (var property in (IDictionary<String, Object>)row)
            {
                Type propertyType = property.Value.GetType();
                Type tableColumn = typeof(DynamicTableColumn<>).MakeGenericType(propertyType);
                object column = Activator.CreateInstance(tableColumn, property.Key);

                columns.Add((IDynamicTableColumn)column);
            }       
     
            //create table columns
            AddColumns(columns);
        }      

        /// <summary>
        /// expands the columns by adding all new elements
        /// </summary>
        /// <param name="row"></param>
        private void ExpandColumnsWithNewElements(dynamic row)
        {
            List<IDynamicTableColumn> newColumns;
            List<string> actualColumnNames;

            //get all actual column names
            actualColumnNames = new List<string>();

            if (_columns != null)
            {
                foreach (IDynamicTableColumn column in _columns)
                {
                    actualColumnNames.Add(column.Name);
                }
            }

            //loop over properties
            newColumns = new List<IDynamicTableColumn>();

            foreach (var property in (IDictionary<String, Object>)row)
            {
                //new column found
                if (actualColumnNames.Contains(property.Key) == false)
                {
                    //add column
                    if (_tableType == DynamicTableType.Expandeable)
                    {
                        Type propertyType = property.Value.GetType();
                        Type tableColumn = typeof(DynamicTableColumn<>).MakeGenericType(propertyType);
                        object column = Activator.CreateInstance(tableColumn, property.Key);

                        newColumns.Add((IDynamicTableColumn)column);
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("Row contains the additional column {0}", property.Key));
                    }
                }                
            }

            //create table columns
            AddColumns(newColumns);

            //expand all existing rows
            if (_rows != null)
            {
                foreach (dynamic element in _rows)
                {
                    IDictionary<string, object> rowDictionary = element;

                    foreach (IDynamicTableColumn column in newColumns)
                    {
                        if (rowDictionary.ContainsKey(column.Name) == false)
                        {
                            // - add element
                            rowDictionary.Add(column.Name, column.DefaultValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// add columns
        /// </summary>
        private void AddColumns(List<IDynamicTableColumn> columns)
        {
            List<string> names;

            //check if all names are valid and if a name is used twice
            names = new List<string>();

            if (_columns != null)
            {
                foreach (IDynamicTableColumn column in _columns)
                {
                    names.Add(column.Name);
                }
            }

            foreach (IDynamicTableColumn column in columns)
            {
                //valid name?
                if (column.Name == null)
                {
                    throw new ArgumentNullException("IDynamicTableColumn.Name");
                }

                if (string.IsNullOrEmpty(column.Name))
                {
                    throw new ArgumentException("Column name is empty");
                }

                if (string.IsNullOrWhiteSpace(column.Name))
                {
                    throw new ArgumentException("Column name contains only white space");
                }

                //already uses
                if (names.Contains(column.Name) == true)
                {
                    throw new ArgumentException("Column name is not unique");
                }

                //add name
                names.Add(column.Name);
            }

            //add columns
            if (_columns == null)
            {
                _columns = new List<IDynamicTableColumn>();
            }
            
            _columns.AddRange(columns);            
        }
        
        #endregion

        #region Add rows

        /// <summary>
        /// add a row to the table
        /// 
        /// if the table does not have pre defined columns, then the first added row is used to initalize the table
        /// 
        /// if 'TableType' is 'Expandeable': 
        /// - new columns will be added if the row contains new elements
        /// - already available column types must match element types
        /// - the row may contain only a part of the elements of the defined columns
        /// - for all other elements the default values will be used
        /// 
        /// if 'TableType' is 'DefineOnce': 
        /// - elements of first row are used to create columns, except the columns are predefined
        /// - available column types must match element types
        /// - the row may contain only a part of the elements of the defined columns
        /// - for all other elements the default values will be used
        /// 
        /// if 'TableType' is 'WellFormet': 
        /// - elements of first row are used to create columns, except the columns are predefined
        /// - available column types must match element types
        /// - the row elements must match with the defined columns
        /// 
        /// Eceptions
        ///     see 'PreDefineColumns' for exceptions in case the columns will be created by using the first row
        ///     ArgumentException: well formed data is forced but row does not contain the expected elements, row type does not macht with element type
        /// </summary>
        /// <param name="row"></param>
        public void AddRow(dynamic row)
        {
            dynamic newRow;

            //check if element is null
            if (row == null)
            {
                return;
            }

            //no predefined columns, then use element of first row
            if (_columns == null)
            {
                CreateColumnsByFirstRow(row);
            }
            //expand columns (will throw an error if table is not expandeable)
            else 
            {
                ExpandColumnsWithNewElements(row);
            }

            //add row
            // - a new element will be created
            newRow = new ExpandoObject();

            // - loop over all columns
            IDictionary<string, object> rowDictionary = row;
            IDictionary<string, object> newRowDictionary = newRow;

            foreach (IDynamicTableColumn column in _columns)
            {                
                //check if columns is found in element
                if (rowDictionary.ContainsKey(column.Name) == true)
                {
                    // - check type
                    if (rowDictionary[column.Name].GetType() != column.ValueType)
                    {
                        throw new ArgumentException(String.Format("Row type does not match with column type of column {0}", column.Name));                    
                    }

                    // - add
                    newRowDictionary.Add(column.Name, rowDictionary[column.Name]);
                }
                else
                {
                    // - well formet, then throw an error
                    if(_tableType == DynamicTableType.WellFormet)                       
                    {
                        throw new ArgumentException(String.Format("Row does not contain the column {0}", column.Name));
                    }
                    // - add element
                    else
                    {
                        newRowDictionary.Add(column.Name, column.DefaultValue);
                    }
                }
            }

            //add row
            if (_rows == null)
            {
                _rows = new List<dynamic>();
            }

            _rows.Add(newRow); 
        }

        /// <summary>
        /// adds rows to the table
        /// if one of the rows contains erroneous elements, then no row will be added
        /// 
        /// Excptions
        ///     see 'AddRow' for excptions
        /// </summary>
        /// <param name="rows"></param>
        public void AddRows(List<dynamic> rows)
        {
            Nullable<int> actualNumberOfRows = null;

            //check if list is null
            if (rows == null)
            {
                return;
            }

            //store actual number of rows for a rollback
            if (_rows != null)
            {
                actualNumberOfRows = _rows.Count;
            }
            
            //add all elements
            try
            {
                foreach (dynamic row in rows)
                {
                    AddRow(row);
                }
            }
            catch(Exception)
            {
                //roolback
                if (actualNumberOfRows != null)
                {
                    _rows.RemoveRange(actualNumberOfRows.Value - 1, _rows.Count - actualNumberOfRows.Value);
                }
                else
                {
                    _rows = null;
                }

                //rethrow error
                throw;
            }
        }

        #endregion

        #region Clear

        /// <summary>
        /// removes all rows
        /// </summary>
        public void RemoveAllRows()
        {
            if (_rows != null)
            {
                _rows = new List<dynamic>();
            }
        }

        /// <summary>
        /// resets the table by removing all rows and columns
        /// </summary>
        public void ResetTable()
        {
            _rows = null;
            _columns = null;
        }

        #endregion

        #region CSV Export

        /// <summary>
        /// convert the content of the table into a string in csv format
        /// - no header
        /// - delimiter: comma
        /// - field quotes: quotes will be used
        /// </summary>
        /// <returns></returns>
        public string AsCsv()
        {
            return AsCsv(false, ',', true);
        }

        /// <summary>
        /// convert the objet list into a string in csv format        
        /// </summary>
        /// <param name="withHeader"></param>
        /// <param name="delimiter"></param>
        /// <param name="useQuotesForFields"></param>
        /// <returns></returns>
        public string AsCsv(bool withHeader, char delimiter, bool useQuotesForFields)
        {
            var csvBuilder = new StringBuilder();
            string[] values;
            int index;

            //table exists?
            if (_columns == null)
            {
                return "";
            }

            //header
            if (withHeader == true)
            {
                csvBuilder.AppendLine(string.Join(new string(delimiter,1),
                    (from c in _columns select c.Name).ToArray()));
            }

            //content
            if (_rows != null)
            {
                values = new string[_columns.Count];
                
                foreach (dynamic row in _rows)
                {
                    IDictionary<string, object> rowDictionary = row;

                    index = 0;
                    foreach(IDynamicTableColumn colum in _columns)
                    {
                        values[index] = ToCsvValue(rowDictionary[colum.Name], useQuotesForFields);
                        index++;
                    }

                    csvBuilder.AppendLine(string.Join(new string(delimiter, 1), values.ToArray()));
                }
            }           
 
            return csvBuilder.ToString();
        }

        /// <summary>
        /// get csv value for this element
        /// </summary>
        /// <param name="item"></param>
        /// <param name="useQuotesForFields"></param>
        /// <returns></returns>
        private static string ToCsvValue(object item, bool useQuotesForFields)
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
        public void FromCsv(IEnumerable<string> content)
        {
            FromCsv(content, false, ',', true); 
        }

        /// <summary>
        /// sets all values according to the csv file content
        /// 
        /// Exceptions
        ///     NotSupportedException: the table already contains data, no columns defined
        ///     ArgumentException: input parameter is wrong (null, empty, missing)
        ///     FormatException: input parameter has wrong format
        /// </summary>
        /// <param name="content"></param>
        /// <param name="withHeader"></param>
        /// <param name="delimiter"></param>
        /// <param name="useQuotesForFields"></param>
        public void FromCsv(IEnumerable<string> content, bool withHeader, char delimiter, bool useQuotesForFields)
        {
            bool rollbackNeeded = true;

            List<string> columnNames = null;
            List<string> lineValues = null;

            dynamic newRow;
            IDictionary<string, object> newRowDictionary;
            int index;

            IDynamicTableColumn column;
            object convertedElement;

            //check status: already rows available
            if (_rows != null)
            {
                if (_rows.Count > 0)
                {
                    throw new NotSupportedException("The table already contains rows");
                }
            }

            //check status: columns defined
            if (_columns == null)
            {
                throw new NotSupportedException("The table contains no column definitions");
            }

            if (_columns.Count == 0)
            {
                throw new NotSupportedException("The table contains no column definitions");
            }

            //check input
            if (content == null)
            {
                return;
            }
           
            //if an error occurs all rows should be removed, therefore use a try finally block for the whole method
            try
            {
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
                    if (columnNames == null)
                    {
                        columnNames = GetColumnNamesFromCsv(line, withHeader, delimiter);

                        //first row with header? then continue with the next line
                        if (withHeader == true)
                        {
                            continue;
                        }
                    }                    

                    //split 
                    lineValues = GetLineValuesFromCsv(line, delimiter, useQuotesForFields);

                    //compare element count with column count
                    if (lineValues.Count != _columns.Count)
                    {
                        throw new FormatException(string.Format("The number of elements in the line does not match with the number of columns. (line: '{0}'", line));
                    }
                    
                    //create the row and try to convert the parameters to the specific type
                    newRow = new ExpandoObject();
                    newRowDictionary = newRow;

                    for(index = 0; index < columnNames.Count; index++)
                    {
                        //get column
                        column = GetColumn(columnNames[index]);

                        //convert value and add to row
                        if (column.ValueType == typeof(DateTime))
                        {
                            convertedElement = DateTime.ParseExact(lineValues[index], "u", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            convertedElement = Convert.ChangeType(lineValues[index], column.ValueType);
                        }                                                 

                        //add to row
                        newRowDictionary.Add(columnNames[index], convertedElement);                        
                    }

                    // - add row to list
                    AddRow(newRow);                    
                }

                //finish successful
                rollbackNeeded = false;
            }
            finally
            {
                //rollback
                if (rollbackNeeded == true)
                {
                    RemoveAllRows();
                }
            }            
        }

        /// <summary>
        /// get the column names
        /// 
        /// check if the file contains a header? 
        /// - yes: import the header and use the names for the parameters + check if the names are unique and equal to the defined column names
        /// - no: use the defined column names
        /// </summary>
        /// <param name="firstLine"></param>
        /// <param name="withHeader"></param>
        /// <returns></returns>
        private List<string> GetColumnNamesFromCsv(string firstLine, bool withHeader, char delimiter)
        {
            List<string> columnNames = new List<string>();

            //without header, then use defined names
            if (withHeader == false)
            {
                foreach (IDynamicTableColumn column in _columns)
                {
                    columnNames.Add(column.Name);                 
                }

                return columnNames;
            }

            //with header, then extract names
            columnNames.AddRange(firstLine.Split(new char[] { delimiter }, StringSplitOptions.None));

            //check if the names are unique and equal to the defined column names
            if (_columns.Count != columnNames.Count)
            {
                throw new FormatException("The number of defined columns does not match with the number of columns found in the file");
            }

            foreach (IDynamicTableColumn column in _columns)
            {
                if (columnNames.Contains(column.Name) == false)
                {
                    throw new FormatException(string.Format("The defined column {0} does not exist in the file", column.Name));
                }
            }

            //return
            return columnNames;
        }

        /// <summary>
        /// split values
        /// </summary>
        /// <param name="line"></param>
        /// <param name="delimiter"></param>
        /// <param name="useQuotes"></param>
        /// <returns></returns>
        private List<string> GetLineValuesFromCsv(string line, char delimiter, bool useQuotes)
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

        #endregion
        
        #region Member

        private List<IDynamicTableColumn> _columns;
        private List<dynamic> _rows;
        private DynamicTableType _tableType;

        #endregion
    }
}

