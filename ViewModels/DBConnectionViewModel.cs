using PhoneBook.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.ViewModels
{
    class DBConnectionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event EventHandler OnRequestClose;
        public DBConnectionViewModel()
        {
            this._confirmAndCloseCommand = new Classes.Command(this.ConfirmAndClose);
            DBConnectionString = PBProperties.DBConnectionString;
        }

        private string _dbConnectionString;
        public string DBConnectionString
        {
            get { return _dbConnectionString; }
            set { _dbConnectionString = value; }
        }

        private Classes.Command _confirmAndCloseCommand;
        public Classes.Command ConfirmAndCloseCommand
        {
            get { return _confirmAndCloseCommand; }
        }
        public void ConfirmAndClose(object state)
        {
            //if (Classes.DBConnection.CheckDBConnection())
            //{
            //    MessageBox.Show($"Подключиться к БД удалось", "Подключение к БД", MessageBoxButton.OK, MessageBoxImage.Information);
            //    OnRequestClose(this, new EventArgs());
            //}
        }
        private Settings PBProperties;
    }
}
