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
    class CreateEditAccountViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event EventHandler OnRequestClose;
        public CreateEditAccountViewModel(string label)
        {
            LabelText = label;
            if (label == "Создание аккаунта") { IsCreateAccount = true; }
            else 
            { 
                IsCreateAccount = false;
            }
            this._createEditAccountCommand = new Classes.Command(this.CreateEdit);
            this._cancelCommand = new Classes.Command(this.Cancel);
        }

        private string _labelText;
        public string LabelText
        {
            get { return _labelText; }
            set { _labelText = value; }
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

        private bool _isCreateAccount;
        public bool IsCreateAccount
        {
            get { return _isCreateAccount; }
            set { _isCreateAccount = value; }
        }

        private readonly Classes.Command _createEditAccountCommand;
        public Classes.Command CreateEditAccountCommand
        {
            get { return _createEditAccountCommand; }
        }
        private void CreateEdit(object state)
        {
            if (IsCreateAccount)
            {
                if (Classes.Validating.CheckLoginAndPassword(LoginString, SecurePassword))
                {
                    if (Classes.DBConnection.InsertAccountInDB(LoginString, Classes.Encoding.CreateHashFromSecureString(SecurePassword)))
                    {
                        MessageBox.Show($"Пользователь {LoginString} успешно создан");
                        IsAccountCreated = true;
                        NewAccount = new Models.Account() { Username = LoginString };
                        OnRequestClose(this, new EventArgs());
                    }
                }
            }
            else
            {
                if (LoginString == OldName && SecurePassword == null)
                {
                    OnRequestClose(this, new EventArgs());
                }
                else if (LoginString == OldName)
                {
                    if (Classes.Validating.CheckPassword(SecurePassword))
                    {
                        if (Classes.DBConnection.EditAccountInDB(OldName, LoginString, Classes.Encoding.CreateHashFromSecureString(SecurePassword)))
                        {
                            MessageBox.Show($"Пароль пользователя {OldName} успешно изменен");
                            IsAccountRenamed = true;
                            OnRequestClose(this, new EventArgs());
                        }
                    }
                }
                else if (SecurePassword == null)
                {
                    if (Classes.Validating.CheckLogin(LoginString))
                    {
                        if (Classes.DBConnection.EditAccountInDB(OldName, LoginString))
                        {
                            MessageBox.Show($"Пользователь {OldName} успешно переименован в {LoginString}");
                            IsAccountRenamed = true;
                            OnRequestClose(this, new EventArgs());
                        }
                    }
                }
            }

        }
        private bool _isAccountCreated;
        public bool IsAccountCreated
        {
            get { return _isAccountCreated; }
            set { _isAccountCreated = value; }
        }

        private Models.Account _newAccount;
        public Models.Account NewAccount
        {
            get { return _newAccount; }
            set { _newAccount = value; }
        }

        private readonly Classes.Command _cancelCommand;
        public Classes.Command CancelCommand
        {
            get { return _cancelCommand; }
        }
        private void Cancel(object state)
        {
            OnRequestClose(this, new EventArgs());
        }

        private string _oldName;
        public string OldName
        {
            get { return _oldName; }
            set { _oldName = value; }
        }

        private bool _isAccountRenamed;
        public bool IsAccountRenamed
        {
            get { return _isAccountRenamed; }
            set { _isAccountRenamed = value; }
        }
    }
}
