// CinemaModule/Models/MovieCollectionItem.cs
using System;

namespace CinemaModule.Models
{
    /// <summary>
    /// Представляет элемент подборки (связь подборки с фильмом).
    /// </summary>
    public class MovieCollectionItem
    {
        /// <summary>
        /// Уникальный идентификатор записи.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор подборки.
        /// </summary>
        public int CollectionId { get; set; }

        /// <summary>
        /// Идентификатор фильма.
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Название фильма (для отображения).
        /// </summary>
        public string MovieTitle { get; set; } = string.Empty;

        /// <summary>
        /// Жанр фильма (для отображения).
        /// </summary>
        public string GenreName { get; set; } = string.Empty;

        /// <summary>
        /// Дата добавления фильма в подборку.
        /// </summary>
        public DateTime AddedDate { get; set; }
    }
}