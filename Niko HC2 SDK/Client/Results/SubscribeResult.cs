namespace HC2.Arcanastudio.Net.Client.Results
{
    public class SubscribeResult : BaseResult<SubscribeResultCode>
    {
        public SubscribeResult(SubscribeResultCode result_code, string message = null) : base(result_code, message)
        {
        }

        #region Overrides of BaseResult<SubscribeResultCode>

        public override bool IsSuccess => ResultCode == SubscribeResultCode.Success;

        #endregion
    }
}
