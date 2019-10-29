using System;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Observable
{
    public sealed class MessageObserver : IObserver<IMessage>
    {
        private readonly Action _onCompletedAction;
        private readonly Action<Exception> _onErrorAction;
        private readonly Action<IMessage> _onNextAction;

        public MessageObserver(Action<IMessage> onnextaction, Action oncompletedaction = null, Action<Exception> onerroraction = null)
        {
            _onCompletedAction = oncompletedaction ?? (() => {});
            _onErrorAction = onerroraction ?? (e => { });
            _onNextAction = onnextaction ?? throw new ArgumentNullException(nameof(onnextaction));
        }

        #region Implementation of IObserver<in IMessage>

        public void OnCompleted()
        {
            _onCompletedAction.Invoke();
        }

        public void OnError(Exception error)
        {
            _onErrorAction?.Invoke(error);
        }

        public void OnNext(IMessage value)
        {
            _onNextAction(value);
        }

        #endregion
    }
}
