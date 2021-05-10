using ISTraining_Part.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{

    public interface IStaffHub
    {
        #region Get region

        Task<ISTrainingPartResponse<IEnumerable<Staff>>> GetStaffsAsync();
        Task<ISTrainingPartResponse<Staff, bool>> GetOrCreateFirstStaffAsync();

        #endregion

        #region CUD region

        Task<ISTrainingPartResponse<bool>> AddStaffAsync(Staff staff);

        Task<ISTrainingPartResponse<bool>> SaveStaffAsync(Staff staff);

        Task<ISTrainingPartResponse<bool>> RemoveStaffAsync(Staff staff);
        #endregion
    }
}
