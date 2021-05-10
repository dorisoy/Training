using ISTraining_Part.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{

    public interface IUsersHub
    {
        #region Get region

        Task<ISTrainingPartResponse<IEnumerable<User>>> GetUsersAsync();
        Task<ISTrainingPartResponse<User>> GetUserAsync(string login, string password, bool usePassword);
        #endregion

        #region Log region

        Task<ISTrainingPartResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId);
        #endregion

        #region CUD region

        Task<ISTrainingPartResponse<bool>> AddUserAsync(User user);


        Task<ISTrainingPartResponse<bool>> SaveUserAsync(User user);


        Task<ISTrainingPartResponse<bool>> RemoveUserAsync(User user);
        #endregion
    }
}
