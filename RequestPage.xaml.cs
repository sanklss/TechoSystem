using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TechnoSystem.Data;
using TechnoSystem.Models;

namespace TechnoSystem
{
    public partial class RequestPage : Page
    {
        private bool _isAscendingSort = false;

        public RequestPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRequests();
            LoadStatuses();
        }

        private void LoadRequests()
        {
            try
            {
                using (var context = new Primer21Context())
                {
                    var requests = context.Requests
                        .Include(r => r.User)
                        .Include(r => r.Tarif)
                        .Include(r => r.RequestStatus)
                        .ToList();

                    RequestsItemsControl.ItemsSource = requests;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка");
            }
        }

        private void LoadStatuses()
        {
            try
            {
                using (var context = new Primer21Context())
                {
                    var statuses = context.RequestStatuses.ToList();
                    statuses.Insert(0, new RequestStatus { Id = 0, Name = "Все статусы" });
                    StatusFilterComboBox.ItemsSource = statuses;
                    StatusFilterComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            _isAscendingSort = !_isAscendingSort;
            SortButton.Content = _isAscendingSort ? "По дате ▲" : "По дате ▼";
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            try
            {
                using (var context = new Primer21Context())
                {
                    var query = context.Requests
                        .Include(r => r.User)
                        .Include(r => r.Tarif)
                        .Include(r => r.RequestStatus)
                        .AsQueryable();

                    // Поиск
                    string searchText = SearchTextBox.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        query = query.Where(r =>
                            r.User.Login.Contains(searchText) ||
                            r.Id.ToString().Contains(searchText));
                    }

                    // Фильтрация по статусу
                    var selectedStatus = StatusFilterComboBox.SelectedItem as RequestStatus;
                    if (selectedStatus != null && selectedStatus.Id != 0)
                    {
                        query = query.Where(r => r.RequestStatusId == selectedStatus.Id);
                    }

                    // Сортировка
                    query = _isAscendingSort
                        ? query.OrderBy(r => r.Date)
                        : query.OrderByDescending(r => r.Date);

                    RequestsItemsControl.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка");
            }
        }

        private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newRequest = new Request();
                var requestWindow = new RequestWindow(newRequest);

                if (requestWindow.ShowDialog() == true)
                {
                    ApplyFilters(); // Обновляем список
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания заявки: {ex.Message}", "Ошибка");
            }
        }
    }
}