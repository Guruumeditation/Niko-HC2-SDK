namespace HC2.Arcanastudio.Net.Client.Results
{
    public class ConnectionResult : BaseResult<ConnectResultCode>
    {
        public override bool IsSuccess => ResultCode == ConnectResultCode.Success;

        public ConnectionResult(ConnectResultCode resultCode, string message = null) : base(resultCode, message)
        {
        }
    }

}
