using System;
using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Observable
{
    public class MessageObservable : IObservable<IMessage>
    {
        private readonly List<IObserver<IMessage>> _observers = new List<IObserver<IMessage>>();

        #region Implementation of IObservable<out NikoResponseMessage>

        public IDisposable Subscribe(IObserver<IMessage> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        #endregion

        public void MessageReceived(IMessage msg)
        {
            foreach (var observer in _observers)
            {
                if (msg == null)
                    observer.OnError(new Exception("Error"));
                else
                    observer.OnNext(msg);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in _observers.ToArray())
                if (_observers.Contains(observer))
                    observer.OnCompleted();

            _observers.Clear();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<IMessage>> _observers;
            private readonly IObserver<IMessage> _observer;

            public Unsubscriber(List<IObserver<IMessage>> observers, IObserver<IMessage> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
