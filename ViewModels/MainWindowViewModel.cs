using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PhoneBook.Classes;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Threading;
using ModernWpf.Controls;

namespace PhoneBook.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        BackgroundWorker worker;
        public MainWindowViewModel()
        {
            //SelectedIndex = -1;
            this._openSettingsWindowCommand = new Classes.Command(this.OpenSettingsWindow);
            //this._keyDownBindingCommand = new Classes.Command(this.KeyDownBinding);
            //this._keyUpBindingCommand = new Classes.Command(this.KeyUpBinding);
            //this._keyReturnBindingCommand = new Classes.Command(this.KeyReturnBinding);
            //this._previewMouseUpEventCommand = new Classes.Command(this.PreviewMouseUpEvent);
            Employees = DBConnection.SelectEmployeesFromDB();

            //UpdateText = false;
            FilteredEmployees = _fullNames;
            this._submitSearchCommand = new Classes.Command(this.SubmitSearch);

            _employeesView = CollectionViewSource.GetDefaultView(Employees);
            _employeesView.GroupDescriptions.Add(new PropertyGroupDescription("Department"));
            //EmployeesView.Filter = i => String.IsNullOrEmpty(SelectedItem) ? true : ((Models.Employee)i).FullName.Contains(SelectedItem)
            //|| ((Models.Employee)i).Title.Contains(SelectedItem) || ((Models.Employee)i).Department.Contains(SelectedItem);
            EmployeesView.SortDescriptions.Add(new SortDescription("Department", ListSortDirection.Ascending));
            EmployeesView.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));
            this._exportPhonebookCommand = new Classes.Command(this.ExportPhonebook);
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(10000);
                App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                Refresh()));
            }
        }

        private void Refresh()
        {
            try
            {
                var updatedEmployees = Classes.DBConnection.SelectEmployeesFromDB();
                var empolyeeMatcher = new Func<Models.Employee, Models.Employee, bool>(
                    (target, update) => target.Id == update.Id);
                var employeeUpdater = new Action<Models.Employee, Models.Employee>((target, update) =>
                {
                    target.Id = update.Id;
                    target.City = update.City;
                    target.Department = update.Department;
                    target.FullName = update.FullName;
                    target.IpPhoneNumber = update.IpPhoneNumber;
                    target.Mail = update.Mail;
                    target.OfficeNumber = update.OfficeNumber;
                    target.Street = update.Street;
                    target.TelephoneNumber = update.TelephoneNumber;
                    target.Title = update.Title;
                });
                Employees.UpdateItems(updatedEmployees, empolyeeMatcher, employeeUpdater);
            }
            catch (Exception e) { }
        }
        private ICollectionView _employeesView;
        public ICollectionView EmployeesView
        {
            get { return _employeesView; }
        }

        private readonly Classes.Command _openSettingsWindowCommand;
        public Classes.Command OpenSettingsWindowCommand
        {
            get { return this._openSettingsWindowCommand; }
        }
        private void OpenSettingsWindow(object state)
        {
            var Login = new Windows.Login();
            Login.Owner = Application.Current.MainWindow;
            Login.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Login.ShowDialog();
            var loginViewModel = Login.DataContext as ViewModels.LoginViewModel;
            if (loginViewModel.Authenticated)
            {
                var Settings = new Windows.Settings();
                Settings.Owner = Application.Current.MainWindow;
                Settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                Settings.ShowDialog();
                var SVM = Settings.DataContext as ViewModels.SettingsViewModel;
                if (SVM.PhonebookSynchronized)
                {
                    Employees.Clear();
                    foreach (var i in Classes.DBConnection.SelectEmployeesFromDB())
                    {
                        Employees.Add(i);
                    }
                }
            }
        }
        //
        //
        //

        //private readonly Classes.Command _keyDownBindingCommand;
        //public Classes.Command KeyDownBindingCommand
        //{
        //    get { return this._keyDownBindingCommand; }
        //}
        //private void KeyDownBinding(object state)
        //{
        //    if (FilteredFullNames != null && SelectedIndex < FilteredFullNames.Count() - 1)
        //    {
        //        SelectedIndex = SelectedIndex + 1;
        //    }
        //    else
        //    {
        //        if (FilteredFullNames != null && SelectedIndex == FilteredFullNames.Count() - 1)
        //        {
        //            SelectedIndex = 0;
        //        }
        //    }
        //}

        //
        //
        //

        private ViewModels.SettingsViewModel CurrentSettingsViewModel { get; set; }

        private ObservableCollection<Models.Employee> _employees;
        public ObservableCollection<Models.Employee> Employees
        {
            get { return _employees; }
            set
            {
                _fullNames = value.Select(x => x.FullName).ToList();
                ///
                _fullNames.AddRange(value.Select(x => x.Title).Distinct().ToList());
                _fullNames.AddRange(value.Select(x => x.Department).Distinct().ToList());
                ///
                _employees = value;
            }
        }
        private List<string> _fullNames { get; set; }

        //private string _searchString;
        //public string SearchString
        //{
        //    get { return _searchString; }
        //    set
        //    {
        //        _searchString = value;
        //        ////PopupIsOpen = true;
        //        ////if (!String.IsNullOrEmpty(value)) { PopupIsOpen = true; }
        //        //if (!(FilteredFullNames == null) && FilteredFullNames.Count() > 0) { PopupIsOpen = true; }
        //        //else { PopupIsOpen = false; }
        //        if (String.IsNullOrEmpty(value)) { SelectedItem = null; SelectedIndex = -1; EmployeesView.Refresh(); }
        //        OnPropertyChanged("SearchString");
        //        OnPropertyChanged("FilteredFullNames");
        //    }
        //}
        //public IEnumerable<string> FilteredFullNames
        //{
        //    //get
        //    //{
        //    //    if (SearchString == null) return null;
        //    //    //if (!String.IsNullOrEmpty(SelectedItem)) return null;
        //    //    if (Employees != null && !String.IsNullOrEmpty(SearchString)) { /*PopupIsOpen = true;*/ return _fullNames.Where(x => x.ToUpper().StartsWith(SearchString.ToUpper())); }
        //    //    else { return null; }
        //    //}
        //    get
        //    {
        //        if (Employees != null && !String.IsNullOrEmpty(SearchString))
        //        {
        //            //if (_fullNames.Where(x => x.ToUpper().StartsWith(SearchString.ToUpper())).Count() > 0)
        //            if (_fullNames.Where(x => x.ToUpper().Contains(SearchString.ToUpper())).Count() > 0)
        //            {
        //                PopupIsOpen = true;
        //                //return _fullNames.Where(x => x.ToUpper().StartsWith(SearchString.ToUpper()));
        //                return _fullNames.Where(x => x.ToUpper().Contains(SearchString.ToUpper()));
        //            }
        //        }
        //        PopupIsOpen = false;
        //        return null;
        //    }
        //}
        //private string _selectedItem;
        //public string SelectedItem
        //{
        //    get
        //    {
        //        return _selectedItem;
        //    }
        //    set
        //    {
        //        if (value != _selectedItem)
        //        {
        //            _selectedItem = value;
        //            //MessageBox.Show($"Selected item - {SelectedItem} and current text in search textbox - {SearchString}");
        //            SearchString = _selectedItem;
        //            //ContentVisibility = Visibility.Hidden;
        //            PopupIsOpen = false;
        //            _employeesView.Refresh();
        //        }
        //        //_selectedItem = value;
        //        //MessageBox.Show($"Selected item - {SelectedItem} and current text in search textbox - {SearchString}");
        //        //SearchString = _selectedItem;
        //        //OnPropertyChanged("SelectedItem");
        //    }
        //}
        //private string SelectedItem;

        //private bool _popupIsOpen;
        //public bool PopupIsOpen
        //{
        //    get
        //    {
        //        return _popupIsOpen;
        //    }
        //    set
        //    {
        //        _popupIsOpen = value;
        //        OnPropertyChanged("PopupIsOpen");
        //    }
        //}

        private readonly Classes.Command _exportPhonebookCommand;
        public Classes.Command ExportPhonebookCommand
        {
            get { return _exportPhonebookCommand; }
        }
        private void ExportPhonebook(object state)
        {
            var Export = new Windows.ExportPhonebook();
            Export.Owner = Application.Current.MainWindow;
            Export.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Export.ShowDialog();
        }

        //private int _selectedIndex;
        //public int SelectedIndex
        //{
        //    get { return _selectedIndex; }
        //    set
        //    {
        //        _selectedIndex = value;
        //        OnPropertyChanged("SelectedIndex");
        //    }
        //}

        //private readonly Classes.Command _keyUpBindingCommand;
        //public Classes.Command KeyUpBindingCommand
        //{
        //    get { return this._keyUpBindingCommand; }
        //}
        //private void KeyUpBinding(object state)
        //{
        //    if (SelectedIndex != 0 && FilteredFullNames != null && SelectedIndex < FilteredFullNames.Count())
        //    {
        //        SelectedIndex = SelectedIndex - 1;
        //    }
        //    else
        //    {
        //        if (FilteredFullNames != null && SelectedIndex == 0)
        //        {
        //            SelectedIndex = FilteredFullNames.Count() - 1;
        //        }
        //    }
        //}

        //private readonly Classes.Command _keyReturnBindingCommand;
        //public Classes.Command KeyReturnBindingCommand
        //{
        //    get { return this._keyReturnBindingCommand; }
        //}
        //private void KeyReturnBinding(object state)
        //{
        //    if (FilteredFullNames != null && SelectedIndex != -1)
        //    {
        //        SearchString = FilteredFullNames.ToList()[SelectedIndex];
        //        ///
        //        SelectedItem = FilteredFullNames.ToList()[SelectedIndex];
        //        ///
        //        _employeesView.Refresh();
        //        PopupIsOpen = false;
        //    }
        //}

        //private readonly Classes.Command _previewMouseUpEventCommand;
        //public Classes.Command PreviewMouseUpEventCommand
        //{
        //    get { return this._previewMouseUpEventCommand; }
        //}
        //private void PreviewMouseUpEvent(object state)
        //{
        //    SearchString = FilteredFullNames.ToList()[SelectedIndex];
        //    ///
        //    SelectedItem = FilteredFullNames.ToList()[SelectedIndex];
        //    ///
        //    _employeesView.Refresh();
        //    PopupIsOpen = false;
        //}
        //private void InitializeView(ObservableCollection<Models.Employee> employees)
        //{
        //    _employeesView = CollectionViewSource.GetDefaultView(employees);
        //    _employeesView.GroupDescriptions.Add(new PropertyGroupDescription("Department"));
        //    EmployeesView.Filter = i => String.IsNullOrEmpty(SelectedItem) ? true : ((Models.Employee)i).FullName.Contains(SelectedItem)
        //    || ((Models.Employee)i).Title.Contains(SelectedItem) || ((Models.Employee)i).Department.Contains(SelectedItem);
        //    EmployeesView.SortDescriptions.Add(new SortDescription("Department", ListSortDirection.Ascending));
        //    EmployeesView.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));
        //    _employeesView.Refresh();

        //}
        //private bool _updateText;
        //public bool UpdateText
        //{
        //    get { return _updateText; }
        //    set
        //    {
        //        _updateText = value;
        //        OnPropertyChanged("UpdateText");
        //    }
        //}

        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged("SearchString");
            }
        }
        private List<string> _filteredEmployees;
        public List<string> FilteredEmployees
        {
            get { return _filteredEmployees; }
            set
            {
                _filteredEmployees = value;
                OnPropertyChanged("FilteredEmployees");
            }
        }

        private readonly Classes.Command _submitSearchCommand;
        public Classes.Command SubmitSearchCommand
        {
            get { return _submitSearchCommand; }
        }
        private void SubmitSearch(object state)
        {
            EmployeesView.Filter = i => String.IsNullOrEmpty(SearchString) ? true : ((Models.Employee)i).FullName.ToLower().Contains(SearchString.ToLower())
            || ((Models.Employee)i).Title.ToLower().Contains(SearchString.ToLower()) || ((Models.Employee)i).Department.ToLower().Contains(SearchString.ToLower());
            _employeesView.Refresh();
        }

        public void TextChangedMethod(object sender, AutoSuggestBoxTextChangedEventArgs a)
        {
            if (a.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                FilteredEmployees = _fullNames;
                var result = FilteredEmployees.Where(x => x.ToLower().Contains(SearchString.ToLower())).ToList();
                if (result.Count == 0) { FilteredEmployees = new List<string> { "Ничего не найдено" }; }
                else FilteredEmployees = result;
            }
        }
    }
}
