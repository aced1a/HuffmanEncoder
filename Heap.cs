using System;
using System.Collections.Generic;


namespace Huffman
{
    class Heap<T> 
        where T : IComparable<T>
    {
        List<T> data;

        public int Count
        {
            get => data.Count;
        }


        public Heap()
        {
            data = new List<T>();
        }

        public void Add(T value)
        {
            data.Add(value);
            ShiftUp();
        }

        public T Extract()
        {
            T first = data[0];

            data[0] = data[data.Count - 1];;
            data.RemoveAt(data.Count - 1);

            if (data.Count != 0)
                ShiftDown();

            return first;
        }

        private void ShiftUp()
        {
            int index = data.Count - 1;
            int parent = (index - 1) / 2;

            while (index > 0 && data[index].CompareTo(data[parent]) > 0) 
            {
                T temp = data[index];
                data[index] = data[parent];
                data[parent] = temp;

                index = parent;
                parent = (index - 1) / 2;
            }
        }


        private void ShiftDown()
        {
            int index = 0, max_index = 0;
            T temp;

            max_index = MaxChildIndex(index);

            while(index*2+1 < data.Count && data[index].CompareTo(data[max_index]) < 0) 
            {
                temp = data[index];
                data[index] = data[max_index];
                data[max_index] = temp;
                index = max_index;

                max_index = MaxChildIndex(index);
            }
        }

        private int MaxChildIndex(int index)
        {
            if(index*2 + 1 >= data.Count) {
                return index;
            }
            if(index*2 + 2 >= data.Count) {
                return (index * 2 + 1);
            }
            return data[index * 2 + 1].CompareTo(data[index*2+2]) > 0 ? index * 2 + 1 : index * 2 + 2;
        }

    }
}
