using System;

namespace UsbInfo.Natives
{
    internal class Disposable : IDisposable
    {
        private readonly Action _action;

        public static Disposable Create(Action action)
        {
            return new Disposable(action);
        }

        private Disposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action?.Invoke();
        }
    }
}