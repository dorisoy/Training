using ISTraining_Part.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{

    public interface IStudentsHub
    {
        #region OtherRegion

        Task RaiseStudentsImported();
        #endregion

        #region Get region

        Task<ISTrainingPartResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId);

        Task<ISTrainingPartResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds);
        #endregion

        #region CUD region

        Task<ISTrainingPartResponse<bool>> AddStudentAsync(Student student);
        Task<ISTrainingPartResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students);
        Task<ISTrainingPartResponse<bool>> SaveStudentAsync(Student student);
        Task<ISTrainingPartResponse<bool>> RemoveStudentAsync(Student student);
        #endregion
    }
}
