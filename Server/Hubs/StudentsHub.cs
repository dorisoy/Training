using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{

    [AuthorizeUser]
    [HubName(HubNames.StudentsHub)]
    public class StudentsHub : Hub<IStudentsHubEvents>, IStudentsHub
    {

        readonly IStudentsRepository repository;


        public StudentsHub(IStudentsRepository repository)
        {
            this.repository = repository;
        }

        #region Other region

        public Task RaiseStudentsImported()
        {
            Clients.Group(Consts.AuthorizedGroup).StudentsImported();
            return Task.CompletedTask;
        }
        #endregion

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId)
        {
            return repository.GetStudentsAsync(groupId);
        }


        public Task<ISTrainingPartResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            return repository.GetStudentsCountAsync(groupIds);
        }
        #endregion

        #region CUD region

        public async Task<ISTrainingPartResponse<bool>> AddStudentAsync(Student student)
        {
            Logger.Log.Info($"Add student: {student.FullName}");

            var res = await repository.AddStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Add, student);

            return res;
        }

        public async Task<ISTrainingPartResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students)
        {
            Logger.Log.Info($"Import students: {students.Count()}");

            var res = await repository.ImportStudentsAsync(students);

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> SaveStudentAsync(Student student)
        {
            Logger.Log.Info($"Save student: {student.FullName}");

            var res = await repository.SaveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Update, student);

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> RemoveStudentAsync(Student student)
        {
            Logger.Log.Info($"Remove student: {student.FullName}");

            var res = await repository.RemoveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Remove, student);

            return res;
        }
        #endregion
    }
}
