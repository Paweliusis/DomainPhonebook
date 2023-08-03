using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.Classes
{
    class SyncBDWithAD
    {
        public static bool Sync(ObservableCollection<Models.Employee> employees)
        {
            try
            {
                Parallel.ForEach(employees, employee =>
                {
                    if (Classes.DBConnection.CheckEmployeeInDB(employee))
                    {
                        DBConnection.EditEmployeeIdDB(employee);
                    }
                    else
                    {
                        DBConnection.InsertEmployeeInDB(employee);
                    }
                });
                var employeesToDelete = Classes.DBConnection.SelectEmployeesFromDB().Except(employees);
                //var employeesToDelete = employees.Except(Classes.DBConnection.SelectEmployeesFromDB());
                //foreach (var i in employeesToDelete) { MessageBox.Show($"{i.FullName}"); }
                Parallel.ForEach(employeesToDelete, employee => { DBConnection.DeleteEmployeeFromDB(employee); });
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
