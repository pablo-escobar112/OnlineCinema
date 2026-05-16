using System.Collections.Generic;
using System.Windows;
using CinemaModule.Models;

namespace OnlineCinema.App
{
    public partial class MovieSelectDialog : Window
    {
        public Movie SelectedMovie { get; private set; }

        public MovieSelectDialog(List<Movie> movies)
        {
            InitializeComponent();
            MoviesListBox.ItemsSource = movies;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            SelectedMovie = MoviesListBox.SelectedItem as Movie;
            DialogResult = SelectedMovie != null;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}