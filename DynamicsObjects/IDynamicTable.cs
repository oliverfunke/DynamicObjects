using System.Collections.Generic;
using System.Data;

namespace OS.Toolbox.DynamicObjects
{
    public interface IDynamicTable
    {
        DynamicTableType TableType { get; }

        List<dynamic> Rows { get; }
        List<IDynamicTableColumn> Columns { get; }

        IDynamicTableColumn GetColumn(string name);

        void AddRow(dynamic row);
        void AddRows(List<dynamic> rows);

        void PreDefineColumns(List<IDynamicTableColumn> columns);

        void RemoveAllRows();
        void ResetTable();

        string AsCsv();
        string AsCsv(bool withHeader, char delimiter, bool useQuotesForFields);

        void FromCsv(IEnumerable<string> content);
        void FromCsv(IEnumerable<string> content, bool withHeader, char delimiter, bool useQuotesForFields);

        string AsXml();
        void FromXml(IEnumerable<string> content);

        DataTable AsDataTable();
        void FromDataTable(DataTable data);
    }

    public enum DynamicTableType
    {        
        Expandable,
        DefineOnce,
        WellFormed
    }
}
