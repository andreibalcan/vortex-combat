using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Specifications;

namespace VortexCombat.Infrastructure.Specifications
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> input, ISpecification<T> spec) where T : class
        {
            if (spec?.Criteria is null) return input;
            return input.Where(spec.Criteria);
        }
    }
}