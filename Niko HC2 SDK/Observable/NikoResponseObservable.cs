using System;
using System.Collections.Generic;
using HC2.Arcanastudio.Net.Client.Messages;

namespace HC2.Arcanastudio.Net.Observable
{
    internal class NikoResponseObservable : IObservable<INikoMessage>
    {
        private readonly List<IObserver<INikoMessage>> _observers = new List<IObserver<INikoMessage>>();

        #region Implementation of IObservable<out NikoResponseMessage>

        public IDisposable Subscribe(IObserver<INikoMessage> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        #endregion

        public void MessageReceived(INikoMessage msg)
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
            private readonly List<IObserver<INikoMessage>> _observers;
            private readonly IObserver<INikoMessage> _observer;

            public Unsubscriber(List<IObserver<INikoMessage>> observers, IObserver<INikoMessage> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
