using System.Linq.Expressions;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Specifications;

namespace VortexCombat.Application.Specifications
{
    public sealed class MasterByIdSpec : Specification<Master>
    {
        private readonly int _id;
        public MasterByIdSpec(int id) => _id = id;
        public override Expression<Func<Master, bool>> Criteria => m => m.Id == _id;
    }
}