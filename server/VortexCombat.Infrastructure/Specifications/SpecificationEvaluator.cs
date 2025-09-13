using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Specifications;

namespace VortexCombat.Infrastructure.Specifications
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(
            IQueryable<T> input,
            Specification<T>? spec)
        {
            if (spec == null) return input;
            return input.Where(e => spec.IsSatisfiedBy(e));
        }
    }
}