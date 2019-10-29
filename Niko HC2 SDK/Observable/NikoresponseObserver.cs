using System;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Client.Messages;

namespace HC2.Arcanastudio.Net.Observable
{
    internal class NikoresponseObserver : IObserver<INikoMessage>
    {
        private readonly Action _onCompletedAction;
        private readonly Action<Exception> _onErrorAction;
        private readonly Action<INikoMessage> _onNextAction;

        public NikoresponseObserver(Action<INikoMessage> onnextaction, Action oncompletedaction = null, Action<Exception> onerroraction = null)
        {
            _onCompletedAction = oncompletedaction;
            _onErrorAction = onerroraction;
            _onNextAction = onnextaction ?? throw new ArgumentNullException(nameof(onnextaction));
        }

        #region Implementation of IObserver<in NikoResponseMessage>

        public void OnCompleted()
        {
            _onCompletedAction.Invoke();
        }

        public void OnError(Exception error)
        {
            _onErrorAction?.Invoke(error);
        }

        public void OnNext(INikoMessage value)
        {
            _onNextAction(value);
        }

        #endregion
    }
}
