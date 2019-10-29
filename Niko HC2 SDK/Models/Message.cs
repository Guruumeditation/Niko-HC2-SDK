using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class Message : IMessage
    {
        public string MessageType { get; }
        public IError Error { get; }

        public bool IsError => Error != null;

        public List<object> Data { get; }

        public Message(string type, IError error)
        {
            MessageType = type;
            Error = error;
        }

        public Message(string type, List<object> data)
        {
            MessageType = type;
            Data = data;
        }
    }
}
