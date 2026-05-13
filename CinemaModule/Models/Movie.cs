// CinemaModule/Models/Movie.cs
namespace CinemaModule.Models
{
    /// <summary>
    /// Представляет фильм в онлайн-кинотеатре.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Уникальный идентификатор фильма.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название фильма.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор жанра.
        /// </summary>
        public int GenreId { get; set; }

        /// <summary>
        /// Название жанра (для отображения).
        /// </summary>
        public string GenreName { get; set; } = string.Empty;

        /// <summary>
        /// Год выпуска.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Длительность в минутах.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Возрастной рейтинг (0, 6, 12, 16, 18).
        /// </summary>
        public int AgeRating { get; set; }

        /// <summary>
        /// Описание фильма.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Путь к постеру фильма.
        /// </summary>
        public string PosterPath { get; set; } = string.Empty;
    }
}