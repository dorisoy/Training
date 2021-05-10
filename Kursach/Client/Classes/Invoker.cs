using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{

    abstract class Invoker
    {

        public IHubConfigurator HubConfigurator { get; }

 
        public IHubProxy Proxy { get; }


        public Invoker(IHubConfigurator configurator, string hubName)
        {
            HubConfigurator = configurator;
            Proxy = HubConfigurator.Hub.CreateHubProxy(hubName);
        }


        public Task TryInvokeAsync([CallerMemberName]string method = null, object[] args = null)
        {
            args = args ?? new object[0];

            if (HubConfigurator.Hub.State != ConnectionState.Connected)
                return Task.CompletedTask;

            try
            {
                return Proxy.Invoke(method, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }


        public Task<ISTrainingPartResponse<T>> TryInvokeAsync<T>([CallerMemberName]string method = null, T defaultValue = default, params object[] args)
        {
            return baseTryInvokeAsync(method, new ISTrainingPartResponse<T>(ISTrainingPartResponseCode.ServerError, defaultValue), args);
        }


        public Task<ISTrainingPartResponse<T, TArg>> TryInvokeAsync<T, TArg>([CallerMemberName]string method = null, T defaultValue = default, TArg argDefault = default, params object[] args)
        {
            return baseTryInvokeAsync(method, new ISTrainingPartResponse<T, TArg>(ISTrainingPartResponseCode.ServerError, argDefault, defaultValue), args);
        }

        Task<T> baseTryInvokeAsync<T>([CallerMemberName]string method = null, T defaultValue = default, params object[] args)
        {
            if (HubConfigurator.Hub.State != ConnectionState.Connected)
                return Task.FromResult(defaultValue);

            try
            {
                return Proxy.Invoke<T>(method, args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                Logger.Log.Error($"Ошибка запроса к серверу: {{method: {method}, args: {string.Join(",", args)}}}", ex);

                return Task.FromResult(defaultValue);
            }
        }
    }
}
