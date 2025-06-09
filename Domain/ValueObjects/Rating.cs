using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Rating : ValueObject<int>
    {
        public Rating(int value)
            : base(new RatingValidator(), value) { }
    }
}