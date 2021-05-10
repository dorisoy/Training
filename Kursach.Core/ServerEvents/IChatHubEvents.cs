namespace ISTraining_Part.Core.ServerEvents
{

    public interface IChatHubEvents
    {
        /// <summary>
        /// 聊天中的新消息
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="text"></param>
        void NewMessage(string senderName, string text);
    }
}
