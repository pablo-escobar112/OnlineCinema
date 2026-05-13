// CinemaModule/Models/UserSubscriptionInfo.cs
using System;

namespace CinemaModule.Models
{
    /// <summary>
    /// Информация о текущей подписке пользователя.
    /// </summary>
    public class UserSubscriptionInfo
    {
        /// <summary>
        /// Дата окончания подписки.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Название тарифа.
        /// </summary>
        public string TariffName { get; set; } = string.Empty;

        /// <summary>
        /// Стоимость тарифа.
        /// </summary>
        public decimal Price { get; set; }
    }
}