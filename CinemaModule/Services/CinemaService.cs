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
        /// <summary>
        /// Создаёт новую подборку фильмов.
        /// </summary>
        /// <param name="name">Название подборки.</param>
        /// <param name="description">Описание подборки.</param>
        /// <returns>True, если название не пустое.</returns>
        public bool CreateCollection(string name, string description, out string error)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                error = "Название подборки не может быть пустым.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        /// <summary>
        /// Добавляет фильм в подборку, если его там ещё нет.
        /// </summary>
        /// <param name="existingMovieIds">Список ID фильмов, уже находящихся в подборке.</param>
        /// <param name="movieId">ID добавляемого фильма.</param>
        /// <returns>True, если фильм добавлен.</returns>
        public bool AddToCollection(ICollection<int> existingMovieIds, int movieId)
        {
            if (existingMovieIds.Contains(movieId))
                return false;
            existingMovieIds.Add(movieId);
            return true;
        }

        /// <summary>
        /// Удаляет фильм из подборки.
        /// </summary>
        /// <param name="existingMovieIds">Список ID фильмов в подборке.</param>
        /// <param name="movieId">ID удаляемого фильма.</param>
        /// <returns>True, если фильм был в подборке и удалён.</returns>
        public bool RemoveFromCollection(ICollection<int> existingMovieIds, int movieId)
        {
            return existingMovieIds.Remove(movieId);
        }

        /// <summary>
        /// Возвращает количество фильмов в подборке.
        /// </summary>
        /// <param name="movieIds">Список ID фильмов.</param>
        /// <returns>Количество фильмов.</returns>
        public int GetMovieCount(ICollection<int> movieIds)
        {
            return movieIds?.Count ?? 0;
        }

        /// <summary>
        /// Проверяет, доступен ли фильм по активной подписке.
        /// Фильмы с возрастным рейтингом 18+ недоступны по базовой подписке.
        /// </summary>
        /// <param name="movieAgeRating">Возрастной рейтинг фильма.</param>
        /// <param name="subscriptionActive">Активна ли подписка.</param>
        /// <param name="tariffName">Название тарифа (Базовый/Премиум).</param>
        /// <returns>True, если фильм доступен.</returns>
        public bool IsMovieAvailableBySubscription(int movieAgeRating, bool subscriptionActive, string tariffName)
        {
            if (!subscriptionActive)
                return false;

            if (tariffName == "Базовый" && movieAgeRating >= 18)
                return false;

            return true;
        }

    }
}