using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class ResponseError : IError
    {
        #region Implementation of IError

        public string ErrCode { get; }
        public string ErrMessage { get; }
        public string Method { get; }

        #endregion

        public ResponseError(string errCode, string errMessage, string method)
        {
            ErrCode = errCode;
            ErrMessage = errMessage;
            Method = method;
        }
    }
}
