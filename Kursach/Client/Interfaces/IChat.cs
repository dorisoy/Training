using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Core.ServerMethods;

namespace ISTraining_Part.Client.Interfaces
{

    interface IChat : IChatHub
    {

        event OnChatMessage NewMessage;
    }
}
