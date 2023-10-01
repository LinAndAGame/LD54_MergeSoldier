using System;

namespace MyGameUtility {
    public class MinHeap<T> where T : IComparable<T> {
        private T[] _HeapSources;
        private int _Capacity;
        
        public  int Count { get; private set; }

        public MinHeap(int capacity = 8) {
            _Capacity    = capacity;
            _HeapSources = new T[_Capacity];
        }

        public T Peak() {
            if (Count == 0) {
                return default;
            }

            return _HeapSources[0];
        }

        public void Enqueue(T target) {
            ResizeArray(Count + 1);
            _HeapSources[Count] = target;
            Count++;
            BubbleUp(Count - 1);
        }

        public T Dequeue() {
            if (Count == 0) {
                throw new IndexOutOfRangeException("Count equals zero, no elements!");
            }

            T result = _HeapSources[0];
            if (Count == 1) {
                Count           = 0;
                _HeapSources[0] = default;
            }
            else {

                _HeapSources[0] = default;
                Swap(0, Count - 1);
                Count--;
                BubbleDown();
            }

            return result;
        }

        private void ResizeArray(int newSize) {
            if (newSize > _Capacity) {
                _Capacity *= 2;
                T[] newArray = new T[_Capacity];
                Array.Copy(_HeapSources, newArray, Count);
                _HeapSources = newArray;
            }
        }

        private void BubbleDown() {
            int parentIndex    = 0;
            int leftChildIndex = (parentIndex * 2) + 1;

            while (leftChildIndex < Count) {
                int rightChildIndex = leftChildIndex + 1;
                int   bestChildIndex       = rightChildIndex < Count && _HeapSources[rightChildIndex].CompareTo(_HeapSources[leftChildIndex]) < 0 ? rightChildIndex : leftChildIndex;
                if (_HeapSources[parentIndex].CompareTo(_HeapSources[bestChildIndex]) > 0) {
                    Swap(bestChildIndex, parentIndex);
                    parentIndex    = bestChildIndex;
                    leftChildIndex = (parentIndex * 2) + 1;
                }
                else {
                    break;
                }
            }
        }

        private void BubbleUp(int startIndex) {
            if (startIndex >= Count) {
                return;
            }
            
            while (startIndex > 0) {
                int parentIndex = (startIndex - 1) / 2;
                if (_HeapSources[parentIndex].CompareTo(_HeapSources[startIndex]) > 0) {
                    Swap(parentIndex,startIndex);
                    startIndex = parentIndex;
                }
                else {
                    break;
                }
            }
        }

        private void Swap(int indexA, int indexB) {
            T temp = _HeapSources[indexA];
            _HeapSources[indexA] = _HeapSources[indexB];
            _HeapSources[indexB] = temp;
        }
    }
}