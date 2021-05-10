using ISTraining_Part.Core;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Server.DataBase.Interfaces
{

    public interface IRepository
    {

        Task<ISTrainingPartResponse<T>> QueryAsync<T>(Func<IDbConnection, Task<T>> func, T defaultValue = default, [CallerMemberName]string callerName = null);
    }
}
