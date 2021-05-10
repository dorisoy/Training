using System;

namespace ISTraining_Part.Models
{
 
    class ChatMessage
    {

        public string SenderName { get; }
        public string Text { get; }

        public string Time { get; }

        public ChatMessage(string senderName, string text)
        {
            SenderName = senderName;
            Text = text;
            Time = DateTime.Now.ToShortTimeString();
        }
    }
}
