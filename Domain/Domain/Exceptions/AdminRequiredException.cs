using PodpiskaNaSemena.Domain.Exceptions;
namespace PodpiskaNaSemena3.Domain.Domain.Exceptions
{
    public class AdminRequiredException : DomainException
    {
        public AdminRequiredException()
            : base("Требуются права администратора")
        { }
    }
}
