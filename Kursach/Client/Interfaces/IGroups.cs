using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerMethods;
using System;

namespace ISTraining_Part.Client.Interfaces
{

    interface IGroups : IGroupsHub
    {

        event OnChanged<Group> OnChanged;


        event Action Imported;
    }
}
