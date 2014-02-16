﻿using NUnit.Framework;
using System.CodeDom.Compiler;
using OS.Toolbox.DynamicObjects;
using System.Dynamic;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Data;

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormed);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormed);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            string csvContent;

            //get csv
            csvContent = table.AsCsv();

            //compare
            Assert.AreEqual("", csvContent);
        }

        [Test]
        public void AsCsv_Empty_WithHeaders()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
        public void FromCsv_NotUniqueColumnNames()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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

        [Test]
        public void FromCsv_WrongColumnNames()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> csvContent = new List<string>()
            {
                "FirstName,Abc,Age,Street",
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

        #region AsXml

        [Test]
        public void AsXml_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            csvContent = table.AsXml();

            //compare
            expectedContent =
                "<DynamicTable>" + Environment.NewLine +
                "  <Rows>" + Environment.NewLine +
                "    <Row>" + Environment.NewLine +
                "      <FirstName value=\"Hans\" />" + Environment.NewLine +
                "      <LastName value=\"Mueller\" />" + Environment.NewLine +
                "      <Age value=\"30\" />" + Environment.NewLine +
                "      <Street value=\"\" />" + Environment.NewLine +
                "    </Row>" + Environment.NewLine +
                "    <Row>" + Environment.NewLine +
                "      <FirstName value=\"\" />" + Environment.NewLine +
                "      <LastName value=\"Meier\" />" + Environment.NewLine +
                "      <Age value=\"0\" />" + Environment.NewLine +
                "      <Street value=\"Main street\" />" + Environment.NewLine +
                "    </Row>" + Environment.NewLine +
                "  </Rows>" + Environment.NewLine +
                "</DynamicTable>";

            Assert.AreEqual(expectedContent, csvContent);
        }

        [Test]
        public void AsXml_NotInitialized()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            string csvContent;

            //get csv
            csvContent = table.AsXml();

            //compare
            Assert.AreEqual("", csvContent);
        }

        [Test]
        public void AsXml_Empty()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            csvContent = table.AsXml();

            //compare
            expectedContent =
                 "<DynamicTable>" + Environment.NewLine +
                 "  <Rows />" + Environment.NewLine +
                 "</DynamicTable>";

            Assert.AreEqual(expectedContent, csvContent);
        }

        #endregion

        #region FromXml

        [Test]
        public void FromXml_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromXml(xmlContent);

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
        public void FromXml_OtherSortOrderInFile()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,                
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,                
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
            };

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<string>("Street"),
                });

            //import
            table.FromXml(xmlContent);

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
        public void FromXml_Empty()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>();

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
            table.FromXml(xmlContent);

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromXml_Null()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            table.FromXml(null);

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromXml_NoColumns()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
            };

            //import
            try
            {
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }
        }

        [Test]
        public void FromXml_AlreadyContainsRows()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
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
            table.FromXml(xmlContent);

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            //import again
            try
            {
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }
        }

        [Test]
        public void FromXml_EmptyLineOrWhitespace()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "",
                "   ",
                null,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
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
            table.FromXml(xmlContent);

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
        public void FromXml_WrongNumberOfElements()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,                
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
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
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromXml_WrongType()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"abc\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
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
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromXml_NotUniqueColumnNames()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <LastName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
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
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void FromXml_WrongColumnNames()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <Aaaaaaaaa value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
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
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_AttributeMissing()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "  </Rows>" + Environment.NewLine,
                "</DynamicTable>"
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
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (FormatException)
            {
            }
        }

        [Test]
        public void FromXml_WrongContentFormat()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rowsssssssssss>" + Environment.NewLine,
                "    <Rowwwww>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Rowwwww>" + Environment.NewLine,
                "    <Rowwwww>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />" + Environment.NewLine,
                "    </Rowwwww>" + Environment.NewLine,
                "  </Rowsssssssssss>" + Environment.NewLine,
                "</DynamicTable>"
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
            table.FromXml(xmlContent);

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromXml_WrongXMLFormat()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            List<string> xmlContent = new List<string>()
            {
                "<DynamicTable>" + Environment.NewLine,
                "  <Rows>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"Hans\" />" + Environment.NewLine,
                "      <LastName value=\"Mueller\" />" + Environment.NewLine,
                "      <Age value=\"30\" />" + Environment.NewLine,
                "      <Street value=\"\" />" + Environment.NewLine,
                "    </Row>" + Environment.NewLine,
                "    <Row>" + Environment.NewLine,
                "      <FirstName value=\"\" />" + Environment.NewLine,
                "      <LastName value=\"Meier\" />" + Environment.NewLine,
                "      <Age value=\"0\" />" + Environment.NewLine,
                "      <Street value=\"Main street\" />"
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
                table.FromXml(xmlContent);
                Assert.Fail();
            }
            catch (XmlException)
            {
            }
        }

        #endregion

        #region AsDataTable

        [Test]
        public void AsDataTable_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;
            DataTable dataTable;

            //add values
            row = new ExpandoObject();
            row.FirstName = "Hans";
            row.LastName = "Mueller";
            row.Age = 30;
            table.AddRow(row);

            row = new ExpandoObject();
            row.LastName = "Meier";
            row.Street = "Main street";
            row.Birthday = new DateTime(2001, 12, 20);
            table.AddRow(row);

            //get data table
            dataTable = table.AsDataTable();

            //compare
            Assert.AreEqual(5, dataTable.Columns.Count);
            Assert.AreEqual("FirstName", dataTable.Columns[0].ColumnName);
            Assert.AreEqual("LastName", dataTable.Columns[1].ColumnName);
            Assert.AreEqual("Age", dataTable.Columns[2].ColumnName);
            Assert.AreEqual("Street", dataTable.Columns[3].ColumnName);
            Assert.AreEqual("Birthday", dataTable.Columns[4].ColumnName);
            Assert.AreEqual("System.String", dataTable.Columns[0].DataType.ToString());
            Assert.AreEqual("System.String", dataTable.Columns[1].DataType.ToString());
            Assert.AreEqual("System.Int32", dataTable.Columns[2].DataType.ToString());
            Assert.AreEqual("System.String", dataTable.Columns[3].DataType.ToString());
            Assert.AreEqual("System.DateTime", dataTable.Columns[4].DataType.ToString());

            Assert.AreEqual(2, dataTable.Rows.Count);

            Assert.AreEqual("Hans", dataTable.Rows[0].ItemArray[0]);
            Assert.AreEqual("Mueller", dataTable.Rows[0].ItemArray[1]);
            Assert.AreEqual(30, dataTable.Rows[0].ItemArray[2]);
            Assert.AreEqual(DBNull.Value, dataTable.Rows[0].ItemArray[3]);
            Assert.AreEqual(new DateTime(0), dataTable.Rows[0].ItemArray[4]);

            Assert.AreEqual(DBNull.Value, dataTable.Rows[1].ItemArray[0]);
            Assert.AreEqual("Meier", dataTable.Rows[1].ItemArray[1]);
            Assert.AreEqual(0, dataTable.Rows[1].ItemArray[2]);
            Assert.AreEqual("Main street", dataTable.Rows[1].ItemArray[3]);
            Assert.AreEqual(new DateTime(2001, 12, 20), dataTable.Rows[1].ItemArray[4]);            
        }

        [Test]
        public void AsDataTable_Standard_PreDefineColumns()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;
            DataTable dataTable;

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
            table.AddRow(row);

            //get data table
            dataTable = table.AsDataTable();

            //compare
            Assert.AreEqual(3, dataTable.Columns.Count);
            Assert.AreEqual("FirstName", dataTable.Columns[0].ColumnName);
            Assert.AreEqual("LastName", dataTable.Columns[1].ColumnName);
            Assert.AreEqual("Age", dataTable.Columns[2].ColumnName);
            Assert.AreEqual("System.String", dataTable.Columns[0].DataType.ToString());
            Assert.AreEqual("System.String", dataTable.Columns[1].DataType.ToString());
            Assert.AreEqual("System.Int32", dataTable.Columns[2].DataType.ToString());
            
            Assert.AreEqual(2, dataTable.Rows.Count);

            Assert.AreEqual("Hans", dataTable.Rows[0].ItemArray[0]);
            Assert.AreEqual("Mueller", dataTable.Rows[0].ItemArray[1]);
            Assert.AreEqual(30, dataTable.Rows[0].ItemArray[2]);
            
            Assert.AreEqual("", dataTable.Rows[1].ItemArray[0]);
            Assert.AreEqual("Meier", dataTable.Rows[1].ItemArray[1]);
            Assert.AreEqual(-1, dataTable.Rows[1].ItemArray[2]);            
        }

        [Test]
        public void AsDataTable_NotInitialized()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            DataTable dataTable;

            //get csv
            dataTable = table.AsDataTable();

            //compare
            Assert.AreEqual(null, dataTable);
        }

        [Test]
        public void AsDataTable_Empty()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            DataTable dataTable;

            //set columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName", ""),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1)
                });

            //get csv
            dataTable = table.AsDataTable();

            //compare            
            Assert.AreEqual(3, dataTable.Columns.Count);
            Assert.AreEqual("FirstName", dataTable.Columns[0].ColumnName);
            Assert.AreEqual("LastName", dataTable.Columns[1].ColumnName);
            Assert.AreEqual("Age", dataTable.Columns[2].ColumnName);
            Assert.AreEqual("System.String", dataTable.Columns[0].DataType.ToString());
            Assert.AreEqual("System.String", dataTable.Columns[1].DataType.ToString());
            
            Assert.AreEqual(0, dataTable.Rows.Count);            
        }

        #endregion

        #region FromXml

        [Test]
        public void FromDataTable_Standard_PreDefinedTable()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;          
            
            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //import
            table.FromDataTable(CreateSimpleTestData());

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual(new DateTime(2001, 12, 20), row.Birthday);

            row = table.Rows[1];
            Assert.AreEqual(null, row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(new DateTime(0), row.Birthday);
        }

        [Test]
        public void FromDataTable_Standard_NotPreDefined()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            //import
            table.FromDataTable(CreateSimpleTestData());

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual(new DateTime(2001, 12, 20), row.Birthday);

            row = table.Rows[1];
            Assert.AreEqual(null, row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(new DateTime(0), row.Birthday);
        }

        [Test]
        public void FromDataTable_OtherSortOrderInFile()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;          
            
            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),                                       
                    new DynamicTableColumn<DateTime>("Birthday"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<string>("LastName")
                });

            //import
            table.FromDataTable(CreateSimpleTestData());

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual(new DateTime(2001, 12, 20), row.Birthday);

            row = table.Rows[1];
            Assert.AreEqual(null, row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(new DateTime(0), row.Birthday);
        }

        [Test]
        public void FromDataTable_Empty_NoColumns()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //import
            table.FromDataTable(new DataTable());

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromDataTable_Empty_NoRows_PreDefinedTable()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            DataTable data;

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //prepare data
            data = CreateSimpleTestData();
            data.Rows.Clear();

            //import
            table.FromDataTable(data);

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromDataTable_Empty_NoRows_NotPreDefined()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            DataTable data;

            //prepare data
            data = CreateSimpleTestData();
            data.Rows.Clear();

            //import
            table.FromDataTable(data);

            //compare    
            Assert.AreEqual(null, table.Columns);
            Assert.AreEqual(null, table.Rows);
        }

        [Test]
        public void FromDataTable_Null()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            table.FromDataTable(null);

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromDataTable_AlreadyContainsRows()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age", -1),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //import
            table.FromDataTable(CreateSimpleTestData());

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            //import again
            try
            {
                table.FromDataTable(CreateSimpleTestData());
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }
        }

        [Test]
        public void FromDataTable_WrongNumberOfColumns_WellFormed()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormed);
            
            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age"),
                });

            //import
            try
            {
                table.FromDataTable(CreateSimpleTestData());
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromDataTable_WrongNumberOfColumns_DefineOnce()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.DefineOnce);

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age"),
                });

            //import
            try
            {
                table.FromDataTable(CreateSimpleTestData());
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromDataTable_WrongNumberOfColumns_Expandable()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastName"),
                    new DynamicTableColumn<int>("Age"),                    
                });

            //import
            table.FromDataTable(CreateSimpleTestData());

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual(new DateTime(2001, 12, 20), row.Birthday);

            row = table.Rows[1];
            Assert.AreEqual(null, row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(new DateTime(0), row.Birthday);
        }

        [Test]
        public void FromDataTable_WrongType()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);       

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<int>("LastName"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //import
            try
            {
                table.FromDataTable(CreateSimpleTestData());
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            //compare    
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void FromDataTable_WrongColumnNames_WellFormed()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.WellFormed);

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastNameeeeee"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //import
            try
            {
                table.FromDataTable(CreateSimpleTestData());
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void FromDataTable_WrongColumnNames_DefineOnce()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.DefineOnce);

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastNameeeeee"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //import
            try
            {
                table.FromDataTable(CreateSimpleTestData());
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void FromDataTable_WrongColumnNames_Expandable()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            dynamic row;

            //columns
            table.PreDefineColumns(
                new List<IDynamicTableColumn>()
                {
                    new DynamicTableColumn<string>("FirstName"),
                    new DynamicTableColumn<string>("LastNameeeeee"),
                    new DynamicTableColumn<int>("Age"),
                    new DynamicTableColumn<DateTime>("Birthday"),
                });

            //import
            table.FromDataTable(CreateSimpleTestData());

            //compare    
            Assert.AreEqual(2, table.Rows.Count);

            row = table.Rows[0];
            Assert.AreEqual("Hans", row.FirstName);
            Assert.AreEqual("Mueller", row.LastName);
            Assert.AreEqual(30, row.Age);
            Assert.AreEqual(new DateTime(2001, 12, 20), row.Birthday);
            Assert.AreEqual(null, row.LastNameeeeee);

            row = table.Rows[1];
            Assert.AreEqual(null, row.FirstName);
            Assert.AreEqual("Meier", row.LastName);
            Assert.AreEqual(0, row.Age);
            Assert.AreEqual(new DateTime(0), row.Birthday);
            Assert.AreEqual(null, row.LastNameeeeee);
        }

        #endregion

        #region RemoveAllRows

        [Test]
        public void RemoveAllRows_Standard()
        {
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);

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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
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
            IDynamicTable table = new DynamicTable(DynamicTableType.Expandable);
            
            //reset
            table.ResetTable();

            //compare
            Assert.AreEqual(null, table.Rows);
            Assert.AreEqual(null, table.Columns);
        }

        #endregion

        #region helper - test data - data table

        private static DataTable CreateSimpleTestData()
        {
            DataTable dataTable = null;
            DataColumn dataColumn = null;
            DataRow dataRow = null;

            //create table
            dataTable = new DataTable("test");

            //create columns
            dataColumn = new DataColumn();
            dataColumn.ColumnName = "FirstName";
            dataColumn.DataType = System.Type.GetType("System.String");
            dataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.ColumnName = "LastName";
            dataColumn.DataType = System.Type.GetType("System.String");
            dataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.ColumnName = "Age";
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.ColumnName = "Birthday";
            dataColumn.DataType = System.Type.GetType("System.DateTime");
            dataTable.Columns.Add(dataColumn);
            
            //create rows
            dataRow = dataTable.NewRow();
            dataRow["FirstName"] = "Hans";
            dataRow["LastName"] = "Mueller";
            dataRow["Age"] = 30;
            dataRow["Birthday"] = new DateTime(2001, 12, 20);
            dataTable.Rows.Add(dataRow);

            dataRow = dataTable.NewRow();
            dataRow["FirstName"] = DBNull.Value;
            dataRow["LastName"] = "Meier";
            dataRow["Age"] = 0;
            dataRow["Birthday"] = new DateTime(0);
            dataTable.Rows.Add(dataRow);

            //return
            return dataTable;
        }

        #endregion
    }
}
