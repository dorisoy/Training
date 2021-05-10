using ISTraining_Part.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{

    public interface IGroupsHub
    {
        #region Get region

        Task<ISTrainingPartResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1);

        #endregion

        #region CUD region

        Task<ISTrainingPartResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups);
        Task<ISTrainingPartResponse<bool>> AddGroupAsync(Group group);
        Task<ISTrainingPartResponse<bool>> SaveGroupAsync(Group group);
        Task<ISTrainingPartResponse<bool>> RemoveGroupAsync(Group group);

        #endregion
    }
}
