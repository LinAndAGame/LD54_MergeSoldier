using System;
using System.Collections;
using System.Collections.Generic;

namespace MyGameUtility {
    public class LoopArray<T> : IEnumerable<T> {
        public T[] Source;
        public int Capacity;

        public LoopArray(int capacity) {
            Capacity = capacity;
            Source   = new T[Capacity];
        }

        public T this[int index] {
            get {
                if (index < 0) {
                    throw new IndexOutOfRangeException("Index can not less than zero!");
                }

                return Source[index % Capacity];
            }
            set {
                if (index < 0) {
                    throw new IndexOutOfRangeException("Index can not less than zero!");
                }

                Source[index % Capacity] = value;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return new LoopArrayEnumerator<T>(Source);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        
        public class LoopArrayEnumerator<T2> : IEnumerator<T2> {
            private  T2[] _Source;
            private int     _Index = -1;

            public LoopArrayEnumerator(T2[] source) {
                _Source = source;
            }

            public bool MoveNext() {
                _Index++;
                return _Index < _Source.Length;
            }

            public void Reset() {
                _Index = -1;
            }

            public T2 Current {
                get {
                    if (_Index >= 0 && _Index < _Source.Length) {
                        return _Source[_Index];
                    }
                    else {
                        throw new IndexOutOfRangeException();
                    }
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose() {
                _Source = null;
            }
        }
    }
}