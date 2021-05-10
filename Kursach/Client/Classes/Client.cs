using ISTraining_Part.Client.Interfaces;

namespace ISTraining_Part.Client.Classes
{

    class Client : IClient
    {

        public Client(IUsers users,
                      IStaff staff,
                      IStudents students,
                      IGroups groups,
                      ILogin login,
                      IChat chat,
                      IDetailInfo detailInfo,
                      IHubConfigurator hubConfigurator)
        {
            Users = users;
            Staff = staff;
            Students = students;
            Groups = groups;
            Login = login;
            Chat = chat;
            DetailInfo = detailInfo;
            HubConfigurator = hubConfigurator;
        }


        public IUsers Users { get; }


        public IStaff Staff { get; }


        public IStudents Students { get; }


        public IGroups Groups { get; }


        public ILogin Login { get; }


        public IChat Chat { get; }


        public IDetailInfo DetailInfo { get; }

        public IHubConfigurator HubConfigurator { get; }
    }
}
