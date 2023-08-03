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
    class ExportPhonebookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event EventHandler OnRequestClose;
        public ExportPhonebookViewModel()
        {
            this._confirmAndExportCommand = new Classes.Command(this.ConfirmAndExport);
            this._cancelAndCloseExportCommand = new Classes.Command(this.CancelAndCloseExport);
        }

        private string _pathString;
        public string PathString
        {
            get { return _pathString; }
            set 
            { 
                _pathString = value;
                OnPropertyChanged("PathString");
            }
        }

        private readonly Classes.Command _confirmAndExportCommand;
        public Classes.Command ConfirmAndExportCommand
        {
            get { return _confirmAndExportCommand; }
        }
        private void ConfirmAndExport(object state)
        {
            if (!String.IsNullOrEmpty(PathString))
            {
                if (Classes.Exporting.Export(PathString))
                {
                    MessageBox.Show($"Процесс экспорта завершился успешно");
                    OnRequestClose(this, new EventArgs());
                }
            }
            else { MessageBox.Show($"Пожалуйста, заполните путь сохранения файла", "Экспорт телефонного справочника", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private readonly Classes.Command _cancelAndCloseExportCommand;
        public Classes.Command CancelAndCloseExportCommand
        {
            get { return _cancelAndCloseExportCommand; }
        }
        private void CancelAndCloseExport(object state)
        {
            OnRequestClose(this, new EventArgs());
        }
    }
}
