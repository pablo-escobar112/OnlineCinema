using System.Windows;
using CinemaModule.Data;

namespace OnlineCinema.App
{
    public partial class CollectionsWindow : Window
    {
        private DatabaseHelper _db = new();
        private int _userId;

        public CollectionsWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadCollections();
        }

        private void LoadCollections()
        {
            CollectionsList.ItemsSource = _db.GetUserCollections(_userId);
        }

        private void CreateCollection_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Создание подборки", "Название:", "Описание:");
            if (dialog.ShowDialog() == true)
            {
                _db.CreateCollection(_userId, dialog.Input1, dialog.Input2);
                LoadCollections();
            }
        }

        private void OpenCollection_Click(object sender, RoutedEventArgs e)
        {
            int collectionId = (int)((System.Windows.Controls.Button)sender).Tag;
            var window = new CollectionDetailWindow(collectionId, _userId);
            window.Owner = this;
            window.ShowDialog();
            LoadCollections();
        }

        private void DeleteCollection_Click(object sender, RoutedEventArgs e)
        {
            int collectionId = (int)((System.Windows.Controls.Button)sender).Tag;
            if (MessageBox.Show("Удалить подборку?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _db.DeleteCollection(collectionId);
                LoadCollections();
            }
        }
    }
}