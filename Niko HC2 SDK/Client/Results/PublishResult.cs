namespace HC2.Arcanastudio.Net.Client.Results
{
    public class PublishResult : BaseResult<PublishResultCode>
    {
        public PublishResult(PublishResultCode resultCode, string message = null) : base(resultCode, message)
        {
        }

        #region Overrides of BaseResult<PublishResultCode>

        public override bool IsSuccess => ResultCode == PublishResultCode.Success;

        #endregion
    }
}
