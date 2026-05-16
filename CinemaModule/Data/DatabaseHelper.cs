// CinemaModule/Data/DatabaseHelper.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using CinemaModule.Models;

namespace CinemaModule.Data
{
    /// <summary>
    /// Предоставляет методы для работы с базой данных онлайн-кинотеатра.
    /// </summary>
    public class DatabaseHelper
    {
        private const string ConnectionString =
            "Server=172.16.1.101,33678;Database=OnlineCinemaDB;User Id=Drachev;Password=j_KQrG;TrustServerCertificate=True;";

        /// <summary>
        /// Получает список всех фильмов с названиями жанров.
        /// </summary>
        public List<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();
            string query = @"SELECT m.Id, m.Title, m.GenreId, g.Name, m.Year, m.Duration, m.AgeRating, m.Description, m.PosterPath
                     FROM Movies m JOIN Genres g ON m.GenreId = g.Id";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        movies.Add(new Movie
                        {
                            Id = r.GetInt32(0),
                            Title = r.GetString(1),
                            GenreId = r.GetInt32(2),
                            GenreName = r.GetString(3),
                            Year = r.GetInt32(4),
                            Duration = r.GetInt32(5),
                            AgeRating = r.GetInt32(6),
                            Description = r.IsDBNull(7) ? "" : r.GetString(7),
                            PosterPath = r.IsDBNull(8) ? "" : r.GetString(8)   // ← ВОТ ЭТА СТРОКА важна!
                        });
            }
            return movies;
        }

        /// <summary>
        /// Получает список всех жанров.
        /// </summary>
        public List<Genre> GetAllGenres()
        {
            var list = new List<Genre>();
            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("SELECT Id, Name FROM Genres ORDER BY Name", c))
            {
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(new Genre { Id = r.GetInt32(0), Name = r.GetString(1) });
            }
            return list;
        }

        /// <summary>
        /// Получает избранные фильмы пользователя.
        /// </summary>
        /// <summary>
        /// Получает избранные фильмы пользователя.
        /// </summary>
        public List<Movie> GetFavorites(int userId)
        {
            var movies = new List<Movie>();
            string query = @"SELECT m.Id, m.Title, m.GenreId, g.Name, m.Year, m.Duration, m.AgeRating, m.Description, m.PosterPath
                     FROM Favorites f 
                     JOIN Movies m ON f.MovieId = m.Id
                     JOIN Genres g ON m.GenreId = g.Id 
                     WHERE f.UserId = @uid 
                     ORDER BY f.AddedDate DESC";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        movies.Add(new Movie
                        {
                            Id = r.GetInt32(0),
                            Title = r.GetString(1),
                            GenreId = r.GetInt32(2),
                            GenreName = r.GetString(3),
                            Year = r.GetInt32(4),
                            Duration = r.GetInt32(5),
                            AgeRating = r.GetInt32(6),
                            Description = r.IsDBNull(7) ? "" : r.GetString(7),
                            PosterPath = r.IsDBNull(8) ? "" : r.GetString(8)   // ← ВОТ ЭТО ДОБАВЛЕНО
                        });
            }
            return movies;
        }

        /// <summary>
        /// Добавляет фильм в избранное пользователя.
        /// </summary>
        public bool AddToFavorites(int userId, int movieId)
        {
            string query = @"IF NOT EXISTS (SELECT 1 FROM Favorites WHERE UserId = @uid AND MovieId = @mid)
                             BEGIN
                                 INSERT INTO Favorites (UserId, MovieId) VALUES (@uid, @mid);
                                 SELECT 1;
                             END
                             ELSE
                                 SELECT 0;";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@mid", movieId);
                c.Open();
                return (int)cmd.ExecuteScalar() == 1;
            }
        }

        /// <summary>
        /// Удаляет фильм из избранного пользователя.
        /// </summary>
        public bool RemoveFromFavorites(int userId, int movieId)
        {
            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(
                "DELETE FROM Favorites WHERE UserId = @uid AND MovieId = @mid", c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@mid", movieId);
                c.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Получает историю просмотров пользователя.
        /// </summary>
        public List<ViewHistory> GetViewHistory(int userId)
        {
            var list = new List<ViewHistory>();
            string query = @"SELECT m.Title, g.Name, v.ViewDate 
                             FROM Views v
                             JOIN Movies m ON v.MovieId = m.Id 
                             JOIN Genres g ON m.GenreId = g.Id
                             WHERE v.UserId = @uid 
                             ORDER BY v.ViewDate DESC";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(new ViewHistory
                        {
                            MovieTitle = r.GetString(0),
                            GenreName = r.GetString(1),
                            ViewDate = r.GetDateTime(2)
                        });
            }
            return list;
        }

        /// <summary>
        /// Добавляет запись о просмотре фильма.
        /// </summary>
        public void AddView(int userId, int movieId)
        {
            string query = "INSERT INTO Views (UserId, MovieId) VALUES (@UserId, @MovieId)";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@MovieId", movieId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Получает список ID жанров просмотренных фильмов (для рекомендаций).
        /// </summary>
        public List<int> GetWatchedGenreIds(int userId)
        {
            var list = new List<int>();
            string query = @"SELECT m.GenreId FROM Views v 
                             JOIN Movies m ON v.MovieId = m.Id 
                             WHERE v.UserId = @uid";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(r.GetInt32(0));
            }
            return list;
        }

        /// <summary>
        /// Получает информацию о текущей активной подписке пользователя.
        /// </summary>
        public UserSubscriptionInfo? GetUserSubscription(int userId)
        {
            string query = @"SELECT TOP 1 us.EndDate, s.Name, s.Price
                             FROM UserSubscriptions us 
                             JOIN Subscriptions s ON us.SubscriptionId = s.Id
                             WHERE us.UserId = @uid AND us.IsActive = 1 
                             ORDER BY us.EndDate DESC";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                c.Open();
                using (var r = cmd.ExecuteReader())
                    if (r.Read())
                        return new UserSubscriptionInfo
                        {
                            EndDate = r.GetDateTime(0),
                            TariffName = r.GetString(1),
                            Price = r.GetDecimal(2)
                        };
            }
            return null;
        }

        /// <summary>
        /// Получает все доступные тарифы подписок.
        /// </summary>
        public List<Subscription> GetAllSubscriptions()
        {
            var list = new List<Subscription>();
            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(
                "SELECT Id, Name, Price, DurationDays FROM Subscriptions", c))
            {
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(new Subscription
                        {
                            Id = r.GetInt32(0),
                            Name = r.GetString(1),
                            Price = r.GetDecimal(2),
                            DurationDays = r.GetInt32(3)
                        });
            }
            return list;
        }

        /// <summary>
        /// Активирует/продлевает подписку пользователя.
        /// </summary>
        public void ActivateSubscription(int userId, int subId)
        {
            using (var c = new SqlConnection(ConnectionString))
            {
                c.Open();
                using (var t = c.BeginTransaction())
                {
                    new SqlCommand(
                        "UPDATE UserSubscriptions SET IsActive = 0 WHERE UserId = " + userId,
                        c, t).ExecuteNonQuery();

                    int days = (int)new SqlCommand(
                        "SELECT DurationDays FROM Subscriptions WHERE Id = " + subId,
                        c, t).ExecuteScalar();

                    new SqlCommand(
                        $"INSERT INTO UserSubscriptions (UserId, SubscriptionId, StartDate, EndDate, IsActive) " +
                        $"VALUES ({userId}, {subId}, GETDATE(), DATEADD(DAY, {days}, GETDATE()), 1)",
                        c, t).ExecuteNonQuery();

                    t.Commit();
                }
            }
        }

        /// <summary>
        /// Создаёт новую подборку и возвращает её ID.
        /// </summary>
        public int CreateCollection(int userId, string name, string description)
        {
            string query = @"INSERT INTO MovieCollections (UserId, Name, Description) 
                     VALUES (@uid, @name, @desc); SELECT SCOPE_IDENTITY();";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", description);
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Получает все подборки пользователя с количеством фильмов.
        /// </summary>
        public List<MovieCollection> GetUserCollections(int userId)
        {
            var list = new List<MovieCollection>();
            string query = @"SELECT c.Id, c.UserId, c.Name, c.Description, c.CreatedDate,
                     (SELECT COUNT(*) FROM MovieCollectionItems WHERE CollectionId = c.Id) as MovieCount
                     FROM MovieCollections c WHERE c.UserId = @uid ORDER BY c.CreatedDate DESC";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(new MovieCollection
                        {
                            Id = r.GetInt32(0),
                            UserId = r.GetInt32(1),
                            Name = r.GetString(2),
                            Description = r.IsDBNull(3) ? "" : r.GetString(3),
                            CreatedDate = r.GetDateTime(4),
                            MovieCount = r.GetInt32(5)
                        });
            }
            return list;
        }

        /// <summary>
        /// Получает фильмы из подборки.
        /// </summary>
        public List<MovieCollectionItem> GetCollectionItems(int collectionId)
        {
            var list = new List<MovieCollectionItem>();
            string query = @"SELECT ci.Id, ci.CollectionId, ci.MovieId, m.Title, g.Name, ci.AddedDate
                     FROM MovieCollectionItems ci
                     JOIN Movies m ON ci.MovieId = m.Id
                     JOIN Genres g ON m.GenreId = g.Id
                     WHERE ci.CollectionId = @cid ORDER BY ci.AddedDate DESC";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@cid", collectionId);
                c.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read())
                        list.Add(new MovieCollectionItem
                        {
                            Id = r.GetInt32(0),
                            CollectionId = r.GetInt32(1),
                            MovieId = r.GetInt32(2),
                            MovieTitle = r.GetString(3),
                            GenreName = r.GetString(4),
                            AddedDate = r.GetDateTime(5)
                        });
            }
            return list;
        }

        /// <summary>
        /// Добавляет фильм в подборку.
        /// </summary>
        public bool AddMovieToCollection(int collectionId, int movieId)
        {
            string query = @"IF NOT EXISTS (SELECT 1 FROM MovieCollectionItems WHERE CollectionId=@cid AND MovieId=@mid)
                     BEGIN INSERT INTO MovieCollectionItems (CollectionId, MovieId) VALUES (@cid, @mid); SELECT 1; END
                     ELSE SELECT 0;";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@cid", collectionId);
                cmd.Parameters.AddWithValue("@mid", movieId);
                c.Open();
                return (int)cmd.ExecuteScalar() == 1;
            }
        }

        /// <summary>
        /// Удаляет фильм из подборки.
        /// </summary>
        public bool RemoveMovieFromCollection(int collectionId, int movieId)
        {
            string query = "DELETE FROM MovieCollectionItems WHERE CollectionId=@cid AND MovieId=@mid";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@cid", collectionId);
                cmd.Parameters.AddWithValue("@mid", movieId);
                c.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Удаляет подборку целиком.
        /// </summary>
        public bool DeleteCollection(int collectionId)
        {
            string query = "DELETE FROM MovieCollections WHERE Id=@id";

            using (var c = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, c))
            {
                cmd.Parameters.AddWithValue("@id", collectionId);
                c.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}