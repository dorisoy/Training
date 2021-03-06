using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;

namespace ISTraining_Part.Client.Interfaces
{

    interface IStaff : IStaffHub
    {

        event OnChanged<Staff> OnChanged;
    }
}
