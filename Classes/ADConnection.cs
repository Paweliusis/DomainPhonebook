using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Collections.ObjectModel;

namespace PhoneBook.Classes
{
    class ADConnection
    {
        private static DirectoryEntry CreateDirectoryEntry(string path)
        {
            //DirectoryEntry ldapConnection = new DirectoryEntry(ip);
            DirectoryEntry ldapConnection = new DirectoryEntry(path);
            var domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName.Split(new char[] { '.' });
            //ldapConnection.Path = $"LDAP://dc={domainName[0]},DC={domainName[1]}";
            //ldapConnection.Path = $"LDAP://OU=Компьютеры,OU=6900,dc=fsfk,dc=inet";
            ldapConnection.Path = $"LDAP:// " + path;
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
            return ldapConnection;
        }
        public static ObservableCollection<Models.Employee> SelectEmployeesFromAD(string searchPath)
        {
            try
            {
                ObservableCollection<Models.Employee> employees = new ObservableCollection<Models.Employee>();
                DirectoryEntry myLDAPConnection = CreateDirectoryEntry(searchPath);
                DirectorySearcher searcher = new DirectorySearcher(myLDAPConnection);
                searcher.Filter = "(&(objectCategory=user)(ipPhone=*))";

                searcher.PropertiesToLoad.Add("displayName");
                searcher.PropertiesToLoad.Add("department");
                searcher.PropertiesToLoad.Add("title");
                searcher.PropertiesToLoad.Add("ipPhone");
                searcher.PropertiesToLoad.Add("telephoneNumber");
                searcher.PropertiesToLoad.Add("mail");
                searcher.PropertiesToLoad.Add("physicalDeliveryOfficeName");
                searcher.PropertiesToLoad.Add("l");
                searcher.PropertiesToLoad.Add("streetAddress");

                var searchResults = searcher.FindAll();
                foreach (SearchResult i in searchResults)
                {
                    DirectoryEntry q = i.GetDirectoryEntry();
                    ////Models.Employee employee = new Models.Employee();
                    ////employee.FullName = (q.Properties["displayName"].Value ?? "NoN").ToString();
                    ////employee.Department = (q.Properties["department"].Value ?? "NoN").ToString();
                    ////employee.Title = (q.Properties["title"].Value ?? "NoN").ToString();
                    ////employee.IpPhoneNumber = q.Properties["ipPhone"].Value.ToString();
                    ////employee.TelephoneNumber = (q.Properties["telephoneNumber"].Value ?? "NoN").ToString();
                    ////employee.Mail = (q.Properties["mail"].Value ?? "NoN").ToString();
                    ////employee.OfficeNumber = (q.Properties["physicalDeliveryOfficeName"].Value ?? "NoN").ToString();
                    ////employee.City = (q.Properties["l"].Value ?? "NoN").ToString();
                    ////employee.Street = (q.Properties["streetAddress"].Value ?? "NoN").ToString();
                    Models.Employee employee = new Models.Employee()
                    {
                        FullName = (q.Properties["displayName"].Value ?? "NoN").ToString(),
                        Department = (q.Properties["department"].Value ?? "NoN").ToString(),
                        Title = (q.Properties["title"].Value ?? "NoN").ToString(),
                        IpPhoneNumber = q.Properties["ipPhone"].Value.ToString(),
                        TelephoneNumber = (q.Properties["telephoneNumber"].Value ?? "NoN").ToString(),
                        Mail = (q.Properties["mail"].Value ?? "NoN").ToString(),
                        OfficeNumber = (q.Properties["physicalDeliveryOfficeName"].Value ?? "NoN").ToString(),
                        City = (q.Properties["l"].Value ?? "NoN").ToString(),
                        Street = (q.Properties["streetAddress"].Value ?? "NoN").ToString()
                    };
                    employees.Add(employee);
                }
                searchResults.Dispose();
                return employees;
            }
            catch(Exception e) 
            { 
                MessageBox.Show($"{e.Message}");
                return null;
            }
        }
    }
}
