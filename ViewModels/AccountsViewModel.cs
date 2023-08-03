using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PhoneBook.ViewModels
{
    class AccountsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event EventHandler OnRequestClose;
        public AccountsViewModel()
        {
            Accounts = Classes.DBConnection.SelectAccountsFromDB();
            _accountsView = CollectionViewSource.GetDefaultView(Accounts);
            this._createAccountCommand = new Classes.Command(CreateAccount);
            this._editAccountCommand = new Classes.Command(this.EditAccount, this.CanOpenEditWindow);
            this._deleteAccountCommand = new Classes.Command(this.DeleteAccount, this.CanDeleteAccount);
            this._confirmAndCloseCommand = new Classes.Command(ConfirmAndClose);
            this._cancelAndCloseCommand = new Classes.Command(CancelAndClose);
        }

        private ObservableCollection<Models.Account> _accounts;
        public ObservableCollection<Models.Account> Accounts
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        private Models.Account _selectedAccount;
        public Models.Account SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged("SelectedAccount");
            }
        }

        private readonly Classes.Command _createAccountCommand;
        public Classes.Command CreateAccountCommand
        {
            get { return _createAccountCommand; }
        }
        private void CreateAccount(object state)
        {
            var CreateEditAccount = new Windows.CreateEditAccount("Создание аккаунта");
            CreateEditAccount.Owner = Application.Current.MainWindow;
            CreateEditAccount.ShowDialog();
            var CEAVM = CreateEditAccount.DataContext as ViewModels.CreateEditAccountViewModel;
            if (CEAVM.IsAccountCreated)
            {
                Accounts.Add(CEAVM.NewAccount);
                _accountsView.Refresh();
            }
        }

        private readonly Classes.Command _editAccountCommand;
        public Classes.Command EditAccountCommand
        {
            get { return _editAccountCommand; }
        }
        private bool CanOpenEditWindow(object state)
        {
            if (_selectedAccount == null) { return false; }
            else { return true; }
        }
        private void EditAccount(object state)
        {
            var CreateEditAccount = new Windows.CreateEditAccount("Редактирование аккаунта");
            CreateEditAccount.Owner = Application.Current.MainWindow;
            var CEAVM = CreateEditAccount.DataContext as ViewModels.CreateEditAccountViewModel;
            CEAVM.LoginString = SelectedAccount.Username;
            CEAVM.OldName = SelectedAccount.Username;
            CreateEditAccount.ShowDialog();
            if (CEAVM.IsAccountRenamed)
            {
                Accounts = new ObservableCollection<Models.Account>(Accounts.Where(i => i.Username == CEAVM.OldName).Select(j => { j.Username = CEAVM.LoginString; return j; }));
                _accountsView.Refresh();
            }
        }

        private readonly Classes.Command _deleteAccountCommand;
        public Classes.Command DeleteAccountCommand
        {
            get { return _deleteAccountCommand; }
        }
        private bool CanDeleteAccount(object state)
        {
            if (_selectedAccount == null) { return false; }
            else { return true; }
        }
        private void DeleteAccount(object state)
        {
            var result = MessageBox.Show($"Вы уверены, что хотите удалить аккаунт {SelectedAccount.Username}?", "Аккаунты", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (Classes.DBConnection.DeleteAccountFromDB(SelectedAccount.Username))
                {
                    MessageBox.Show($"Аккаунт {SelectedAccount.Username} успешно удален", "Аккаунты", MessageBoxButton.OK, MessageBoxImage.Information);
                    Accounts.Remove(SelectedAccount);
                    _accountsView.Refresh();
                }
            }
        }

        private ICollectionView _accountsView;
        public ICollectionView AccountsView
        {
            get { return _accountsView; }
        }

        private Classes.Command _confirmAndCloseCommand;
        public Classes.Command ConfirmAndCloseCommand
        {
            get { return _confirmAndCloseCommand; }
        }
        public void ConfirmAndClose(object state)
        {
            OnRequestClose(this, new EventArgs());
        }

        private Classes.Command _cancelAndCloseCommand;
        public Classes.Command CancelAndCloseCommand
        {
            get { return _cancelAndCloseCommand; }
        }
        public void CancelAndClose(object state)
        {
            OnRequestClose(this, new EventArgs());
        }
    }
}
