using DevExpress.Mvvm;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Dialogs.Attributes;

namespace ISTraining_Part.Dialogs
{
    /// <summary>
    /// Detail info view model.
    /// </summary>
    [DialogName(nameof(DetailInfoView))]
    class DetailInfoViewModel : ViewModelBase
    {

        public DetailInfo DetailInfo { get; private set; }

        public DetailInfoViewModel()
        {
            DetailInfo = new DetailInfo
            {
                Phone = "88005553535",
                Address = "г.о.г. Бор",
                EMail = "allaha@net.net",
            };
        }

        public DetailInfoViewModel(int id, DetailInfoType type, IClient client)
        {
            Load(client, id, type);
        }

        private async void Load(IClient client, int id, DetailInfoType type)
        {
            var res = await client.DetailInfo.GetDetailInfoAsync(id, type);

            DetailInfo = res.Response;
        }
    }
}
