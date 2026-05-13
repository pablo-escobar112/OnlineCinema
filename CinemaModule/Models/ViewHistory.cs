// CinemaModule/Models/ViewHistory.cs
using System;

namespace CinemaModule.Models
{
    /// <summary>
    /// Представляет запись истории просмотров.
    /// </summary>
    public class ViewHistory
    {
        /// <summary>
        /// Название просмотренного фильма.
        /// </summary>
        public string MovieTitle { get; set; } = string.Empty;

        /// <summary>
        /// Название жанра.
        /// </summary>
        public string GenreName { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время просмотра.
        /// </summary>
        public DateTime ViewDate { get; set; }
    }
}