using System.Windows;
using System.Windows.Controls;
using TechnoSystem.Models;

namespace TechnoSystem
{
    public partial class RequestCard : UserControl
    {
        public RequestCard()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is Request request)
                {
                    var requestWindow = new RequestWindow(request);
                    requestWindow.ShowDialog();

                    if (requestWindow.DialogResult == true)
                    {
                        var parent = Parent as ItemsControl;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия заявки: {ex.Message}", "Ошибка");
            }
        }
    }
}