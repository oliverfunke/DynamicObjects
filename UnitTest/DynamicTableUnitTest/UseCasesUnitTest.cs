using NUnit.Framework;
using System.CodeDom.Compiler;
using OS.Toolbox.DynamicObjects;
using System.Dynamic;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Data;

namespace OS.Toolbox.DynamicObjectsUnitTest.DynamicTableUnitTest
{
    [GeneratedCodeAttribute("UnitTest", "")] //NOTE: this tag is added to provide fx cop from analyzing this class
    [TestFixture]
    public class UseCasesUnitTest
    {
        #region Init and Clean Up

        [SetUp]
        public void SetUp()
        {
            //get directory of assembly
            _assemblyDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(UseCasesUnitTest)).CodeBase);
            if (_assemblyDirectory.StartsWith("file") == true)
            {
                _assemblyDirectory = _assemblyDirectory.Substring(6);
            }
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion 

        #region UseTable

        [Test]
        public void UseTable_WithoutColumnDefinition_Expandeable()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(4, table.Columns.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual(null, row.Street);

            row = table.Rows[1];
            Assert.AreEqual(null, row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void UseTable_WithoutColumnDefinition_DefineOnce()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.DefineOnce);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.FirstName = "Sarah";
            row.LastName = "Meier";            
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(3, table.Columns.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);            

            row = table.Rows[1];
            Assert.AreEqual("Sarah", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);            
        }

        [Test]
        public void UseTable_WithoutColumnDefinition_WellFormet()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormed);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.FirstName = "Sarah";
            row.LastName = "Meier";
            row.Age = 22;
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(3, table.Columns.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);

            row = table.Rows[1];
            Assert.AreEqual("Sarah", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(22, row.Age);
        }

        [Test]
        public void UseTable_PreDefinedColumns_Expandeable()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            //set columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName", ""),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(4, table.Columns.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual(null, row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(-1, row.Age);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void UseTable_PreDefinedColumns_DefineOnce()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.DefineOnce);
            dynamic row;

            //set columns         
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age")
                });

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.FirstName = "Sarah";
            row.LastName = "Meier";
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(3, table.Columns.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);

            row = table.Rows[1];
            Assert.AreEqual("Sarah", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
        }

        [Test]
        public void UseTable_PreDefinedColumns_WellFormet()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormed);
            dynamic row;

            //set columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", 100)
                });

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.FirstName = "Sarah";
            row.LastName = "Meier";
            row.Age = 50;
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(3, table.Columns.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);

            row = table.Rows[1];
            Assert.AreEqual("Sarah", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(50, row.Age);
        }

        #endregion

        #region Export and Import CSV

        [Test]
        public void ExportAndImport_CSV_Comma_WithHeader_WithQuotes()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;
            string csvExport;

            string fileName = _assemblyDirectory + @"\CsvTest.txt";

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            row.TimeStamp = new DateTime(2012, 12, 24, 1, 2, 3);
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //export
            csvExport = table.AsCsv(true, ',', true);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(csvExport);
            }

            //remove rows
            table.RemoveAllRows();
            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //import
            using (StreamReader reader = new StreamReader(fileName))
            {
                table.FromCsv(ReadFile(reader), true, ',', true);
            }

            //compare
            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);

            Assert.AreEqual(2012, row.TimeStamp.Year);
            Assert.AreEqual(12, row.TimeStamp.Month);
            Assert.AreEqual(24, row.TimeStamp.Day);
            Assert.AreEqual(1, row.TimeStamp.Hour);
            Assert.AreEqual(2, row.TimeStamp.Minute);
            Assert.AreEqual(3, row.TimeStamp.Second);
            
            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(0, row.TimeStamp.Ticks);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void ExportAndImport_CSV_Semicolon_WithoutHeader_WithoutQuotes()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;
            string csvExport;

            string fileName = _assemblyDirectory + @"\CsvTest.txt";

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            row.TimeStamp = new DateTime(2012, 12, 24, 1, 2, 3);
            table.AddRow(row);            

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //export
            csvExport = table.AsCsv(false, ';', false);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(csvExport);
            }

            //remove rows
            table.RemoveAllRows();
            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //import
            using (StreamReader reader = new StreamReader(fileName))
            {
                table.FromCsv(ReadFile(reader), false, ';', false);
            }

            //compare
            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);

            Assert.AreEqual(2012, row.TimeStamp.Year);
            Assert.AreEqual(12, row.TimeStamp.Month);
            Assert.AreEqual(24, row.TimeStamp.Day);
            Assert.AreEqual(1, row.TimeStamp.Hour);
            Assert.AreEqual(2, row.TimeStamp.Minute);
            Assert.AreEqual(3, row.TimeStamp.Second);

            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(0, row.TimeStamp.Ticks);
            Assert.AreEqual("Main street", row.Street);
        }

        #endregion

        #region Export and Import XML

        [Test]
        public void ExportAndImport_XML()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;
            string xmlExport;

            string fileName = _assemblyDirectory + @"\CsvTest.xml";

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            row.TimeStamp = new DateTime(2012, 12, 24, 1, 2, 3);
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //export
            xmlExport = table.AsXml();

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(xmlExport);
            }

            //remove rows
            table.RemoveAllRows();
            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //import
            using (StreamReader reader = new StreamReader(fileName))
            {
                table.FromXml(ReadFile(reader));
            }

            //compare
            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);

            Assert.AreEqual(2012, row.TimeStamp.Year);
            Assert.AreEqual(12, row.TimeStamp.Month);
            Assert.AreEqual(24, row.TimeStamp.Day);
            Assert.AreEqual(1, row.TimeStamp.Hour);
            Assert.AreEqual(2, row.TimeStamp.Minute);
            Assert.AreEqual(3, row.TimeStamp.Second);

            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(0, row.TimeStamp.Ticks);
            Assert.AreEqual("Main street", row.Street);
        }

        #endregion

        #region Export and Import DataTable

        [Test]
        public void ExportAndImport_DataTable()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;
            DataTable data;
            
            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            row.TimeStamp = new DateTime(2012, 12, 24, 1, 2, 3);
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);

            //compare
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //export
            data = table.AsDataTable();
            
            //remove rows
            table.RemoveAllRows();
            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(5, table.Columns.Count);

            //import
            table.FromDataTable(data);

            //compare
            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);

            Assert.AreEqual(2012, row.TimeStamp.Year);
            Assert.AreEqual(12, row.TimeStamp.Month);
            Assert.AreEqual(24, row.TimeStamp.Day);
            Assert.AreEqual(1, row.TimeStamp.Hour);
            Assert.AreEqual(2, row.TimeStamp.Minute);
            Assert.AreEqual(3, row.TimeStamp.Second);

            Assert.AreEqual(null, row.Street);

            row = table.Rows[1];
            Assert.AreEqual(null, row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(0, row.TimeStamp.Ticks);
            Assert.AreEqual("Main street", row.Street);
        }

        #endregion

        #region Internal

        private static IEnumerable<string> ReadFile(StreamReader reader)
        {
            while (reader.EndOfStream == false)
            {
                yield return reader.ReadLine();
            }
        }
        
        #endregion

        #region Member

        private string _assemblyDirectory;

        #endregion
    }
}
