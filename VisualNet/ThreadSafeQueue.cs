using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace VisualNet
{
    public interface ILockedQueue<T> : IDisposable
    {
        int Count { get; }
        bool Contains(T value);
        T Dequeue();
    }
    public sealed class ThreadSafeQueue<T>
    {

        #region LockedQueue
        private sealed class LockedQueue : ILockedQueue<T>
        {
            private ThreadSafeQueue<T> m_outer;
            internal LockedQueue(ThreadSafeQueue<T> outer)
            {
                m_outer = outer;
                Monitor.Enter(m_outer.m_lock);
            }

            #region ILockedQueue<T> Members
            public int Count
            {
                get { return m_outer.m_queue.Count; }
            }
            public bool Contains(T value)
            {
                return m_outer.m_queue.Contains(value);
            }
            public T Dequeue()
            {
                return m_outer.m_queue.Dequeue();
            }
            #endregion
            #region IDisposable Members
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Monitor.Exit(m_outer.m_lock);
                }
            }
            ~LockedQueue()
            {
                Dispose(false);
            }
            #endregion
        }
        #endregion

        private Queue<T> m_queue = new Queue<T>();
        private object m_lock = new object();

        public ThreadSafeQueue() { }
        public void Enqueue(T value)
        {
            lock (m_lock)
            {
                m_queue.Enqueue(value);
            }
        }

        public void Clear()
        {
            lock (m_lock)
            {
                m_queue.Clear();
            }
        }

        public ILockedQueue<T> Lock()
        {
            return new LockedQueue(this);
        }
    }
}
