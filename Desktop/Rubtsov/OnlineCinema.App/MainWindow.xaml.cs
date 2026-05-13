using System.Linq;
using System.Windows;
using CinemaModule.Data;
using CinemaModule.Services;

namespace OnlineCinema.App
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper _db = new DatabaseHelper();
        private CinemaService _cs = new CinemaService();
        private int _userId = 1;

        public MainWindow()
        {
            InitializeComponent();
            LoadGenres();
            ShowAllMovies();
        }

        private void LoadGenres()
        {
            FilterCombo.ItemsSource = _db.GetAllGenres();
            FilterCombo.DisplayMemberPath = "Name";
            FilterCombo.SelectedValuePath = "Id";
        }

        private void ShowAllMovies()
        {
            StatusText.Text = "Загрузка...";
            var movies = _db.GetAllMovies();
            DisplayMovies(movies);
            StatusText.Text = $"Фильмов: {movies.Count}";
        }

        private void ShowAll_Click(object s, RoutedEventArgs e)
        {
            FilterCombo.SelectedIndex = -1;
            ShowAllMovies();
        }

        private void FilterChanged(object s, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FilterCombo.SelectedValue is int genreId)
                DisplayMovies(_db.GetAllMovies().Where(m => m.GenreId == genreId).ToList());
        }

        private void ShowFavorites_Click(object s, RoutedEventArgs e)
        {
            var fav = _db.GetFavorites(_userId);
            DisplayMovies(fav);
            StatusText.Text = fav.Any() ? $"Избранное: {fav.Count}" : "Пусто";
        }

        private void ShowHistory_Click(object s, RoutedEventArgs e)
        {
            var history = _db.GetViewHistory(_userId);
            string msg = history.Any()
                ? string.Join("\n", history.Select(h => $"{h.MovieTitle} — {h.ViewDate:dd.MM.yyyy}"))
                : "История пуста.";
            MessageBox.Show(msg, "История");
        }

        private void ShowSubscription_Click(object s, RoutedEventArgs e)
        {
            var window = new SubscriptionWindow(_userId);
            window.Owner = this;
            window.ShowDialog();
        }

        private void ShowRecommendations_Click(object s, RoutedEventArgs e)
        {
            var genres = _db.GetWatchedGenreIds(_userId);
            int rec = _cs.GetRecommendedGenre(genres);

            if (rec > 0)
            {
                var movies = _db.GetAllMovies().Where(m => m.GenreId == rec).ToList();
                if (movies.Any())
                {
                    DisplayMovies(movies);
                    StatusText.Text = $"🎯 Рекомендации по жанру: {movies.FirstOrDefault()?.GenreName}";
                    MessageBox.Show(
                        $"Рекомендуем посмотреть фильмы жанра «{movies.FirstOrDefault()?.GenreName}»!\nНайдено фильмов: {movies.Count}",
                        "🎯 Рекомендации",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    StatusText.Text = "Нет фильмов этого жанра";
                    MessageBox.Show("К сожалению, фильмов рекомендуемого жанра нет в каталоге.",
                        "Рекомендации", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                StatusText.Text = "Недостаточно данных";
                MessageBox.Show(
                    "Недостаточно данных для рекомендаций.\n\nПосмотрите несколько фильмов, чтобы мы могли определить ваш любимый жанр!",
                    "🎯 Рекомендации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void DisplayMovies(System.Collections.Generic.List<CinemaModule.Models.Movie> movies)
        {
            MoviesPanel.Children.Clear();
            foreach (var m in movies)
                MoviesPanel.Children.Add(new Controls.FilmCard { DataContext = m });
        }
    }
}