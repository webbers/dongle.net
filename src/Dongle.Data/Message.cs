using System.Collections.Generic;

namespace Dongle.Data
{
    public class Message
    {
        public string Text { get; set; }
        public MessageType MesssageType { get; set; }

        public List<IHaveId> Items { get; set; }
        public bool Persistent { get; set; }

        public string Url { get; set; }
        public string UrlDescription { get; set; }
    }
}