﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueLogic
{
    /// <summary>
    /// implementation structure of data LILO
    /// </summary>
    /// <typeparam name="T">type of data</typeparam>
    public class Queue<T> : IEnumerable<T>
    {
        private T[] elements;
        private int head;
        private int tail;
        private int capacity;
        private int count;

        #region ctors
        public Queue() : this(10) { }
        public Queue(int capacity)
        {
            if(capacity<1) throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be natural");
            elements = new T[capacity];
            this.capacity = capacity;
            head = -1;
            tail = -1;
            count = 0;
        }
        public Queue(IEnumerable<T> collection)
        {
            if(collection==null) throw new ArgumentNullException();
            capacity = collection.Count();
            elements = new T[capacity];
            head = -1;
            tail = -1;
            foreach (T element in collection)
            {
                EnQueue(element);
            }
        }
        #endregion


        public int Count => count;
        public T Peek
        {
            get
            {
                if (IsEmpty()) throw new InvalidOperationException("empty stack");
                return elements[(head + 1) % capacity];
            }
        }
        /// <summary>
        /// adds element into queue
        /// </summary>
        /// <param name="arg">element for adding</param>
        public void EnQueue(T arg)
        {
            if (IsFull())
            {
                int newcapacity = capacity * 2;
                T[] newelements = new T[newcapacity];
                for (int i = 0; i < elements.Length; i++)
                {
                    newelements[i] = DeQueue();
                }
                head = -1;
                count = capacity;
                tail = count - 1;
                capacity = newcapacity;
                elements = newelements;
            }
            elements[++tail % capacity] = arg;
            count++;
        }
        /// <summary>
        /// retrieves element from queue
        /// </summary>
        /// <returns>retrieved element</returns>
        public T DeQueue()
        {
            if (IsEmpty()) throw new InvalidOperationException("empty stack");
            count--;
            return elements[++head % capacity];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private struct Enumerator : IEnumerator<T>
        {
            private Queue<T> queue;
            private int cursor;
            public Enumerator(Queue<T> queue)
            {
                this.queue = queue;
                cursor = queue.head;
            }
            public T Current
            {
                get { return queue.elements[cursor % queue.capacity]; }
            }

            object IEnumerator.Current
            {
                get
                {
                    return queue.elements[cursor % queue.capacity];
                }
            }

            public void Dispose() { }

            public bool MoveNext()
            {

                while (cursor < queue.tail)
                {
                    cursor++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                cursor = queue.head;
            }
        }
        private bool IsEmpty()
        {
            return Count == 0;
        }

        private bool IsFull()
        {
            return Count == capacity;
        }
    }
}
