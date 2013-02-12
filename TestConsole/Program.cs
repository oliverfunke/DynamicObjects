using OS.Toolbox.DinamicObjects;
using System;
using System.Dynamic;
using System.IO;

namespace OS.Toolbox.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
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
                writer.Write(table.AsCsv(true, ",", true));
            }

            Console.ReadKey();
        }
    }
}
