using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CinemaModule.Data;

namespace OnlineCinema.App.Controls
{
    public partial class FilmCard : UserControl
    {
        private DatabaseHelper _db = new DatabaseHelper();
        private int _userId = 1;

        public FilmCard()
        {
            InitializeComponent();
            this.MouseDoubleClick += FilmCard_MouseDoubleClick;
            this.Loaded += FilmCard_Loaded;
        }

        private void FilmCard_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CinemaModule.Models.Movie movie)
            {
                LoadImage(movie);
            }
        }

        private void LoadImage(CinemaModule.Models.Movie movie)
        {
            if (string.IsNullOrEmpty(movie.PosterPath))
            {
                ErrorText.Text = "PosterPath пустой";
                return;
            }

            // Список путей для проверки
            string[] paths = new string[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", movie.PosterPath),
                Path.Combine(Environment.CurrentDirectory, "Images", movie.PosterPath),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, movie.PosterPath),
            };

            foreach (string path in paths)
            {
                string fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        var bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.UriSource = new Uri(fullPath);
                        bmp.EndInit();
                        PosterImage.Source = bmp;
                        ErrorText.Text = ""; // Ошибок нет, скрываем текст
                        return;
                    }
                    catch (Exception ex)
                    {
                        ErrorText.Text = "Ошибка загрузки: " + ex.Message;
                        return;
                    }
                }
            }

            // Файл не найден — показываем где искали
            ErrorText.Text = "Не найден: " + movie.PosterPath;
        }

        private void FilmCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CinemaModule.Models.Movie movie)
            {
                _db.AddView(_userId, movie.Id);
                MessageBox.Show($"👁 Просмотр: {movie.Title}", "Просмотр");
            }
        }

        private void AddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            int movieId = (int)((Button)sender).Tag;
            if (_db.AddToFavorites(_userId, movieId))
                MessageBox.Show("✅ Добавлено в избранное!", "Успех");
            else
                MessageBox.Show("⚠ Уже в избранном.", "Инфо");
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CinemaModule.Models.Movie movie)
            {
                MessageBox.Show(
                    $"🎬 {movie.Title}\n\n" +
                    $"📂 Жанр: {movie.GenreName}\n" +
                    $"📅 Год: {movie.Year}\n" +
                    $"⏱ {movie.Duration} мин.\n" +
                    $"🔞 {movie.AgeRating}+\n\n" +
                    $"📝 {movie.Description}",
                    "О фильме");
            }
        }
    }
}