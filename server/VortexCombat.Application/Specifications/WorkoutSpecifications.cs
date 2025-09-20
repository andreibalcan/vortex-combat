using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Specifications;

namespace VortexCombat.Application.Specifications
{
    public sealed class WorkoutByIdSpec : Specification<Workout> // rename to WorkoutState
    {
        private readonly int _id;
        public WorkoutByIdSpec(int id) => _id = id;
        public override bool IsSatisfiedBy(Workout workout) => workout.Id == _id;
    }

    public sealed class StudentByIdSpec : Specification<Student> // rename to StudentState
    {
        private readonly int _id;
        public StudentByIdSpec(int id) => _id = id;
        public override bool IsSatisfiedBy(Student student) => student.Id == _id;
    }

    public sealed class MasterByIdSpec : Specification<Master> // rename to MasterState
    {
        private readonly int _id;
        public MasterByIdSpec(int id) => _id = id;
        public override bool IsSatisfiedBy(Master master) => master.Id == _id;
    }

    public sealed class StudentByUserIdSpec : Specification<Student>
    {
        private readonly Guid _userId;
        public StudentByUserIdSpec(Guid userId) => _userId = userId;
        public override bool IsSatisfiedBy(Student student) => student.UserId.Value == _userId;
    }
}