using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMax2.Data.Models
{
    public class ExerciseSet
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public int Reps { get; set; }
        public float Est1Rm { get; set; }
        public bool UseForPercentages { get; set; }
        public DateTimeOffset CreatedTs { get; set; }
    }
}
