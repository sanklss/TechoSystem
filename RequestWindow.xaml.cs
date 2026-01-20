using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using TechnoSystem.Data;
using TechnoSystem.Models;

namespace TechnoSystem
{
    public partial class RequestWindow : Window
    {
        private Request _request;
        private bool _isEditMode = false;
        private int _originalLicenseCount = 0;

        public RequestWindow(Request request)
        {
            InitializeComponent();
            _request = request;
            LoadData();
            LoadRequest();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAvailableLicenses();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new Primer21Context())
                {
                    // Загружаем тарифы
                    var tarifs = context.Tarifs
                        .Include(t => t.Service)
                        .OrderBy(t => t.Name)
                        .ToList();
                    TarifComboBox.ItemsSource = tarifs;

                    // Загружаем пользователей
                    var users = context.Users
                        .OrderBy(u => u.Login)
                        .ToList();
                    UserComboBox.ItemsSource = users;

                    // Загружаем статусы
                    var statuses = context.RequestStatuses
                        .OrderBy(s => s.Id)
                        .ToList();
                    StatusComboBox.ItemsSource = statuses;

                    // Устанавливаем выбранные значения для редактирования
                    if (_request.Id > 0)
                    {
                        if (_request.TarifId > 0)
                        {
                            TarifComboBox.SelectedItem = tarifs.FirstOrDefault(t => t.Id == _request.TarifId);
                        }
                        if (_request.UserId > 0)
                        {
                            UserComboBox.SelectedItem = users.FirstOrDefault(u => u.Id == _request.UserId);
                        }
                        if (_request.RequestStatusId > 0)
                        {
                            StatusComboBox.SelectedItem = statuses.FirstOrDefault(s => s.Id == _request.RequestStatusId);
                        }
                    }
                    else
                    {
                        // Для новой заявки выбираем первый статус (обычно "Новая")
                        StatusComboBox.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
                Close();
            }
        }

        private void LoadRequest()
        {
            if (_request.Id > 0)
            {
                _isEditMode = true;
                _originalLicenseCount = _request.LicenseCount;

                RequestIdText.Text = _request.Id.ToString();
                DatePicker.SelectedDate = new DateTime(_request.Date.Year, _request.Date.Month, _request.Date.Day);
                LicenseQuantityTextBox.Text = _request.LicenseCount.ToString();
                CommentTextBox.Text = _request.Comment ?? "";

                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                RequestIdText.Text = "Новая заявка";
                DatePicker.SelectedDate = DateTime.Today;
                LicenseQuantityTextBox.Text = "1";
            }
        }

        private void UpdateAvailableLicenses()
        {
            if (TarifComboBox.SelectedItem is Tarif selectedTarif)
            {
                AvailableLicensesText.Text = $"Доступно: {selectedTarif.FreeLicense}";

                // Подсветка если мало лицензий
                if (selectedTarif.FreeLicense < 10)
                {
                    AvailableLicensesText.Foreground = System.Windows.Media.Brushes.Red;
                    AvailableLicensesText.FontWeight = FontWeights.Bold;
                }
                else
                {
                    AvailableLicensesText.Foreground = System.Windows.Media.Brushes.Green;
                    AvailableLicensesText.FontWeight = FontWeights.Normal;
                }
            }
        }

        private void TarifComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateAvailableLicenses();
        }

        private void LicenseQuantityTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateAvailableLicenses();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (TarifComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тариф!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(LicenseQuantityTextBox.Text, out int licenseCount) || licenseCount <= 0)
            {
                MessageBox.Show("Введите корректное количество лицензий!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedTarif = TarifComboBox.SelectedItem as Tarif;
            var selectedUser = UserComboBox.SelectedItem as User;
            var selectedStatus = StatusComboBox.SelectedItem as RequestStatus;

            try
            {
                using (var context = new Primer21Context())
                {
                    // Проверка доступности лицензий
                    var tarifFromDb = context.Tarifs.Find(selectedTarif.Id);

                    if (licenseCount > tarifFromDb.FreeLicense)
                    {
                        MessageBox.Show($"Недостаточно свободных лицензий!\n" +
                                      $"Доступно: {tarifFromDb.FreeLicense}\n" +
                                      $"Требуется: {licenseCount}",
                                      "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Request requestToSave;

                    if (_isEditMode)
                    {
                        // Редактирование существующей заявки
                        requestToSave = context.Requests.Find(_request.Id);
                        if (requestToSave == null)
                        {
                            MessageBox.Show("Заявка не найдена!", "Ошибка");
                            return;
                        }

                        // Возвращаем старые лицензии
                        if (requestToSave.TarifId == selectedTarif.Id)
                        {
                            // Тот же тариф
                            tarifFromDb.FreeLicense += _originalLicenseCount;
                        }
                        else
                        {
                            // Другой тариф - возвращаем в старый, забираем из нового
                            var oldTarif = context.Tarifs.Find(requestToSave.TarifId);
                            if (oldTarif != null)
                            {
                                oldTarif.FreeLicense += _originalLicenseCount;
                            }
                        }
                    }
                    else
                    {
                        // Новая заявка
                        requestToSave = new Request();
                        context.Requests.Add(requestToSave);
                    }

                    // Обновляем данные заявки
                    requestToSave.TarifId = selectedTarif.Id;
                    requestToSave.UserId = selectedUser.Id;
                    requestToSave.RequestStatusId = selectedStatus.Id;
                    requestToSave.Date = DateOnly.FromDateTime(DatePicker.SelectedDate ?? DateTime.Today);
                    requestToSave.LicenseCount = licenseCount;
                    requestToSave.Comment = CommentTextBox.Text;

                    // Забираем лицензии
                    tarifFromDb.FreeLicense -= licenseCount;

                    // Сохраняем изменения
                    context.SaveChanges();

                    // Обновляем ID для новой заявки
                    if (!_isEditMode)
                    {
                        _request.Id = requestToSave.Id;
                    }

                    MessageBox.Show("Заявка успешно сохранена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить заявку?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new Primer21Context())
                    {
                        var requestToDelete = context.Requests.Find(_request.Id);
                        if (requestToDelete != null)
                        {
                            // Возвращаем лицензии
                            var tarif = context.Tarifs.Find(requestToDelete.TarifId);
                            if (tarif != null)
                            {
                                tarif.FreeLicense += requestToDelete.LicenseCount;
                            }

                            context.Requests.Remove(requestToDelete);
                            context.SaveChanges();
                        }
                    }

                    MessageBox.Show("Заявка удалена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}