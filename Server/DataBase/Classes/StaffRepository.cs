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

    class StaffRepository : IStaffRepository
    {

        readonly IRepository repository;


        public StaffRepository(IRepository repository)
        {
            this.repository = repository;
        }

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            return repository.QueryAsync(con =>
            {
                return con.GetAllAsync<Staff>();
            }, Enumerable.Empty<Staff>());
        }


        public async Task<ISTrainingPartResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            ISTrainingPartResponse<Staff, bool> response = null;
            bool added = false;

            var query = await repository.QueryAsync(async con =>
            {
                var staff = await con.QueryFirstOrDefaultAsync<Staff>("SELECT id FROM staff LIMIT 1");
                if (staff == null)
                {
                    staff = new Staff
                    {
                        LastName = "陈",
                        FirstName = "某某",
                        MiddleName = "曾用名",
                        Position = "未知"
                    };

                    added = await AddStaffAsync(staff);

                    return added ? staff : null;
                }
                else return staff;
            });

            response = new ISTrainingPartResponse<Staff, bool>(query.Code, added, query.Response);

            return response;
        }
        #endregion

        #region CUD region

        public Task<ISTrainingPartResponse<bool>> AddStaffAsync(Staff staff)
        {
            return repository.QueryAsync(async con =>
            {
                await con.InsertAsync(staff);
                return true;
            });
        }


        public Task<ISTrainingPartResponse<bool>> SaveStaffAsync(Staff staff)
        {
            return repository.QueryAsync(con =>
            {
                return con.UpdateAsync(staff);
            });
        }


        public Task<ISTrainingPartResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            return repository.QueryAsync(con =>
            {
                return con.DeleteAsync(staff);
            });
        }
        #endregion
    }
}
