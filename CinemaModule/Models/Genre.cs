// CinemaModule/Models/Genre.cs
namespace CinemaModule.Models
{
    /// <summary>
    /// Представляет жанр фильма.
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Уникальный идентификатор жанра.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название жанра.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}