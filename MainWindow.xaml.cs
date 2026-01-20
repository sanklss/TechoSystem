using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnoSystem.Models;

namespace TechnoSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Role _role;
        public MainWindow(Role role)
        {
            InitializeComponent();
            _role = role;
            MainFrame.Navigated += MainFrame_Navigated;
            MainFrame.Navigate(new ServicePage(role));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            
        }
    }
}