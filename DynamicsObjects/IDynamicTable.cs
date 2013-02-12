using System.Collections.Generic;

namespace OS.Toolbox.DinamicObjects
{
    public interface IDynamicTable
    {
        DynamicTableType TableType { get; }

        List<dynamic> Rows { get; }
        List<IDynamicTableColumn> Columns { get; }

        void AddRow(dynamic row);
        void AddRows(List<dynamic> rows);

        void PreDefineColumns(List<IDynamicTableColumn> columns);

        void RemoveAllRows();
        void ResetTable();

        string AsCsv();
        string AsCsv(bool withHeader, string delimiter, bool useQuotesForFields);
    }

    public enum DynamicTableType
    {
        Expandeable,
        DefineOnce,
        WellFormet
    }
}
