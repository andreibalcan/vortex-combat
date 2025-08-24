using System.Linq.Expressions;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Specifications;

namespace VortexCombat.Application.Specifications
{
    // Workout exists by Id
    public sealed class WorkoutByIdSpec : Specification<Workout>
    {
        private readonly int _id;
        public WorkoutByIdSpec(int id) => _id = id;
        public override Expression<Func<Workout, bool>> Criteria => w => w.Id == _id;
    }
}