// OnlineCinema.App/SubscriptionWindow.xaml.cs
using System.Linq;
using System.Windows;
using CinemaModule.Data;
using CinemaModule.Services;

namespace OnlineCinema.App
{
    public partial class SubscriptionWindow : Window
    {
        private DatabaseHelper _db = new();
        private CinemaService _cs = new();
        private int _userId;

        public SubscriptionWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadSubscriptionData();
        }

        /// <summary>
        /// Загружает данные о текущей подписке и доступных тарифах.
        /// </summary>
        private void LoadSubscriptionData()
        {
            // Текущая подписка
            var sub = _db.GetUserSubscription(_userId);
            if (sub != null)
            {
                bool active = _cs.IsSubscriptionActive(sub.EndDate);
                int days = _cs.GetRemainingDays(sub.EndDate);

                CurrentSubInfo.Text = $"Тариф: {sub.TariffName} ({sub.Price:F2} ₽)";
                StatusInfo.Text = active ? "✅ Активна" : "❌ Истекла";
                StatusInfo.Foreground = active
                    ? System.Windows.Media.Brushes.Green
                    : System.Windows.Media.Brushes.Red;
                DaysInfo.Text = active
                    ? $"Осталось дней: {days}"
                    : "Подписка неактивна. Оформите новую.";
            }
            else
            {
                CurrentSubInfo.Text = "У вас нет активной подписки.";
                StatusInfo.Text = "❌ Нет подписки";
                StatusInfo.Foreground = System.Windows.Media.Brushes.Red;
                DaysInfo.Text = "Оформите один из тарифов ниже.";
            }

            // Доступные тарифы
            TariffsControl.ItemsSource = _db.GetAllSubscriptions();
        }

        /// <summary>
        /// Обработчик оформления подписки.
        /// </summary>
        private void ActivateSubscription_Click(object sender, RoutedEventArgs e)
        {
            int subId = (int)((System.Windows.Controls.Button)sender).Tag;

            var result = MessageBox.Show(
                "Вы уверены, что хотите оформить этот тариф?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _db.ActivateSubscription(_userId, subId);
                    MessageBox.Show("Подписка успешно оформлена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadSubscriptionData(); // Обновляем данные
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Закрытие окна.
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}