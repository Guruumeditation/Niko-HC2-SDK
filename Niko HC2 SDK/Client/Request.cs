using System;
using System.Linq;

namespace HC2.Arcanastudio.Net.Client
{
    internal class Request
    {
        public Guid Id { get; }
        public string Payload { get; }

        public string Method { get; }

        public string TopicArea { get; }

        public Request(string method, string payload)
        {
            Id = Guid.NewGuid();

            Method = method;

            TopicArea = Method.Split('.').First();

            Payload = payload;
        }
    }
}
