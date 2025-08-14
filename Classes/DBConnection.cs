using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using PhoneBook.Properties;
using System.Data.SQLite;

namespace PhoneBook.Classes
{
    class DBConnection
    {
        static string GetConnString()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["Default"];
            return settings.ConnectionString;
        }
        static readonly string connectionString = GetConnString();

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
                catch (Exception e)
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
                catch (Exception e)
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
                catch (Exception e)
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
                catch (Exception e)
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                result = false;
            }
            return result;
        }
        public static bool CheckDBConnection()
        {
            bool result = false;
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                result = true;
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
                    var weight = cmd.ExecuteScalar();
                    connection.Close();
                    if (weight == null) { return 99; }
                    else { return int.Parse(weight.ToString()); }
                }
                catch (Exception e)
                {
                    return 99;
                }
            }
        }

        public static void SQLiteDBInit()
        {
            string dbname = "phonebook.db";
            string connString = $"Data Source={dbname}";

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                try
                {
                    string defaultPassword = "qX5X2y3p";
                    string salt = Classes.Encoding.CreateSalt();
                    string passWithHash = Classes.Encoding.HashAndSaltPassword(defaultPassword, salt);
                    string cmdText = "CREATE TABLE IF NOT EXISTS Accounts (Id INTEGER PRIMARY KEY AUTOINCREMENT,Username TEXT UNIQUE NOT NULL,Password TEXT NOT NULL," +
                        "Salt TEXT UNIQUE NOT NULL); INSERT OR IGNORE INTO Accounts (Username, Password, Salt) VALUES ('Admin', '" + passWithHash + "', '" + salt + "');";
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static bool SQLiteCheckLoginInDB(string login)
        {
            string dbname = "phonebook.db";
            string connString = $"Data Source={dbname}";

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                try
                {
                    string exist = "SELECT EXISTS(SELECT * FROM Accounts WHERE Username = '" + login + "');";
                    SQLiteCommand cmd = new SQLiteCommand(exist, connection);
                    int isExist = int.Parse(cmd.ExecuteScalar().ToString());
                    if (isExist == 1) return true;
                    else return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Что-то пошло не так - {e.Message}");
                    return false;
                }
            }
        }

        public static List<string> SQLiteGetPassAndSalt(string username)
        {
            string dbname = "phonebook.db";
            string connString = $"Data Source={dbname}";

            List<string> passAndSalt = new List<string> { };

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                try
                {
                    SQLiteCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT Password, Salt FROM Accounts WHERE Username = '" + username + "';";
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        passAndSalt.Add(reader["Password"].ToString());
                        passAndSalt.Add(reader["Salt"].ToString());
                    }
                    reader.Close();
                    return passAndSalt;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }
        }

        public static ObservableCollection<Models.JobTitle> SelectJobsRulesFromDB()
        {
            ObservableCollection<Models.JobTitle> jobTitles = new ObservableCollection<Models.JobTitle> { };
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT JobName, JobSortingWeight FROM JobTitles;";
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Models.JobTitle jobTitle = new Models.JobTitle();
                        jobTitle.Name = reader["JobName"].ToString();
                        jobTitle.Weight = int.Parse(reader["JobSortingWeight"].ToString());
                        jobTitles.Add(jobTitle);
                    }
                    reader.Close();
                    return jobTitles;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Что-то пошло не так - {e.Message}");
                    return null;
                }
            }
        }

        public static List<Models.JobTitle> SelectMissingJobTitles()
        {
            List<Models.JobTitle> jobTitles = new List<Models.JobTitle> { };
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    //cmd.CommandText = "SELECT t1.Title FROM Employees t1 LEFT JOIN JobTitles t2 ON t1.Title = t2.JobName WHERE t2.JobName IS NULL;";
                    cmd.CommandText = "SELECT t1.Title FROM Employees t1 LEFT JOIN JobTitles t2 ON t1.Title = t2.JobName WHERE t2.JobName IS NULL GROUP BY t1.Title;";
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Models.JobTitle jobTitle = new Models.JobTitle();
                        jobTitle.Name = reader["Title"].ToString();
                        jobTitle.Weight = 99;
                        jobTitles.Add(jobTitle);
                    }
                    reader.Close();
                    return jobTitles;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Что-то пошло не так - {e.Message}");
                    return null;
                }
            }
        }
        public static void UpdateJobTitleWeight(Models.JobTitle jobTitle)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE JobTitles SET JobSortingWeight=@JobSortingWeight WHERE JobName=@JobName;";
                    cmd.Parameters.AddWithValue("@JobSortingWeight", jobTitle.Weight);
                    cmd.Parameters.AddWithValue("@JobName", jobTitle.Name);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Что-то пошло не так - {e.Message}");
                }
            }
        }
        public static void InsertJobTitleWeight(Models.JobTitle jobTitle)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO JobTitles (JobName, JobSortingWeight) VALUES (@JobName, @JobSortingWeight);";
                    cmd.Parameters.AddWithValue("@JobName", jobTitle.Name);
                    cmd.Parameters.AddWithValue("@JobSortingWeight", jobTitle.Weight);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Что-то пошло не так - {e.Message}");
                }
            }
        }
    }
}
