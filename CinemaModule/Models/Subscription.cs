// CinemaModule/Models/Subscription.cs
namespace CinemaModule.Models
{
    /// <summary>
    /// Представляет тариф подписки.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Уникальный идентификатор тарифа.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название тарифа.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Стоимость в рублях.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Срок действия в днях.
        /// </summary>
        public int DurationDays { get; set; }
    }
}