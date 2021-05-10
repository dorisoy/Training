using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{

    class Staffs : Invoker, IStaff
    {

        public Staffs(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.StaffHub)
        {
            Proxy.On<DbChangeStatus, Staff>(nameof(IStaffHubEvents.OnChanged),
                (status, staff) => OnChanged?.Invoke(status, staff));
        }


        public event OnChanged<Staff> OnChanged;

        #region Get region

        public Task<ISTrainingPartResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            Logger.Log.Info("Получение списка сотрудников");

            return TryInvokeAsync(defaultValue: Enumerable.Empty<Staff>());
        }


        public Task<ISTrainingPartResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            Logger.Log.Info("Получение первого сотрудника.");

            return TryInvokeAsync<Staff, bool>();
        }
        #endregion

        #region CUD region

        public Task<ISTrainingPartResponse<bool>> AddStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Добавление сотрудника: {{id: {staff.Id}}}");

            return TryInvokeAsync<bool>(args: staff);
        }


        public Task<ISTrainingPartResponse<bool>> SaveStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Сохранение сотрудника: {{id: {staff.Id}}}");

            return TryInvokeAsync<bool>(args: staff);
        }


        public Task<ISTrainingPartResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Удаление сотрудника: {{id: {staff.Id}}}");

            return TryInvokeAsync<bool>(args: staff);
        }
        #endregion
    }
}
