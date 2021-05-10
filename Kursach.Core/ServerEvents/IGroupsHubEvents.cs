using ISTraining_Part.Core.Models;

namespace ISTraining_Part.Core.ServerEvents
{

    public interface IGroupsHubEvents : IChangedEvent<Group>
    {
        /// <summary>
        /// 导入组
        /// </summary>
        void GroupsImport();
    }
}
