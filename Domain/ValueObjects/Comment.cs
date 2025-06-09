using PodpiskaNaSemena.Domain.ValueObjects.Base;
using PodpiskaNaSemena.Domain.ValueObjects.Validators;

namespace PodpiskaNaSemena.Domain.ValueObjects
{
    public sealed class Comment : ValueObject<string>
    {
        public Comment(string value)
            : base(new CommentValidator(), value.Trim()) { }
    }
}