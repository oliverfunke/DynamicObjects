using OS.Toolbox.DynamicObjects;
using System;
using System.Dynamic;
using System.IO;
using OS.Toolbox.DynamicObjectsUnitTest.DynamicTableUnitTest;

namespace OS.Toolbox.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);            
            dynamic row;

            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);
            
            using (StreamWriter writer = new StreamWriter("test1.csv"))
            {
                writer.Write(table.AsCsv(true, ',', true));
            }
            */

            UseCasesUnitTest test = new UseCasesUnitTest();
            test.SetUp();
            test.ExportAndImport_CSV_Semicolon_WithoutHeader_WithoutQuotes();

            Console.ReadKey();
        }
    }
}
