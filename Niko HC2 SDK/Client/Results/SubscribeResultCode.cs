namespace HC2.Arcanastudio.Net.Client.Results
{
    public enum SubscribeResultCode
    {
        UnspecifiedError = 128,
        ImplementationSpecificError = 131,
        NotAuthorized = 135,
        TopicFilterInvalid = 143,
        PacketIdentifierInUse = 145,
        QuotaExceeded = 151,
        SharedSubscriptionsNotSupported = 158,
        SubscriptionIdentifiersNotSupported = 161,
        WildcardSubscriptionsNotSupported = 162,
        Success = 7000
    }
}
