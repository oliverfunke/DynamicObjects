using NUnit.Framework;
using System.CodeDom.Compiler;
using OS.Toolbox.DynamicObjects;
using System.Dynamic;
using System.Collections.Generic;
using System;
using System.Xml;

namespace OS.Toolbox.DynamicObjectsUnitTest.ExpandoObjectSerializerUnitTest
{
    [GeneratedCodeAttribute("UnitTest", "")] //NOTE: this tag is added to provide fx cop from analyzing this class
    [TestFixture]
    public class FunctionsUnitTest
    {
        #region AsCsv

        [Test]
        public void AsCsv_Standard()
        {
            dynamic element;
            string csvContent;
            string expectedContent;
            
            //set values
            element = new ExpandoObject();
            element.FirstName = "Hans";
            element.LastName = "Mueller";
            element.Age = 30;
            element.Street = "";
           
            //get csv
            csvContent = ExpandoObjectSerializer.AsCsv(element);

            //compare
            expectedContent = "\"Hans\",\"Mueller\",30,\"\"" + Environment.NewLine;

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_Comma_WithHeaders_WithQuotes()
        {
            dynamic element;
            string csvContent;
            string expectedContent;

            //add values
            element = new ExpandoObject();
            element.FirstName = "Hans";
            element.LastName = "Mu\"eller";
            element.Age = 30;
            element.Street = "";

            //get csv
            csvContent = ExpandoObjectSerializer.AsCsv(element, true, ',', true);

            //compare
            expectedContent = "FirstName,LastName,Age,Street" + Environment.NewLine +
                 "\"Hans\",\"Mu\"\"eller\",30,\"\"" + Environment.NewLine;

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_Semicolon_WithoutHeaders_WithoutQuotes()
        {
            dynamic element;
            string csvContent;
            string expectedContent;

            //add values
            element = new ExpandoObject();
            element.FirstName = "Hans";
            element.LastName = "Mueller";
            element.Age = 30;
            element.Street = "";

            //get csv
            csvContent = ExpandoObjectSerializer.AsCsv(element, false, ';', false);

            //compare
            expectedContent = "Hans;Mueller;30;" + Environment.NewLine;

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_Empty_WithHeaders()
        {
            dynamic element;
            string csvContent;
            string expectedContent;

            //add values
            element = new ExpandoObject();

            //get csv
            csvContent = ExpandoObjectSerializer.AsCsv(element);

            //compare
            expectedContent = "";

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_Empty_WithoutHeaders()
        {
            dynamic element;
            string csvContent;
            string expectedContent;

            //add values
            element = new ExpandoObject();

            //get csv
            csvContent = ExpandoObjectSerializer.AsCsv(element);

            //compare         
            expectedContent = "";

            Assert.AreEqual(expectedContent, csvContent);
        }

        #endregion

        #region FromCsv

        [Test]
        public void FromCsv_Standard()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",30,\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions);

            //compare  
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }

        [Test]
        public void FromCsv_Comma_WithHeaders_WithQuotes()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "FirstName,LastName,Age,Street",
                 "\"Hans\",\"Mu\"\"eller\",30,\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions, true, ',', true);

            //compare  
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mu\"eller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }

        [Test]
        public void FromCsv_Semicolon_WithoutHeaders_WithoutQuotes()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "Hans;Mueller;30;"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions, false, ';', false);

            //compare  
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }

        [Test]
        public void FromCsv_WithHeaders_OtherSortOrderInFile()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "Street,FirstName,Age,LastName",
                 ",Hans,30,Mueller"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions, true, ',', false);

            //compare     
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }

        [Test]
        public void FromCsv_SeparatorInField()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "FirstName,LastName,Age,Street",
                 "\"Ha,ns\",\"Mu,\"\",\"\",\"\"\"\",eller\",30,\"\"\"\"\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions, true, ',', true);

            //compare    
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Ha,ns", element.FirstName);
            Assert.AreEqual("Mu,\",\",\"\",eller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("\"\"", element.Street);
        }

        [Test]
        public void FromCsv_Empty_Content()
        {
            dynamic element;

            List<string> csvContent = new List<string>();

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions);

            //compare    
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(0, elementDictionary.Count);
        }

        [Test]
        public void FromCsv_Empty_Defintions()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "FirstName,LastName,Age,Street",
                 "\"Ha,ns\",\"Mu,\"\",\"\",\"\"\"\",eller\",30,\"\"\"\"\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>();

            //import
            try
            {
                element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromCsv_Null_Content()
        {
            dynamic element;

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(null, elementDefinitions);

            //compare    
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(0, elementDictionary.Count);
        }

        [Test]
        public void FromCsv_Null_Definitions()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "FirstName,LastName,Age,Street",
                 "\"Ha,ns\",\"Mu,\"\",\"\",\"\"\"\",eller\",30,\"\"\"\"\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromCsv(csvContent, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [Test]
        public void FromCsv_EmptyLineOrWhitespace()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                null,
                "",
                "\"Hans\",\"Mueller\",30,\"\"",
                "  "
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions);

            //compare    
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }

        [Test]
        public void FromCsv_WrongNumberOfElements()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",30"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromCsv_WrongType()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",a,\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromCsv_NotUniqueColumnNames()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "FirstName,FirstName,Age,Street",
                "\"Hans\",\"Mueller\",30,\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions, true, ',', true);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromCsv_WrongColumnNames()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "FirstName,abcde,Age,Street",
                "\"Hans\",\"Mueller\",30,\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions, true, ',', true);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromCsv_WrongLineCount()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "FirstName,LastName,Age,Street",
                 "\"Hans\",\"Mu\"\"eller\",30,\"\"",
                 "\"Hans\",\"Mu\"\"eller\",30,\"\""
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromCsv(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        #endregion

        #region AsXml

        [Test]
        public void AsXml_Standard()
        {
            dynamic element;
            string xmlContent;
            string expectedContent;

            //set values
            element = new ExpandoObject();
            element.FirstName = "Hans";
            element.LastName = "Mueller";
            element.Age = 30;
            element.Street = "";

            //get xml
            xmlContent = ExpandoObjectSerializer.AsXml(element);

            //compare
            expectedContent =
                "<DynamicObject>" + Environment.NewLine +
                "  <Properties>" + Environment.NewLine +                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine +
                "    <LastName value=\"Mueller\" />" + Environment.NewLine +
                "    <Age value=\"30\" />" + Environment.NewLine +
                "    <Street value=\"\" />" + Environment.NewLine +
                "  </Properties>" + Environment.NewLine +
                "</DynamicObject>";

            Assert.AreEqual(expectedContent, xmlContent);
        }

        [Test]
        public void AsXml_Null()
        {
            dynamic element = null;
            string xmlContent;
            string expectedContent;

            //get xml
            xmlContent = ExpandoObjectSerializer.AsXml(element);

            //compare
            expectedContent = "";

            Assert.AreEqual(expectedContent, xmlContent);
        }

        [Test]
        public void AsXml_Empty()
        {
            dynamic element;
            string xmlContent;
            string expectedContent;

            //set values
            element = new ExpandoObject();
            
            //get xml
            xmlContent = ExpandoObjectSerializer.AsXml(element);

            //compare
            expectedContent = "";

            Assert.AreEqual(expectedContent, xmlContent);
        }

        #endregion

        #region FromXml

        [Test]
        public void FromXml_Standard()
        {
            dynamic element;

            List<string> xmlContent = new List<string>()
            {                
                "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age"),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromXml(xmlContent, elementDefinitions);

            //compare  
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }        

        [Test]
        public void FromXml_OtherSortOrderInFile()
        {
            dynamic element;

            List<string> xmlContent = new List<string>()
            {                
                "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                                
                "    <Street value=\"\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age"),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromXml(xmlContent, elementDefinitions);

            //compare  
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }
        
        [Test]
        public void FromXml_Empty_Content()
        {
            dynamic element;

            List<string> xmlContent = new List<string>();

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age"),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromXml(xmlContent, elementDefinitions);

            //compare    
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(0, elementDictionary.Count);
        }

        [Test]
        public void FromXml_Empty_Defintions()
        {
            dynamic element;

            List<string> xmlContent = new List<string>()
            {                
                "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                                
                "    <Street value=\"\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>();

            //import
            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(xmlContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_Null_Content()
        {
            dynamic element;

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromXml(null, elementDefinitions);

            //compare    
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(0, elementDictionary.Count);
        }

        [Test]
        public void FromXml_Null_Definitions()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(csvContent, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [Test]
        public void FromXml_EmptyLineOrWhitespace()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "<DynamicObject>" + Environment.NewLine,
                "",
                "   ",
                null,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            element = ExpandoObjectSerializer.FromXml(csvContent, elementDefinitions);

            //compare    
            IDictionary<string, object> elementDictionary = element;
            Assert.AreEqual(4, elementDictionary.Count);

            Assert.AreEqual("Hans", element.FirstName);
            Assert.AreEqual("Mueller", element.LastName);
            Assert.AreEqual(30, element.Age);
            Assert.AreEqual("", element.Street);
        }

        [Test]
        public void FromXml_WrongNumberOfElements()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,                
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_WrongType()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"abc\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_WrongPropertyNames()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <WrongName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_AttributeMissing()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                "<DynamicObject>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <LastName />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Properties>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_WrongContentFormat()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "<DynamicObject>" + Environment.NewLine,
                "  <Propertiessssssssss>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />" + Environment.NewLine,
                "  </Propertiessssssssss>" + Environment.NewLine,
                "</DynamicObject>"
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_WrongXMLFormat()
        {
            dynamic element;

            List<string> csvContent = new List<string>()
            {
                 "<DynamicObjects>" + Environment.NewLine,
                "  <Properties>" + Environment.NewLine,                
                "    <FirstName value=\"Hans\" />" + Environment.NewLine,
                "    <LastName value=\"Mueller\" />" + Environment.NewLine,
                "    <Age value=\"30\" />" + Environment.NewLine,
                "    <Street value=\"\" />"                
            };

            //columns
            List<IDynamicTableColumn> elementDefinitions = new List<IDynamicTableColumn>()
            {
                new DynamicTableColumn<string>("FirstName"),
                new DynamicTableColumn<string>("LastName"),
                new DynamicTableColumn<int>("Age", -1),
                new DynamicTableColumn<string>("Street"),
            };

            //import
            try
            {
                element = ExpandoObjectSerializer.FromXml(csvContent, elementDefinitions);
                Assert.Fail();
            }
            catch (XmlException)
            {
            }
        }

        #endregion
    }
}
