using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace LinkedList
{
    public class SimpleLinkedList<T> : IEnumerable<T>
    {
        private class SimpleLinkedListEnumerator : IEnumerator<T>
        {
            private readonly SimpleLinkedList<T> _list;
            private SimpleLinkedListElement? _node;
            private T _current;
            
            public SimpleLinkedListEnumerator(SimpleLinkedList<T> simpleLinkedList)
            {
                _list = simpleLinkedList;
                _node = _list._head;
                _current = default;
            }
            
            public bool MoveNext()
            {
                if (_node == null)
                {
                    return false;
                }

                _current = _node.Value;
                _node = _node.Next;

                return true;
            }

            public void Reset()
            {
                _node = _list._head;
            }

            public T Current => _current;

            object? IEnumerator.Current => this.Current;

            public void Dispose()
            {
            }
        }

        private class SimpleLinkedListElement
        {
            public SimpleLinkedListElement? Next { get; set; }
            
            public T Value { get; }
            
            public SimpleLinkedListElement(T value)
            {
                this.Value = value;
            }

            public override string ToString() => this.Value?.ToString() ?? string.Empty;
        }

        private SimpleLinkedListElement? _head;

        public SimpleLinkedList(IEnumerable<T> source)
        {
            SimpleLinkedListElement? previous = null;
            foreach (var element in source)
            {
                var current = new SimpleLinkedListElement(element);
                if (_head == null)
                {
                    _head = current;
                    previous = _head;
                }
                else
                {
                    previous!.Next = current;
                    previous = current;
                }
            }
        }

        public void Reverse()
        {
            if (_head?.Next == null)
            {
                return;
            }

            SimpleLinkedListElement? previous = default;
            SimpleLinkedListElement? current = _head;
            
            do
            {
                var next = current.Next;
                current.Next = previous;
                previous = current;
                current = next;

            } while (current != null);

            _head = previous;
        }
        
        public IEnumerator<T> GetEnumerator() => new SimpleLinkedListEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => string.Join("; ", this);
    }
}