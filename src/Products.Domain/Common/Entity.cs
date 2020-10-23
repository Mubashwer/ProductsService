using System;

namespace Products.Domain.Common
{
    /// <summary>
    /// Abstract class for domain objects that have a distinct identity
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; private set; }

        protected Entity(Guid id) => Id = id;

        public bool Equals(Entity? other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Entity)obj);
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        // EF Core requires properties to have a non-readonly backing field
        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity left, Entity right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right) => !(left == right);
    }
}
