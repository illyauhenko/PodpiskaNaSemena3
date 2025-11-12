using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Rating : ValueObject<int>
    {
        public Rating(int value)
            : base(new RatingValidator(), value) { }

        // Предопределенные рейтинги
        public static Rating OneStar => new(1);
        public static Rating TwoStars => new(2);
        public static Rating ThreeStars => new(3);
        public static Rating FourStars => new(4);
        public static Rating FiveStars => new(5);

        // Проверка уровня рейтинга
        public bool IsPoor => Value <= 2;
        public bool IsAverage => Value == 3;
        public bool IsGood => Value >= 4;
        public bool IsExcellent => Value == 5;

        // Получение рейтинга в виде звезд (строка)
        public string ToStarsString() => new string('⭐', Value);

        // Вычисление процента рейтинга (для прогресс-баров)
        public decimal ToPercentage() => (Value / 5.0m) * 100;
    }
}