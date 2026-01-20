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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnoSystem.Data;
using TechnoSystem.Models;
using TechnoSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace TechnoSystem
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        private AuthService _authService;
        private Role _role;
        public ServicePage(Role role)
        {
            InitializeComponent();
            _role = role;
            _authService = new AuthService();
            LoadData();
            AdjustInterface();
        }

        private void LoadData()
        {
            using (var context = new Primer21Context())
            {
                TarifItemsControl.ItemsSource = context.Tarifs
                    .Include(p => p.Service)
                    .ToList();
            }
        }
        private void AdjustInterface()
        {
            if (_role == null)
            {
                RequestButton.Visibility = Visibility.Collapsed;
                return;
            }


            switch (_role.Id)
            {
                case 1:
                    RequestButton.Visibility = Visibility.Visible;
                    break;

                case 2:
                    RequestButton.Visibility = Visibility.Visible;
                    break;

                case 3:
                default:
                    RequestButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void RequestPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestPage());
        }
    }
}
