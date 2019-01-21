using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GetMeX.ViewModels.Utilities
{
    public class Messenger
    {
        private static readonly object CreationLock = new object();
        private static readonly ConcurrentDictionary<MessengerKey, object> MsgDict = new ConcurrentDictionary<MessengerKey, object>();

        private static Messenger _base;

        public static Messenger Base
        {
            get
            {
                if (_base == null)
                {
                    lock (CreationLock)
                    {
                        if (_base == null)
                        {
                            _base = new Messenger();
                        }
                    }
                }
                return _base;
            }
        }

        private Messenger() { }

        // Register a recipient to msg of type T
        public void Register<T>(object recipient, Action<T> action)
        {
            Register(recipient, action, null);
        }

        // Register a recipient to msg of type T and matching context
        public void Register<T>(object recipient, Action<T> action, object context)
        {
            var key = new MessengerKey(recipient, context);
            MsgDict.TryAdd(key, action);
        }

        // Remove a recipient from "queue" to receive msg
        public void Remove(object recipient)
        {
            Remove(recipient, null);
        }

        // Remove a recipient with corresponding context from "queue" to receive msg
        public void Remove(object recipient, object context)
        {
            object action;
            var key = new MessengerKey(recipient, context);
            MsgDict.TryRemove(key, out action);
        }

        // Sends msg to registered recipients
        public void Send<T>(T msg)
        {
            Send(msg, null);
        }

        // Sends msg to registered recipients with matching context
        public void Send<T>(T msg, object context)
        {
            IEnumerable<KeyValuePair<MessengerKey, object>> result;
            if (context == null)
            {
                // Recipients with null context
                result = from r in MsgDict where r.Key.Context == null select r;
            }
            else
            {
                // Recipients with matching context
                result = from r in MsgDict where r.Key.Context != null && r.Key.Context.Equals(context) select r; 
            }

            foreach (var action in result.Select(x => x.Value).OfType<Action<T>>())
            {
                // Send msg to recipients
                action(msg);
            }
        }

        protected class MessengerKey
        {
            public object Recipient { get; private set; }
            public object Context { get; private set; }

            public MessengerKey(object recipient, object context)
            {
                Recipient = recipient;
                Context = context;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != GetType()) return false;
                return ReferenceEquals(this, obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Recipient != null ? Recipient.GetHashCode() : 0) * 397) ^ (Context != null ? Context.GetHashCode() : 0);
                }
            }
        }
    }
}
