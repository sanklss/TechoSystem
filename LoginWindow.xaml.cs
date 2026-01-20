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
using TechnoSystem.Models;
using TechnoSystem.Services;

namespace TechnoSystem
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AuthService _authService;
        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void Guest_click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(null);
            mainWindow.Show();
            this.Close();
        }

        private void Enter_click(object sender, RoutedEventArgs e)
        {
            if (!ValidateData())
            {
                return;
            }

            Role role = _authService.TryAuth(LoginTextBox.Text, PasswordTextBox.Text);
            if (role != null)
            {
                var mainWindow = new MainWindow(role);
                mainWindow.Show();
                this.Close();
            }
        }

        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Text ))
            {
                MessageBox.Show($"Поле 'Логин' или 'Пароль' не может быть пустым!", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
