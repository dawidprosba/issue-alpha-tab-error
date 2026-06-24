using System;
using AlphaTab;

namespace AlphaTabPrototype
{
    internal class StubEventEmitter : IEventEmitter
    {
        public Action On(Action value) => () => { };
        public void Off(Action value) { }
    }

    internal class StubEventEmitterOfT<T> : IEventEmitterOfT<T>
    {
        public Action On(Action<T> value) => () => { };
        public void Off(Action<T> value) { }
        // Untyped overloads required by the partial interface definition
        public Action On(Action value) => () => { };
        public void Off(Action value) { }
    }
}
