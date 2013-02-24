using NUnit.Framework;
using System.CodeDom.Compiler;
using OS.Toolbox.DynamicObjects;
using System.Dynamic;
using System.Collections.Generic;
using System;

namespace OS.Toolbox.DynamicObjectsUnitTest.DynamicTableUnitTest
{
    [GeneratedCodeAttribute("UnitTest", "")] //NOTE: this tag is added to provide fx cop from analyzing this class
    [TestFixture]
    public class FunctionsUnitTest
    {
        #region PreDefineColumns

        [Test]
        public void PreDefineColumns_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //set columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName", ""),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

            //compare            
            Assert.AreEqual(3, table.Columns.Count);

            Assert.AreEqual("FirstName", table.Columns[0].Name);            
            Assert.AreEqual("", table.Columns[0].DefaultValue);

            Assert.AreEqual("LastName", table.Columns[1].Name);
            Assert.AreEqual(null, table.Columns[1].DefaultValue);

            Assert.AreEqual("Age", table.Columns[2].Name);
            Assert.AreEqual(-1, table.Columns[2].DefaultValue);
        }

        [Test]
        public void PreDefineColumns_AlreadyDefined()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //set columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName", ""),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

            //compare            
            Assert.AreEqual(3, table.Columns.Count);

            //set columns again
            try
            {
                table.PreDefineColumns(
                    new List<IDynamicTableColumn>()
                    {
                        new DynamicTableColumn<string>("FirstName", ""),
                        new DynamicTableColumn<string>("LastName"),
                        new DynamicTableColumn<int>("Age", -1)
                    });

                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }
        }

        [Test]
        public void PreDefineColumns_Null()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //set columns
            try
            {
                table.PreDefineColumns(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [Test]
        public void PreDefineColumns_Empty()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //set columns
            try
            {
                table.PreDefineColumns(new List<IDynamicTableColumn>());

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void PreDefineColumns_Error_NameNull()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //set columns
            try
            {
                table.PreDefineColumns(
                    new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>(null, ""),
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }       

        [Test]
        public void PreDefineColumns_Error_NameEmpty()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //set columns
            try
            {
                table.PreDefineColumns(
                    new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("", ""),
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }       

        [Test]
        public void PreDefineColumns_Error_NameNotUnique()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //set columns
            try
            {
                table.PreDefineColumns(
                    new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName", ""),
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }       

        #endregion

        #region AddRow

        [Test]
        public void AddRow_Error_WrongType()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            try
            {
                row = new ExpandoObject();
                row.LastName = 50;
                row.Street = "Main street";
                table.AddRow(row);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void AddRow_Error_WellFormet_MissingElement()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormet);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            try
            {
                row = new ExpandoObject();
                row.FirstName = "Hans";
                row.LastName = "Meier";                
                table.AddRow(row);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void AddRow_Error_AdditionalElement_DefineOnce()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.DefineOnce);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            try
            {
                row = new ExpandoObject();
                row.FirstName = "Hans";
                row.LastName = "Meier";
                row.Age = 30;
                row.Street = "Main street";
                table.AddRow(row);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void AddRow_Error_AdditionalElement_WellFormet()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormet);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            try
            {
                row = new ExpandoObject();
                row.FirstName = "Hans";
                row.LastName = "Meier";
                row.Age = 30;
                row.Street = "Main street";
                table.AddRow(row);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region AddRows

        [Test]
        public void AddRows_Error_Rollback()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;
            dynamic rowB;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            try
            {
                row = new ExpandoObject();
                row.FirstName = "Hans";
                row.LastName = "Meier";
                row.Age = 30;

                rowB = new ExpandoObject();
                rowB.LastName = 50;
                rowB.Street = "Main street";

                table.AddRows(new List<dynamic>(){ row, rowB});

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            //compare
            Assert.AreEqual(1, table.Rows.Count);
        }

        #endregion

        #region GetColumn

        [Test]
        public void GetColumn_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            IDynamicTableColumn column;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);
            
            //compare
            column = table.GetColumn("LastName");
            Assert.AreEqual("LastName", column.Name);

            column = table.GetColumn("Age");
            Assert.AreEqual("Age", column.Name);            
        }

        [Test]
        public void GetColumn_NotExisting()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            IDynamicTableColumn column;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            //compare
            column = table.GetColumn("xxxxxxx");
            Assert.AreEqual(null, column);
        }

        [Test]
        public void GetColumn_Null()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            IDynamicTableColumn column;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            //compare
            column = table.GetColumn(null);
            Assert.AreEqual(null, column);
        }

        [Test]
        public void GetColumn_Empty()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            IDynamicTableColumn column;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            //compare
            column = table.GetColumn("");
            Assert.AreEqual(null, column);
        }

        [Test]
        public void GetColumn_NotIntialized()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            
            IDynamicTableColumn column;
            
            //compare
            column = table.GetColumn("acb");
            Assert.AreEqual(null, column);
        }

        #endregion

        #region AsCsv

        [Test]
        public void AsCsv_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;
            string csvContent;
            string expectedContent;

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

            //get csv
            csvContent = table.AsCsv();

            //compare
            expectedContent = "\"Hans\",\"Mueller\",30,\"\"" + Environment.NewLine +
                 "\"\",\"Meier\",0,\"Main street\"" + Environment.NewLine;

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_Comma_WithHeaders_WithQuotes()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;
            string csvContent;
            string expectedContent;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mu\"eller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            table.AddRow(row);

            //get csv
            csvContent = table.AsCsv(true, ',', true);

            //compare
            expectedContent = "FirstName,LastName,Age,Street" + Environment.NewLine +
                 "\"Hans\",\"Mu\"\"eller\",30,\"\"" + Environment.NewLine +
                 "\"\",\"Meier\",0,\"Main street\"" + Environment.NewLine;

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_Semicolon_WithoutHeaders_WithoutQuotes()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;
            string csvContent;
            string expectedContent;

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

            //get csv
            csvContent = table.AsCsv(false, ';', false);

            //compare
            expectedContent = "Hans;Mueller;30;" + Environment.NewLine +
                 ";Meier;0;Main street" + Environment.NewLine;

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_NotInitialized()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            string csvContent;

            //get csv
            csvContent = table.AsCsv();

            //compare
            Assert.AreEqual("", csvContent);
        }

        [Test]
        public void AsCsv_Empty_WithHeaders()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            string csvContent;
            string expectedContent;

            //set columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName", ""),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

            //get csv
            csvContent = table.AsCsv(true, ',', false);

            //compare
            expectedContent = "FirstName,LastName,Age" + Environment.NewLine;

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsCsv_Empty_WithoutHeaders()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            string csvContent;
            
            //set columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName", ""),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

            //get csv
            csvContent = table.AsCsv(false, ',', false);

            //compare           
            Assert.AreEqual("", csvContent);
        }

        #endregion

        #region FromCsv

        [Test]
        public void FromCsv_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",30,\"\"",
                 "\"\",\"Meier\",0,\"Main street\""
            };
            
            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void FromCsv_Comma_WithHeaders_WithQuotes()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            List<string> csvContent = new List<string>()
            {
                 "FirstName,LastName,Age,Street",
                 "\"Hans\",\"Mu\"\"eller\",30,\"\"",
                 "\"\",\"Meier\",0,\"Main street\""
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent, true, ',', true);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mu\"eller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void FromCsv_Semicolon_WithoutHeaders_WithoutQuotes()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            List<string> csvContent = new List<string>()
            {
                 "Hans;Mueller;30;",
                 ";Meier;0;Main street"
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent, false, ';', false);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void FromCsv_WithHeaders_OtherSortOrderInFile()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            List<string> csvContent = new List<string>()
            {
                 "Street,FirstName,Age,LastName",
                 ",Hans,30,Mueller",
                 "Main street,,0,Meier"
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent, true, ',', false);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void FromCsv_SeparatorInField()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            List<string> csvContent = new List<string>()
            {
                 "FirstName,LastName,Age,Street",
                 "\"Ha,ns\",\"Mu,\"\",\"\",\"\"\"\",eller\",30,\"\"\"\"\"\"",
                 "\"\"\"\",\"Me,ier\",0,\"Main,,street\""
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent, true, ',', true);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Ha,ns", row.FirstName);
            Assert.AreEqual("Mu,\",\",\"\",eller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual("\"\"", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("\"", row.FirstName);
            Assert.AreEqual("Me,ier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual("Main,,street", row.Street);
        }

        [Test]
        public void FromCsv_Empty()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            List<string> csvContent = new List<string>();
            
            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent);

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromCsv_Null()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            List<string> csvContent = new List<string>();

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(null);

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromCsv_NoColumns()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",30,\"\"",
                 "\"\",\"Meier\",0,\"Main street\""
            };

            //import
            try
            {
                table.FromCsv(csvContent);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }
        }

        [Test]
        public void FromCsv_AlreadyContainsRows()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",30,\"\"",
                 "\"\",\"Meier\",0,\"Main street\""
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            //import again
            try
            {
                table.FromCsv(csvContent);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }
        }

        [Test]
        public void FromCsv_EmptyLineOrWhitespace()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",30,\"\"",
                null,
                "",
                "  ",
                "\"\",\"Meier\",0,\"Main street\""
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromCsv(csvContent);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual("", row.Street);

            row = table.Rows[1];
            Assert.AreEqual("", row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual("Main street", row.Street);
        }

        [Test]
        public void FromCsv_WrongNumberOfElements()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",30,\"\"",
                 "\"\",\"Meier\",0"
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            try
            {
                table.FromCsv(csvContent);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromCsv_WrongType()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            List<string> csvContent = new List<string>()
            {
                "\"Hans\",\"Mueller\",a,\"\"",
                 "\"\",\"Meier\",0,\"Main street\""
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            try
            {
                table.FromCsv(csvContent);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromCsv_WrongColumnNames()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            List<string> csvContent = new List<string>()
            {
                "FirstName,FirstName,Age,Street",
                "\"Hans\",\"Mueller\",30,\"\"",
                 "\"\",\"Meier\",0,\"Main street\""
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            try
            {
                table.FromCsv(csvContent, true, ',', true);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        #endregion

        #region RemoveAllRows

        [Test]
        public void RemoveAllRows_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            //compare
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual(3, table.Columns.Count);

            //reset
            table.RemoveAllRows();

            //compare
            Assert.AreEqual(0, table.Rows.Count);
            Assert.AreEqual(3, table.Columns.Count);
        }

        [Test]
        public void RemoveAllRows_NotInitialized()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);

            //reset
            table.RemoveAllRows();

            //compare
            Assert.AreEqual(null, table.Rows);
            Assert.AreEqual(null, table.Columns);
        }

        #endregion

        #region ResetTable

        [Test]
        public void ResetTable_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            dynamic row;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            //compare
            Assert.AreEqual(1, table.Rows.Count);
            Assert.AreEqual(3, table.Columns.Count);

            //reset
            table.ResetTable();

            //compare
            Assert.AreEqual(null, table.Rows);
            Assert.AreEqual(null, table.Columns);
        }

        [Test]
        public void ResetTable_NotInitialized()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandeable);
            
            //reset
            table.ResetTable();

            //compare
            Assert.AreEqual(null, table.Rows);
            Assert.AreEqual(null, table.Columns);
        }

        #endregion
    }
}
