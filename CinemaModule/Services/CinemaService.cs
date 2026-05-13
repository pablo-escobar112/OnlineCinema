// CinemaModule/Services/CinemaService.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaModule.Services
{
    /// <summary>
    /// Сервис бизнес-логики для управления подписками, избранным и рекомендациями.
    /// </summary>
    public class CinemaService
    {
        /// <summary>
        /// Проверяет, активна ли подписка на текущую дату.
        /// </summary>
        /// <param name="subscriptionEndDate">Дата окончания подписки.</param>
        /// <returns>True если подписка ещё действует.</returns>
        public bool IsSubscriptionActive(DateTime subscriptionEndDate)
        {
            return DateTime.Today <= subscriptionEndDate.Date;
        }

        /// <summary>
        /// Рассчитывает количество оставшихся дней подписки.
        /// </summary>
        /// <param name="subscriptionEndDate">Дата окончания подписки.</param>
        /// <returns>Количество оставшихся дней (0 если истекла).</returns>
        public int GetRemainingDays(DateTime subscriptionEndDate)
        {
            var remaining = (subscriptionEndDate.Date - DateTime.Today).Days;
            return remaining > 0 ? remaining : 0;
        }

        /// <summary>
        /// Добавляет фильм в список избранного.
        /// </summary>
        /// <param name="favoritesList">Список ID избранных фильмов.</param>
        /// <param name="movieId">ID фильма для добавления.</param>
        /// <returns>True если добавлен, False если уже в списке.</returns>
        public bool AddToFavorites(ICollection<int> favoritesList, int movieId)
        {
            if (favoritesList.Contains(movieId))
                return false;

            favoritesList.Add(movieId);
            return true;
        }

        /// <summary>
        /// Определяет рекомендуемый жанр на основе самого часто просматриваемого.
        /// </summary>
        /// <param name="watchedGenres">Список ID жанров из истории просмотров.</param>
        /// <returns>ID рекомендуемого жанра или -1 если данных нет.</returns>
        public int GetRecommendedGenre(List<int> watchedGenres)
        {
            if (watchedGenres == null || watchedGenres.Count == 0)
                return -1;

            return watchedGenres
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .First().Key;
        }
    }
}