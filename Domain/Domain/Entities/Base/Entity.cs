using System;
using System.Linq;

namespace PodpiskaNaSemena.Domain.Entities.Base
{
    /// <summary>
    /// Базовая сущность системы с идентификатором типа TId.
    /// </summary>
    public abstract class Entity<TId> where TId : struct, IEquatable<TId>
    {
        public TId Id { get; protected set; }

        protected Entity() { }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
            => left?.Id.Equals(right?.Id ?? default) ?? false;

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
            => !(left == right);
    }
}