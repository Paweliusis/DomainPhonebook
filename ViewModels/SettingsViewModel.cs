using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ClosedXML.Excel;
using PhoneBook.Properties;

namespace PhoneBook.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event EventHandler OnRequestClose;
        public SettingsViewModel()
        {
            PhonebookSynchronized = false;
            this._syncWithADCommand = new Classes.Command(this.SyncWithAD);
            DomainSearchPath = PBParams.domainSearchPath;
            this._openAccountsCommand = new Classes.Command(this.OpenAccounts);
            this._saveDBConnectionStringCommand = new Classes.Command(this.SaveDBConnectionString);
            if (String.IsNullOrEmpty(PBParams.DBConnectionString))
            {
                DBConnectionString = Classes.Reader.ReadConnectionString();
            }
            else { DBConnectionString = PBParams.DBConnectionString; }
            //DBConnectionString = PBParams.DBConnectionString;
            this._goBackCommand = new Classes.Command(this.GoBack);
        }

        private readonly Classes.Command _syncWithADCommand;
        public Classes.Command SyncWithADCommand
        {
            get { return _syncWithADCommand; }
        }
        private void SyncWithAD(object state)
        {
            var confirmSync = MessageBox.Show($"Вы уверены, что хотите синхронизировать справочник с AD? (Этот процесс не обратим и затронит все копии " +
                $"телефонного справочника)", "Настройки", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmSync == MessageBoxResult.Yes && !String.IsNullOrEmpty(DomainSearchPath))
            {
                ADEmployees = Classes.ADConnection.SelectEmployeesFromAD(DomainSearchPath);
                if (ADEmployees != null)
                {
                    var resultOfSync = Classes.SyncBDWithAD.Sync(ADEmployees);
                    if (resultOfSync) { MessageBox.Show($"Синхронизация AD и DB прошла успешна", "Settings", MessageBoxButton.OK, 
                        MessageBoxImage.Information); }
                    PBParams.domainSearchPath = DomainSearchPath;
                    PBParams.Save();
                    PhonebookSynchronized = true;
                }
            }
            if (String.IsNullOrEmpty(DomainSearchPath))
            {
                MessageBox.Show("Пожалуйста, заполните путь поиска пользователей для телефонного справочника", "Настройки", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private bool _phonebookHasSynchronized;
        public bool PhonebookSynchronized
        {
            get { return _phonebookHasSynchronized; }
            set { _phonebookHasSynchronized = value; }
        }

        private ObservableCollection<Models.Employee> _employees;
        public ObservableCollection<Models.Employee> ADEmployees
        {
            get { return _employees; }
            set { _employees = value; }
        }

        //private string _domainIpAddress;
        //public string DomainIpAddress
        //{
        //    get { return _domainIpAddress; }
        //    set
        //    {
        //        _domainIpAddress = value;
        //        OnPropertyChanged("DomainIpAddress");
        //    }
        //}

        private string _domainSearchPath;
        public string DomainSearchPath
        {
            get { return _domainSearchPath; }
            set
            {
                _domainSearchPath = value;
                OnPropertyChanged("DomainSearchPath");
            }
        }

        private Settings PBParams = new Settings();

        private readonly Classes.Command _openAccountsCommand;
        public Classes.Command OpenAccountsCommand
        {
            get { return _openAccountsCommand; }
        }
        private void OpenAccounts(object state)
        {
            var Accounts = new Windows.Accounts();
            Accounts.Owner = Application.Current.MainWindow;
            Accounts.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Accounts.ShowDialog();
        }

        private string _DBConnectionString;
        public string DBConnectionString
        {
            get { return _DBConnectionString; }
            set
            {
                _DBConnectionString = value;
                OnPropertyChanged("DBConnectionString");
            }
        }

        private readonly Classes.Command _saveDBConnectionStringCommand;
        public Classes.Command SaveDBConnectionStringCommand
        {
            get { return _saveDBConnectionStringCommand; }
        }
        private void SaveDBConnectionString(object state)
        {
            var result = MessageBox.Show($"Вы уверены, что хотите сохранить текущую строку подключения к БД?", "Настройки", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (Classes.DBConnection.CheckDBConnection(DBConnectionString))
                {
                    MessageBox.Show($"Соединение с БД успешно", "Настройки", MessageBoxButton.OK, MessageBoxImage.Information);
                    //PBParams.DBConnectionString = DBConnectionString;
                    //PBParams.Save();
                    Classes.Writer.WriteDBString(DBConnectionString);
                    PBParams.DBConnectionString = DBConnectionString;
                    PBParams.Save();
                }
                else { MessageBox.Show("Невозможно соединиться с БД, пожалуйста, проверьте корректность введенной строки", "Настройки", MessageBoxButton.OK,
                    MessageBoxImage.Error); }
            }
        }

        private readonly Classes.Command _goBackCommand;
        public Classes.Command GoBackCommand
        {
            get { return _goBackCommand; }
        }
        private void GoBack(object state)
        {
            OnRequestClose(this, new EventArgs());
        }
    }
}
