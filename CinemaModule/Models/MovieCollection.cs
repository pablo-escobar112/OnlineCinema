// CinemaModule/Models/MovieCollection.cs
using System;

namespace CinemaModule.Models
{
    /// <summary>
    /// Представляет персональную подборку фильмов пользователя.
    /// </summary>
    public class MovieCollection
    {
        /// <summary>
        /// Уникальный идентификатор подборки.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя-владельца.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Название подборки.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание подборки.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания подборки.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Количество фильмов в подборке.
        /// </summary>
        public int MovieCount { get; set; }
    }
}