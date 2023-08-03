using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event EventHandler OnRequestClose;
        public LoginViewModel()
        {
            _authenticated = false;
            this._confirmLoginCommand = new Classes.Command(this.ConfirmLogin);
            this._cancelLoginCommand = new Classes.Command(this.CancelLogin);
        }

        private string _loginString;
        public string LoginString
        {
            get { return _loginString; }
            set
            {
                _loginString = value;
                OnPropertyChanged("LoginString");
            }
        }
        public SecureString SecurePassword { private get; set; }

        private readonly Classes.Command _confirmLoginCommand;
        public Classes.Command ConfirmLoginCommand
        {
            get { return _confirmLoginCommand; }
        }
        private void ConfirmLogin(object state)
        {
            if (!String.IsNullOrEmpty(LoginString))
            {
                if (Classes.Validating.ValidateLogIn(LoginString, SecurePassword))
                {
                    _authenticated = true;
                    OnRequestClose(this, new EventArgs());
                }
                else { MessageBox.Show("Неверно введено имя пользователя или пароль", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
            else { MessageBox.Show("Пожалуйста введите имя пользователя", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private bool _authenticated;
        public bool Authenticated
        {
            get { return _authenticated; }
        }

        private readonly Classes.Command _cancelLoginCommand;
        public Classes.Command CancelLoginCommand
        {
            get { return _cancelLoginCommand; }
        }
        private void CancelLogin(object state)
        {
            OnRequestClose(this, new EventArgs());
        }
    }
}
