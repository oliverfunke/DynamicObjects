using NUnit.Framework;
using System.CodeDom.Compiler;
using OS.Toolbox.DynamicObjects;
using System.Dynamic;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;

namespace OS.Toolbox.DynamicObjectsUnitTest.ExpandoObjectSerializerUnitTest
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

        #region Export and Import CSV

        [Test]
        public void ExportAndImport_CSV_Comma_WithHeader_WithQuotes()
        {
            dynamic element;
            string csvExport;

            string fileName = _assemblyDirectory + @"\CsvTest.txt";

            //define elements
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age"),
                new DynamicTableColumn<DateTime>("TimeStamp")
            };

            //set values
            element = new ExpandoObject();
            element.FirstName = "Hans";
            element.LastName = "Mueller";
            element.Age = 30;
            element.TimeStamp = new DateTime(2012, 12, 24, 1, 2, 3);
            
            //export
            csvExport = ExpandoObjectSerializer.AsCsv(element, true, ',', true);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(csvExport);
            }

            //import
            using (StreamReader reader = new StreamReader(fileName))
            {
                element = ExpandoObjectSerializer.FromCsv(ReadFile(reader), elementDefinitions, true, ',', true);
            }

            //compare
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);

            Assert.AreEqual(2012, element.TimeStamp.Year);
            Assert.AreEqual(12, element.TimeStamp.Month);
            Assert.AreEqual(24, element.TimeStamp.Day);
            Assert.AreEqual(1, element.TimeStamp.Hour);
            Assert.AreEqual(2, element.TimeStamp.Minute);
            Assert.AreEqual(3, element.TimeStamp.Second);                       
        }

        [Test]
        public void ExportAndImport_CSV_Semicolon_WithoutHeader_WithoutQuotes()
        {
            dynamic element;
            string csvExport;

            string fileName = _assemblyDirectory + @"\CsvTest.txt";

            //define elements
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age"),
                new DynamicTableColumn<DateTime>("TimeStamp")
            };

            //set values
            element = new ExpandoObject();
            element.FirstName = "Hans";
            element.LastName = "Mueller";
            element.Age = 30;
            element.TimeStamp = new DateTime(2012, 12, 24, 1, 2, 3);

            //export
            csvExport = ExpandoObjectSerializer.AsCsv(element, false, ';', false);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(csvExport);
            }

            //import
            using (StreamReader reader = new StreamReader(fileName))
            {
                element = ExpandoObjectSerializer.FromCsv(ReadFile(reader), elementDefinitions, false, ';', false);
            }

            //compare
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);

            Assert.AreEqual(2012, element.TimeStamp.Year);
            Assert.AreEqual(12, element.TimeStamp.Month);
            Assert.AreEqual(24, element.TimeStamp.Day);
            Assert.AreEqual(1, element.TimeStamp.Hour);
            Assert.AreEqual(2, element.TimeStamp.Minute);
            Assert.AreEqual(3, element.TimeStamp.Second);  
        }

        #endregion

        #region Export and Import XML

        [Test]
        public void ExportAndImport_XML()
        {
            dynamic element;
            string xmlExport;

            string fileName = _assemblyDirectory + @"\XmlTest.xml";

            //define elements
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age"),
                new DynamicTableColumn<DateTime>("TimeStamp")
            };

            //set values
            element = new ExpandoObject();
            element.FirstName = "Hans";
            element.LastName = "Mueller";
            element.Age = 30;
            element.TimeStamp = new DateTime(2012, 12, 24, 1, 2, 3);

            //export
            xmlExport = ExpandoObjectSerializer.AsXml(element);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(xmlExport);
            }

            //import
            using (StreamReader reader = new StreamReader(fileName))
            {
                element = ExpandoObjectSerializer.FromXml(ReadFile(reader), elementDefinitions);
            }

            //compare
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);

            Assert.AreEqual(2012, element.TimeStamp.Year);
            Assert.AreEqual(12, element.TimeStamp.Month);
            Assert.AreEqual(24, element.TimeStamp.Day);
            Assert.AreEqual(1, element.TimeStamp.Hour);
            Assert.AreEqual(2, element.TimeStamp.Minute);
            Assert.AreEqual(3, element.TimeStamp.Second);
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
