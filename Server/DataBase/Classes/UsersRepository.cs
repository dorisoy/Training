using Dapper;
using Dapper.Contrib.Extensions;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataBase.Classes
{

    class UsersRepository : IUsersRepository
    {

        readonly IRepository repository;


        public UsersRepository(IRepository repository)
        {
            this.repository = repository;
        }

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<User>>> GetUsersAsync()
        {
            return repository.QueryAsync(con =>
            {
                return con.GetAllAsync<User>();
            }, Enumerable.Empty<User>());
        }


        public Task<ISTrainingPartResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            return repository.QueryAsync(con =>
            {
                if (usePassword)
                    return con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login AND password = @password", new { login, password });

                return con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login", new { login });
            });
        }
        #endregion

        #region Log region

        public Task<ISTrainingPartResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            return repository.QueryAsync(con =>
            {
                return con.QueryAsync<SignInLog>("SELECT * FROM signinlogs WHERE userId = @userId", new { userId });
            }, Enumerable.Empty<SignInLog>());
        }


        public async void AddSignInLogAsync(User user)
        {
            await repository.QueryAsync<long>(async con =>
            {
                return await con.InsertAsync(new SignInLog { UserId = user.Id });
            });
        }
        #endregion

        #region CUD region

        public Task<ISTrainingPartResponse<bool>> AddUserAsync(User user)
        {
            return repository.QueryAsync(async con =>
            {
                await con.InsertAsync(user);
                return true;
            });
        }


        public Task<ISTrainingPartResponse<bool>> SaveUserAsync(User user)
        {
            return repository.QueryAsync(con =>
            {
                return con.UpdateAsync(user);
            });
        }


        public Task<ISTrainingPartResponse<bool>> RemoveUserAsync(User user)
        {
            return repository.QueryAsync(con =>
            {
                return con.DeleteAsync(user);
            });
        }
        #endregion
    }
}
