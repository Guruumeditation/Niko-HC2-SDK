namespace HC2.Arcanastudio.Net.RequestSerializers
{
    internal interface IRequestSerializer
    {
        string Serialize(string method, object payload);
    }
}
