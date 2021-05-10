namespace ISTraining_Part.Client.Interfaces
{

    interface IClient
    {

        IHubConfigurator HubConfigurator { get; }


        IUsers Users { get; }


        IStaff Staff { get; }

        IStudents Students { get; }


        IGroups Groups { get; }


        ILogin Login { get; }


        IChat Chat { get; }


        IDetailInfo DetailInfo { get; }
    }
}
