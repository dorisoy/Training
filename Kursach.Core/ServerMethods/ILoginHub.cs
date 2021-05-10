using ISTraining_Part.Core.Models;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{

    public interface ILoginHub
    {
        Task<ISTrainingPartResponse<User, LoginResponse>> LoginAsync(string login, string password);
        void Logout();
    }
}
