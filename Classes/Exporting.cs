using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.Classes
{
    class Exporting
    {
        public static bool Export(string path)
        {
            try
            {
                //var dataSet = new DataSet();
                //var dataTable = new DataTable();
                //dataSet.Tables.Add(dataTable);

                // we assume that the properties of DataSourceVM are the columns of the table
                // you can also provide the type via the second parameter
                //dataTable.Columns.Add("Title");
                //dataTable.Columns.Add("FullName");
                //dataTable.Columns.Add("IpPhoneNumber");
                //dataTable.Columns.Add("TelephoneNumber");
                //dataTable.Columns.Add("OfficeNumber");

                var Backends = Classes.DBConnection.SelectEmployeesFromDB();
                var deps = Backends.Select(x => x.Department).Distinct();

                var wbook = new XLWorkbook();
                var ws = wbook.Worksheets.Add("phonebook");
                ws.Cell("A1").Value = $"Data exported from Phonebook by {DateTime.Now}";
                wbook.SaveAs($@"{path}\phonebook.xlsx");

                foreach (var dep in deps)
                {
                    var dataSet = new DataSet();
                    var dataTable = new DataTable();
                    dataSet.Tables.Add(dataTable);

                    dataTable.Columns.Add("Title");
                    dataTable.Columns.Add("FullName");
                    dataTable.Columns.Add("IpPhoneNumber");
                    dataTable.Columns.Add("TelephoneNumber");
                    dataTable.Columns.Add("OfficeNumber");

                    var selDep = from p in Backends
                                 where p.Department == dep
                                 select p;
                    ObservableCollection<Models.Employee> selDepO = new ObservableCollection<Models.Employee>(selDep);
                    foreach (var element in selDepO)
                    {
                        var newRow = dataTable.NewRow();

                        newRow["Title"] = element.Title;
                        newRow["FullName"] = element.FullName;
                        newRow["IpPhoneNumber"] = element.IpPhoneNumber;
                        newRow["TelephoneNumber"] = element.TelephoneNumber;
                        newRow["OfficeNumber"] = element.OfficeNumber;

                        dataTable.Rows.Add(newRow);
                    }

                    //XLWorkbook wb = new XLWorkbook($"{path}\\phonebook.xlsx");
                    //IXLWorksheet worksheet = wb.Worksheet("phonebook");
                    //int numb = worksheet.LastRowUsed().RowNumber();
                    //IXLCell Cellfornew = worksheet.Cell(numb + 1, 1);
                    int numb = ws.LastRowUsed().RowNumber();
                    IXLCell Cellfornew = ws.Cell(numb + 1, 1);

                    Cellfornew.SetValue($"{dep}");
                    //wb.Save();


                    //int qwerty = worksheet.LastRowUsed().RowNumber();
                    //worksheet.Range($"A{qwerty}:E{qwerty}").Merge();
                    //wb.Save();
                    numb = ws.LastRowUsed().RowNumber();
                    ws.Range($"A{numb}:E{numb}").Merge();


                    //numb = worksheet.LastRowUsed().RowNumber();
                    //Cellfornew = worksheet.Cell(numb + 1, 1);
                    numb = ws.LastRowUsed().RowNumber();
                    Cellfornew = ws.Cell(numb + 1, 1);

                    Cellfornew.InsertTable(dataTable);
                    //wb.Save();
                    //wb.Dispose();
                    wbook.Save();
                    //wbook.Dispose();
                    dataTable.Dispose();
                    dataSet.Dispose();
                }
                wbook.Dispose();
                return true;
                //foreach (var element in Backends)
                //{
                //    var newRow = dataTable.NewRow();

                //    // fill the properties into the cells
                //    newRow["Title"] = element.Title;
                //    newRow["FullName"] = element.FullName;
                //    newRow["IpPhoneNumber"] = element.IpPhoneNumber;
                //    newRow["TelephoneNumber"] = element.TelephoneNumber;
                //    newRow["OfficeNumber"] = element.OfficeNumber;

                //    dataTable.Rows.Add(newRow);
                //}
                //XLWorkbook wb = new XLWorkbook();
                //wb.Worksheets.Add(dataTable, "example");
                //wb.SaveAs("example.xlsx");
                ////Do excel export
            }
            catch (Exception e)
            {
                MessageBox.Show($"Что-то пошло не так - {e.Message}");
                return false;
            }
        }
    }
}
