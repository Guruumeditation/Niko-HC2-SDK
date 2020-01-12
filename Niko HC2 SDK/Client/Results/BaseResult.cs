namespace HC2.Arcanastudio.Net.Client.Results
{
    public abstract class BaseResult<T>
    {
        public abstract bool IsSuccess { get; }
        public T ResultCode { get; }

        public string ResultMessage { get; }

        protected BaseResult(T result_code, string message = null)
        {
            ResultCode = result_code;
            ResultMessage = message;
        }
    }
}
