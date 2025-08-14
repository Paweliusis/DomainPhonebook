using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PhoneBook.ViewModels
{
    class JobTitleSortViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event EventHandler OnRequestClose;
        public JobTitleSortViewModel()
        {
            //Commands section
            this._updateJobTitlesCommand = new Classes.Command(UpdateJobTitles);
            this._saveChangesCommand = new Classes.Command(SaveChanges);
            this._goBackCommand = new Classes.Command(GoBack);
            //Commands section
            JobTitles = Classes.DBConnection.SelectJobsRulesFromDB();
            jobTitlesBeforeChanges = Classes.DBConnection.SelectJobsRulesFromDB().ToList();
            _jobTitlesView = CollectionViewSource.GetDefaultView(JobTitles);
            JobTitlesView.SortDescriptions.Add(new SortDescription("Weight", ListSortDirection.Ascending));

            var worker = new BackgroundWorker();
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
            JobTitlesView.Refresh();
        }

        private ObservableCollection<Models.JobTitle> _jobTitles;
        public ObservableCollection<Models.JobTitle> JobTitles
        {
            get { return _jobTitles; }
            set
            {
                _jobTitles = value;
            }
        }

        private ICollectionView _jobTitlesView;
        public ICollectionView JobTitlesView
        {
            get { return _jobTitlesView; }
        }

        private readonly Classes.Command _updateJobTitlesCommand;
        public Classes.Command UpdateJobTitlesCommand
        {
            get { return _updateJobTitlesCommand; }
        }
        private void UpdateJobTitles(object state)
        {
            foreach(var item in Classes.DBConnection.SelectMissingJobTitles())
            {
                JobTitles.Add(item);
            }
        }

        private List<Models.JobTitle> jobTitlesBeforeChanges;

        private readonly Classes.Command _saveChangesCommand;
        public Classes.Command SaveChangesCommand
        {
            get { return _saveChangesCommand; }
        }
        private void SaveChanges(object state)
        {
            //foreach (var item in JobTitles)
            //{

            //    foreach(var item2 in jobTitlesBeforeChanges)
            //    {
            //        MessageBox.Show($"Job title from window - {item.Name}, item weight - {item.Weight}");
            //        MessageBox.Show($"Job title before changes - {item2.Name}, item weight - {item2.Weight}");
            //        var eql = item.Equals(item2);
            //        if (eql)
            //        {
            //            MessageBox.Show("They equal");
            //        }
            //        else
            //        {
            //            MessageBox.Show("They not equal");
            //        }
            //    }
            //}
            var result = MessageBox.Show($"Вы уверены, что хотите сохранить изменения?", "JobSorting", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bool somethingChange = false;
                foreach (var item in JobTitles)
                {
                    var find = jobTitlesBeforeChanges.ToList().Find(x => x.Name == item.Name);
                    if (find != null)
                    {
                        //MessageBox.Show($"Found, {qwe.Name}");
                        if (item.Weight != find.Weight)
                        {
                            //MessageBox.Show($"Weight of {item.Name} not equal, from window - {item.Weight} and from DB - {qwe.Weight}");
                            Classes.DBConnection.UpdateJobTitleWeight(item);
                            somethingChange = true;
                        }
                        //else
                        //{
                        //    MessageBox.Show($"Weight equal, from window - {item.Weight} and from DB - {qwe.Weight}");
                        //}
                    }
                    else
                    {
                        //MessageBox.Show($"Title {item.Name} is new");
                        Classes.DBConnection.InsertJobTitleWeight(item);
                        somethingChange = true;
                    }
                }
                if (somethingChange) { MessageBox.Show("Изменения успешно сохранены"); }
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
