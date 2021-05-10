using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Interfaces
{

    interface IHubConfigurator
    {

        HubConnection Hub { get; }


        event Action Connected;

        event Action Disconnected;


        event Action Reconnected;


        event Action Reconnecting;

        Task ConnectAsync();
    }
}
