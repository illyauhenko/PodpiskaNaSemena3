namespace PodpiskaNaSemena.Application.Models.Base
{
    public interface ICreateModel<out TId> where TId : struct, IEquatable<TId>
    {
        public TId Id { get; }
        public string Username { get; }
    }
}