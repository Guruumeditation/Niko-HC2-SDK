using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IMessage
    {
        string MessageType { get; }
        IError Error { get; }
        bool IsError { get; }
        List<object> Data { get; }
    }
}
