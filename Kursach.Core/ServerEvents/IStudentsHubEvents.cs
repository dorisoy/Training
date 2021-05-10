using ISTraining_Part.Core.Models;

namespace ISTraining_Part.Core.ServerEvents
{

    public interface IStudentsHubEvents : IChangedEvent<Student>
    {
        /// <summary>
        /// 学生导入
        /// </summary>
        void StudentsImported();
    }
}
