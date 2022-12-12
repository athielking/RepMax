using RepMax2.Data.Models;

namespace RepMax2.Data
{
    public interface IRepository
    {
        Task EnsureDbVersion();
        Task InsertExerciseSet(string name, float weight, int reps);
        Task<IEnumerable<ExerciseSet>> GetAllExerciseMaximums();
    }
}