using ISTraining_Part.Core.ServerEvents;

namespace ISTraining_Part.Client.Delegates
{
   
    delegate void OnChanged<T>(DbChangeStatus status, T arg);
}
