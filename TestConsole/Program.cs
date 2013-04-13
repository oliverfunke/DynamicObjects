using OS.Toolbox.DynamicObjects;
using System;
using System.Dynamic;
using System.IO;
using OS.Toolbox.DynamicObjectsUnitTest.DynamicTableUnitTest;
using System.Collections.Generic;

namespace OS.Toolbox.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic element = new ExpandoObject();
            element.FirstName = "John";
            element.LastName = "Doe";
            element.Age = 30;

            using (StreamWriter writer = new StreamWriter("test.csv"))
            {
                writer.Write(ExpandoObjectSerializer.AsCsv(element));
            }

            ImportElementCsvFile();




            
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);            
            dynamic row;

            row = new ExpandoObject();
            row.FirstName = "John";
            row.LastName = "Doe";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.FirstName = "Jane";
            row.LastName = "Doe";
            row.Street = "Main street";
            table.AddRow(row);


            foreach (dynamic actualRow in table.Rows)
            {
                Console.WriteLine(
                    string.Format("{0} {1} is {2} years old.",
                        actualRow.FirstName,
                        actualRow.LastName,
                        actualRow.Age));
            }


            using (StreamWriter writer = new StreamWriter("test.csv"))
            {
                writer.Write(table.AsCsv());
            }

            using (StreamWriter writer = new StreamWriter("test.xml"))
            {
                writer.Write(table.AsXml());
            }

            ImportTableCsvFile();            

            Console.ReadKey();
        }

        private static void ImportTableCsvFile()
        {            
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            //pre define columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<string>("Street")
                });

            //import
            using (StreamReader reader = new StreamReader("test.csv"))
            {
                table.FromCsv(ReadFile(reader));
            }

            foreach (dynamic actualRow in table.Rows)
            {
                Console.WriteLine(
                    string.Format("{0} {1} is {2} years old.",
                        actualRow.FirstName,
                        actualRow.LastName,
                        actualRow.Age));
            }
        }

        private static void ImportElementCsvFile()
        {
            dynamic element;
 
            //define elements
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age")
            };

            //import
            using (StreamReader reader = new StreamReader("test.csv"))
            {
                element = ExpandoObjectSerializer.FromCsv(ReadFile(reader), elementDefinitions);
            }

            Console.WriteLine(
                string.Format("{0} {1} is {2} years old.",
                        element.FirstName,
                        element.LastName,
                        element.Age));
        }

        private static IEnumerable<string> ReadFile(StreamReader reader)
        {
            while (reader.EndOfStream == false)
            {
                yield return reader.ReadLine();
            }
        }
    }
}
