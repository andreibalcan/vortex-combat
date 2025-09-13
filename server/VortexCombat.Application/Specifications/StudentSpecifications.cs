using System.Linq.Expressions;
using VortexCombat.Domain.Common;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Specifications;

namespace VortexCombat.Application.Specifications
{
    public sealed class StudentByIdSpec : Specification<Student>
    {
        private readonly int _id;
        public StudentByIdSpec(int id) => _id = id;
        public override Expression<Func<Student, bool>> Criteria => s => s.Id == _id;
    }

    public sealed class StudentByUserIdSpec : Specification<Student>
    {
        private readonly UserId _userId;
        public StudentByUserIdSpec(UserId userId) => _userId = userId;
        public override Expression<Func<Student, bool>> Criteria => s => s.UserId == _userId;
    }
}