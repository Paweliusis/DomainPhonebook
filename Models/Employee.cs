using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    /*public*/public class Employee : IEquatable<Employee>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private int _id;
        private string _fullName;
        private string _department;
        private string _title;
        private string _ipPhoneNumber;
        private string _telephoneNumber;
        private string _mail;
        private string _officeNumber;
        private string _city;
        private string _street;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }
        public string Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public string IpPhoneNumber
        {
            get { return _ipPhoneNumber; }
            set
            {
                _ipPhoneNumber = value;
                OnPropertyChanged();
            }
        }
        public string TelephoneNumber
        {
            get { return _telephoneNumber; }
            set
            {
                _telephoneNumber = value;
                OnPropertyChanged();
            }
        }
        public string Mail
        {
            get { return _mail; }
            set
            {
                _mail = value;
                OnPropertyChanged();
            }
        }
        public string OfficeNumber
        {
            get { return _officeNumber; }
            set
            {
                _officeNumber = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }
        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged();
            }
        }
        public bool Equals(Employee other)
        {
            if (other is null)
                return false;

            return this.FullName == other.FullName && this.Department == other.Department && this.Title == other.Title
                && this.IpPhoneNumber == other.IpPhoneNumber && this.TelephoneNumber == other.TelephoneNumber && this.Mail == other.Mail
                && this.OfficeNumber == other.OfficeNumber && this.City == other.City && this.Street == other.Street;
        }
        public override bool Equals(object obj) => Equals(obj as Employee);
        public override int GetHashCode() => (FullName, Department, Title, IpPhoneNumber, TelephoneNumber, Mail, OfficeNumber, City, Street).GetHashCode();
    }
}
