using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBook.Windows
{
    /// <summary>
    /// Логика взаимодействия для ExportPhonebook.xaml
    /// </summary>
    public partial class ExportPhonebook : Window
    {
        public ExportPhonebook()
        {
            InitializeComponent();
            var dataContext = new ViewModels.ExportPhonebookViewModel();
            DataContext = dataContext;
            dataContext.OnRequestClose += (s, e) => this.Close();
        }
    }
}
