using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Hubs
{

    [AuthorizeUser]
    [HubName(HubNames.StaffHub)]
    public class StaffHub : Hub<IStaffHubEvents>, IStaffHub
    {

        readonly IStaffRepository repository;


        public StaffHub(IStaffRepository repository)
        {
            this.repository = repository;
        }

        #region Get region
 
        public Task<ISTrainingPartResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            return repository.GetStaffsAsync();
        }


        public async Task<ISTrainingPartResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            var res = await repository.GetOrCreateFirstStaffAsync();

            if (res && res.Arg) 
            {
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Add, res.Response);
            }

            return res;
        }
        #endregion

        #region CUD region

        public async Task<ISTrainingPartResponse<bool>> AddStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Add staff: {staff.FullName}");

            var res = await repository.AddStaffAsync(staff);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Add, staff);

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> SaveStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Save staff: {staff.FullName}");

            var res = await repository.SaveStaffAsync(staff);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Update, staff);

            return res;
        }


        public async Task<ISTrainingPartResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Remove staff: {staff.FullName}");

            var res = await repository.RemoveStaffAsync(staff);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Remove, staff);

            return res;
        }
        #endregion
    }
}
