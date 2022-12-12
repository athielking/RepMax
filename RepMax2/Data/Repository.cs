using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepMax2.Constants;
using RepMax2.Data.Models;
using RepMax2.Services;
using System.Collections;

namespace RepMax2.Data
{
    public class Repository : IRepository
    {
        Lazy<SQLiteAsyncConnection> Database = new Lazy<SQLiteAsyncConnection>(new SQLiteAsyncConnection(DbConstants.DatabasePath, DbConstants.Flags));

        public async Task EnsureDbVersion()
        {
            var db = Database.Value;

            Models.Version version = null;
            try
            {
                version = await db.Table<Models.Version>().OrderByDescending(v => v.VersionNumber).FirstOrDefaultAsync();
            }
            catch (SQLiteException e)
            {
            }

            if (version == null)
            {
                await db.CreateTableAsync<Models.Version>();
                await db.CreateTableAsync<Models.ExerciseSet>();

                version = new Models.Version { VersionNumber = 0 };
                await db.InsertAsync(version);
            }

            int maxVersion = Migrations.Up.Keys.Any() ? Migrations.Up.Keys.Max() : 0;

            if (maxVersion <= version.VersionNumber)
                return;

            await db.RunInTransactionAsync(connection =>
            {
                for (int i = version.VersionNumber + 1; i <= maxVersion; i++)
                {
                    var statements = Migrations.Up[i];

                    foreach (var sql in statements)
                    {
                        connection.Execute(sql);
                    }

                    connection.Insert(new Models.Version { VersionNumber = i });
                }
            });
        }

        public async Task InsertExerciseSet(string name, float weight, int reps)
        {
            var estimate = CalculatorService.Estimate1RM(weight, reps);

            var set = new ExerciseSet { Name = name, Weight = weight, Reps = reps, Est1Rm = estimate };

            var current = await Database.Value.Table<ExerciseSet>().Where(s => s.Name == name && s.UseForPercentages).FirstOrDefaultAsync();

            if (current != null && current.Est1Rm < estimate)
            {
                current.UseForPercentages = false;
                set.UseForPercentages = true;

                await Database.Value.UpdateAsync(current);
            }

            await Database.Value.InsertAsync(set);
        }

        public async Task<IEnumerable<ExerciseSet>> GetAllExerciseMaximums()
        {
            var list = await Database.Value.Table<ExerciseSet>().Where(s => s.UseForPercentages).ToListAsync();
            var dictionary = list.GroupBy(s => s.Name).ToDictionary(group => group.Key, group => group.OrderByDescending(s => s.CreatedTs).FirstOrDefault());

            return new List<ExerciseSet> {
                new ExerciseSet
                {
                    Id = 1,
                    Name= "Deadlift",
                    Est1Rm = 400,
                    Reps = 5,
                    Weight = 365,
                    UseForPercentages = true,
                    CreatedTs = DateTimeOffset.UtcNow
                }
            };

            //return dictionary.Values.ToList();
        }
    }
}
