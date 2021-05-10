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

    class GroupsRepository : IGroupsRepository
    {

        readonly IRepository repository;

   
        public GroupsRepository(IRepository repository)
        {
            this.repository = repository;
        }

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            return repository.QueryAsync(con =>
            {
                if (divisionId == -1)
                    return con.GetAllAsync<Group>();
                else return con.QueryAsync<Group>("SELECT * FROM groups WHERE division = @division", new { division = divisionId });
            }, Enumerable.Empty<Group>());
        }
        #endregion

        #region CUD region
 
        public Task<ISTrainingPartResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            return repository.QueryAsync(async con =>
            {
                await con.InsertAsync(groups);
                return true;
            });
        }


        public Task<ISTrainingPartResponse<bool>> AddGroupAsync(Group group)
        {
            return repository.QueryAsync(async con =>
            {
                await con.InsertAsync(group);
                return true;
            });
        }


        public Task<ISTrainingPartResponse<bool>> SaveGroupAsync(Group group)
        {
            return repository.QueryAsync(con =>
            {
                return con.UpdateAsync(group);
            });
        }


        public Task<ISTrainingPartResponse<bool>> RemoveGroupAsync(Group group)
        {
            return repository.QueryAsync(con =>
            {
                return con.DeleteAsync(group);
            });
        }
        #endregion
    }
}
