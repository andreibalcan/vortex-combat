using System;

namespace VortexCombat.Domain.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T entity);

        public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);
        public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);
        public Specification<T> Not() => new NotSpecification<T>(this);
    }

    public sealed class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T entity) => _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
    }

    public sealed class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T entity) => _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
    }

    public sealed class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _spec;

        public NotSpecification(Specification<T> spec)
        {
            _spec = spec;
        }

        public override bool IsSatisfiedBy(T entity) => !_spec.IsSatisfiedBy(entity);
    }
}