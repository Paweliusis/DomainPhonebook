using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using PhoneBook.Properties;

namespace PhoneBook.Classes
{
    class DBConnection
    {
        static readonly string connectionString = "server=192.168.0.95;user id=test;password=test;persistsecurityinfo=True;sslmode=None;" +
    "port=3306;database=Phonebook";

        public static bool CheckUsernameInDB(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string exist = "SELECT EXISTS(SELECT * FROM Accounts WHERE Username = '" + username + "' AND Password = '" + password + "');";
                    MySqlCommand cmd = new MySqlCommand(exist, connection);
                    int isExist = int.Parse(cmd.ExecuteScalar().ToString());
                    if (isExist == 1) return true;
                    else return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static bool CheckUsernameInDB(string login)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string exist = "SELECT EXISTS(SELECT * FROM Accounts WHERE Username = '" + login + "');";
                    MySqlCommand cmd = new MySqlCommand(exist, connection);
                    int isExist = int.Parse(cmd.ExecuteScalar().ToString());
                    if (isExist == 1) return true;
                    else return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static bool InsertAccountInDB(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Accounts (Username, Password) VALUES (@Username, @Password);";
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch(Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static bool InsertEmployeeInDB(Models.Employee employee)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Employees (FullName, Department, Title, IpPhone, TelephoneNumber, Mail, OfficeNumber, City, Street) " +
                        "VALUES (@FullName, @Department, @Title, @IpPhone, @TelephoneNumber, @Mail, @OfficeNumber, @City, @Street);";
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@Department", employee.Department);
                    cmd.Parameters.AddWithValue("@Title", employee.Title);
                    cmd.Parameters.AddWithValue("@IpPhone", employee.IpPhoneNumber);
                    cmd.Parameters.AddWithValue("@TelephoneNumber", employee.TelephoneNumber);
                    cmd.Parameters.AddWithValue("@Mail", employee.Mail);
                    cmd.Parameters.AddWithValue("@OfficeNumber", employee.OfficeNumber);
                    cmd.Parameters.AddWithValue("@City", employee.City);
                    cmd.Parameters.AddWithValue("@Street", employee.Street);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch(Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static bool CheckEmployeeInDB(Models.Employee employee)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string excst = "SELECT EXISTS(SELECT * FROM Employees WHERE IpPhone = '" + employee.IpPhoneNumber + "');";
                    MySqlCommand cmd = new MySqlCommand(excst, connection);
                    int isExist = int.Parse(cmd.ExecuteScalar().ToString());
                    if (isExist == 1) { return true; }
                    else { return false; }
                }
                catch(Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static void EditEmployeeIdDB(Models.Employee employee)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE Employees SET FullName=@FullName, Department=@Department, Title=@Title, TelephoneNumber=@TelephoneNumber, "
                        + "Mail=@Mail, OfficeNumber=@OfficeNumber, City=@City, Street=@Street WHERE IpPhone = @IpPhone;";
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@Department", employee.Department);
                    cmd.Parameters.AddWithValue("@Title", employee.Title);
                    cmd.Parameters.AddWithValue("@IpPhone", employee.IpPhoneNumber);
                    cmd.Parameters.AddWithValue("@TelephoneNumber", employee.TelephoneNumber);
                    cmd.Parameters.AddWithValue("@Mail", employee.Mail);
                    cmd.Parameters.AddWithValue("@OfficeNumber", employee.OfficeNumber);
                    cmd.Parameters.AddWithValue("@City", employee.City);
                    cmd.Parameters.AddWithValue("@Street", employee.Street);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                }
            }    
        }
        public static ObservableCollection<Models.Account> SelectAccountsFromDB()
        {
            ObservableCollection<Models.Account> accounts = new ObservableCollection<Models.Account> { };
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT Username FROM Accounts;";
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Models.Account account = new Models.Account();
                        account.Username = reader["Username"].ToString();
                        accounts.Add(account);
                    }
                    reader.Close();
                    return accounts;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return null;
                }
            }
        }
        public static bool EditAccountInDB(string oldname, string newname, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE Accounts SET Username=@Username, Password=@Password WHERE Username = @Oldname;";
                    cmd.Parameters.AddWithValue("@Username", newname);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Oldname", oldname);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static bool EditAccountInDB(string oldname, string newname)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE Accounts SET Username=@Username WHERE Username = @Oldname;";
                    cmd.Parameters.AddWithValue("@Username", newname);
                    cmd.Parameters.AddWithValue("@Oldname", oldname);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static bool DeleteAccountFromDB(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM Accounts WHERE Username = @Username;";
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return false;
                }
            }
        }
        public static ObservableCollection<Models.Employee> SelectEmployeesFromDB()
        {
            ObservableCollection<Models.Employee> employees = new ObservableCollection<Models.Employee> { };
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM Employees;";
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Models.Employee employee = new Models.Employee();
                        ///
                        employee.Id = int.Parse(reader["EmployeeID"].ToString());
                        ///
                        employee.FullName = reader["FullName"].ToString();
                        employee.Department = reader["Department"].ToString();
                        employee.Title = reader["Title"].ToString();
                        employee.IpPhoneNumber = reader["IpPhone"].ToString();
                        employee.TelephoneNumber = reader["TelephoneNumber"].ToString();
                        employee.Mail = reader["Mail"].ToString();
                        employee.OfficeNumber = reader["officeNumber"].ToString();
                        employee.City = reader["City"].ToString();
                        employee.Street = reader["Street"].ToString();
                        employees.Add(employee);
                    }
                    reader.Close();
                    return employees;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                    return null;
                }
            }
        }
        public static bool CheckDBConnection(string connstring)
        {
            bool result = false;
            MySqlConnection connection = new MySqlConnection(connstring);
            try
            {
                connection.Open();
                result = true;
                connection.Close();
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static void DeleteEmployeeFromDB(Models.Employee employee)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM Employees WHERE IpPhone = @IpPhone;";
                    cmd.Parameters.AddWithValue("@IpPhone", employee.IpPhoneNumber);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}");
                }
            }
        }
        public static int SelectTitleWeight(string jobTitle)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string commandText = "SELECT JobSortingWeight FROM JobTitles WHERE JobName = '" + jobTitle + "';";
                    MySqlCommand cmd = new MySqlCommand(commandText, connection);
                    //var temp = cmd.ExecuteScalar().ToString();
                    //if (String.IsNullOrEmpty(cmd.ExecuteScalar().ToString())) { return 0; }
                    //else return 0;
                    var qwe = cmd.ExecuteScalar();
                    connection.Close();
                    if (qwe == null) { return 0; }
                    else { return int.Parse(qwe.ToString()); }
                }
                catch (Exception e)
                {
                    return 99;
                }
            }
            //if (jobTitle == "Начальник отдела") { return 1; }
            //if (jobTitle == "Заместитель начальника отдела") { return 2; }
            //if (jobTitle == "Главный казначей") { return 3; }
            //if (jobTitle == "Главный специалист-эксперт") { return 4; }
            //if (jobTitle == "Ведущий специалист-эксперт") { return 5; }
            //if (jobTitle == "Стариший специалист 1 разряда") { return 6; }
            //if (jobTitle == "Специалист 1 разряда") { return 7; }
            //return 99;
        }
        //public static void WriteSession(string username, string computername)
        //{
        //    if (!CheckSessionInDB(username, computername))
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            try
        //            {
        //                MySqlCommand cmd = connection.CreateCommand();
        //                cmd.CommandText = "INSERT INTO Sessions (Sessionusername, SessionComputerName) VALUES (@SessionUsername, @SessionComputerName);";
        //                cmd.Parameters.AddWithValue("@SessionUsername", username);
        //                cmd.Parameters.AddWithValue("@SessionComputerName", computername);
        //                cmd.ExecuteNonQuery();
        //            }
        //            catch(Exception e)
        //            {
        //                MessageBox.Show(e.Message);
        //            }
        //        }
        //    }
        //}
        //private static bool CheckSessionInDB(string userName, string computerName)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        try
        //        {
        //            string exist = "SELECT EXISTS(SELECT * FROM Sessions WHERE SessionUsername = '" + userName + "' AND SessionComputerName = '" +
        //                computerName + "');";
        //            MySqlCommand cmd = new MySqlCommand(exist, connection);
        //            int isExist = int.Parse(cmd.ExecuteScalar().ToString());
        //            if (isExist == 1) { return true; }
        //            else { return false; }
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.Message);
        //            return false;
        //        }
        //    }
        //}
        //public static void DeleteSessionFromDB(string userName, string computerName)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        try
        //        {
        //            MySqlCommand cmd = connection.CreateCommand();
        //            cmd.CommandText = "DELETE FROM Sessions WHERE SessionUsername = '" + userName + "' AND SessionComputerName = '" +
        //                computerName + "';";
        //            cmd.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //        catch(Exception e)
        //        {
        //            MessageBox.Show(e.Message);
        //        }
        //    }
        //}

    }
}
