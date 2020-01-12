namespace HC2.Arcanastudio.Net.Client.Messages
{
    internal interface INikoMessage
    {
        NikoMessageType MessageType { get; }

        string Method { get; }
    }
}
