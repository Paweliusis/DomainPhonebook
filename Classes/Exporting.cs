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

                    int numb = ws.LastRowUsed().RowNumber();
                    IXLCell Cellfornew = ws.Cell(numb + 1, 1);

                    Cellfornew.SetValue($"{dep}");

                    numb = ws.LastRowUsed().RowNumber();
                    ws.Range($"A{numb}:E{numb}").Merge();

                    numb = ws.LastRowUsed().RowNumber();
                    Cellfornew = ws.Cell(numb + 1, 1);

                    Cellfornew.InsertTable(dataTable);
                    wbook.Save();
                    dataTable.Dispose();
                    dataSet.Dispose();
                }
                wbook.Dispose();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Что-то пошло не так - {e.Message}");
                return false;
            }
        }
    }
}
