namespace HC2.Arcanastudio.Net.Client.Messages
{
    internal class NikoErrorMessage : INikoMessage
    {
        public string Method { get; set; }
        public string ErrCode { get; set; }
        public string ErrMessage { get; set; }

        #region Implementation of INikoMessage

        public NikoMessageType MessageType => NikoMessageType.Err;

        #endregion
    }
}
