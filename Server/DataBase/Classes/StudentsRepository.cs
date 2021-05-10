using Dapper;
using Dapper.Contrib.Extensions;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataBase.Classes
{

    class StudentsRepository : IStudentsRepository
    {

        readonly IRepository repository;

        public StudentsRepository(IRepository repository)
        {
            this.repository = repository;
        }


        public Task RaiseStudentsImported()
        {
            return Task.CompletedTask;
        }

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId)
        {
            return repository.QueryAsync(con =>
            {
                if (groupId == -1)
                    return con.QueryAsync<Student>("SELECT * FROM students");
                else return con.QueryAsync<Student>("SELECT * FROM students WHERE groupId = @id", new { id = groupId });
            }, Enumerable.Empty<Student>());
        }


        public Task<ISTrainingPartResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            return repository.QueryAsync(async con =>
            {
                var students = new Dictionary<int, StudentsCount>();

                foreach (var item in groupIds)
                {
                    var total = await con.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM students WHERE groupId = @item", new { item });
                    var sabbatical = await con.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM students WHERE groupId = @item AND onSabbatical = 1", new { item });
                    students[item] = new StudentsCount(total, sabbatical);
                }

                return students;
            }, new Dictionary<int, StudentsCount>());
        }
        #endregion

        #region CUD region

        public Task<ISTrainingPartResponse<bool>> AddStudentAsync(Student student)
        {
            return repository.QueryAsync(async con =>
            {
                await con.InsertAsync(student);
                return true;
            });
        }


        public Task<ISTrainingPartResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students)
        {
            return repository.QueryAsync(async con =>
            {
                await con.InsertAsync(students);
                return true;
            });
        }

        public Task<ISTrainingPartResponse<bool>> SaveStudentAsync(Student student)
        {
            return repository.QueryAsync(con =>
            {
                return con.UpdateAsync(student);
            });
        }


        public Task<ISTrainingPartResponse<bool>> RemoveStudentAsync(Student student)
        {
            return repository.QueryAsync(con =>
            {
                return con.DeleteAsync(student);
            });
        }
        #endregion
    }
}
