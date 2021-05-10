using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;

namespace Server.DataBase.Interfaces
{
    public interface IUsersRepository : IUsersHub
    {
        void AddSignInLogAsync(User user);
    }
}
