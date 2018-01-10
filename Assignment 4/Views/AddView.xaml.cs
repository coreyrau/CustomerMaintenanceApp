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
using Assignment_4.ViewModel;

namespace Assignment_4.Views
{
    /// <summary>
    /// Interaction logic for AddView.xaml
    /// </summary>
    public partial class AddView : Window, IClosable
    {
        public AddView()
        {
            InitializeComponent();
        }

        private void StatesComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
