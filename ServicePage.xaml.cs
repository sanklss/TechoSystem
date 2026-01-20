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
    }
}
