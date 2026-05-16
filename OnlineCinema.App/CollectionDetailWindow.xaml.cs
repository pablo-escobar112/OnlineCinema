using System.Linq;
using System.Windows;
using CinemaModule.Data;

namespace OnlineCinema.App
{
    public partial class CollectionDetailWindow : Window
    {
        private DatabaseHelper _db = new();
        private int _collectionId;
        private int _userId;

        public CollectionDetailWindow(int collectionId, int userId)
        {
            InitializeComponent();
            _collectionId = collectionId;
            _userId = userId;
            LoadItems();
        }

        private void LoadItems()
        {
            var items = _db.GetCollectionItems(_collectionId);
            ItemsList.ItemsSource = items;
            MovieCountText.Text = $"Фильмов в подборке: {items.Count}";
        }

        private void AddMovie_Click(object sender, RoutedEventArgs e)
        {
            var movies = _db.GetAllMovies();
            var items = _db.GetCollectionItems(_collectionId);
            var existingIds = items.Select(i => i.MovieId).ToHashSet();
            var available = movies.Where(m => !existingIds.Contains(m.Id)).ToList();

            if (available.Any())
            {
                var dialog = new MovieSelectDialog(available);
                dialog.Owner = this;
                if (dialog.ShowDialog() == true && dialog.SelectedMovie != null)
                {
                    _db.AddMovieToCollection(_collectionId, dialog.SelectedMovie.Id);
                    LoadItems();
                }
            }
            else
            {
                MessageBox.Show("Все фильмы уже в подборке.");
            }
        }

        private void RemoveMovie_Click(object sender, RoutedEventArgs e)
        {
            int itemId = (int)((System.Windows.Controls.Button)sender).Tag;
            var items = _db.GetCollectionItems(_collectionId);
            var item = items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _db.RemoveMovieFromCollection(_collectionId, item.MovieId);
                LoadItems();
            }
        }
    }
}