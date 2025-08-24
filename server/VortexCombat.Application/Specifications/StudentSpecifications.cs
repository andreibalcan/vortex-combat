using System.Linq.Expressions;
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
        private readonly string _userId;
        public StudentByUserIdSpec(string userId) => _userId = userId;
        public override Expression<Func<Student, bool>> Criteria => s => s.ApplicationUserId == _userId;
    }
}