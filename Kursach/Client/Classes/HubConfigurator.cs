using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Properties;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{

    class HubConfigurator : IHubConfigurator
    {

        public event Action Connected;


        public event Action Disconnected;


        public event Action Reconnected;

  
        public event Action Reconnecting;

  
        public HubConnection Hub { get; }

        readonly TaskFactory sync;


        public HubConfigurator(TaskFactory sync)
        {
            Hub = new HubConnection(Settings.Default.server);

            this.sync = sync;

            Hub.Closed += () => sync.StartNew(() => Disconnected?.Invoke());
            Hub.Reconnected += () => sync.StartNew(() => Reconnected?.Invoke());
            Hub.Reconnecting += () => sync.StartNew(() => Reconnecting?.Invoke());

            Hub.Received += Console.WriteLine;
        }


        public async Task ConnectAsync()
        {
            try
            {
                await Hub.Start();

                await sync.StartNew(() => Connected?.Invoke());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
