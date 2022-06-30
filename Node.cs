using System;


namespace Huffman
{
    class Node : IComparable<Node>
    {
        public byte Symbol;
        public uint Frequency;
        public Node Prev;
        public Node Left, Right;

        public Node(byte symbol, uint freq, Node l = null, Node r = null, Node p = null)
        {
            Symbol = symbol;
            Frequency = freq;
            Left = l;
            Right = r;
            Prev = p;
        }

        public Node Join(Node x)
        {
            return new Node(Symbol, Frequency + x.Frequency, x, this);
        }

        public int CompareTo(Node x)
        {
            if (Frequency < x.Frequency)
                return 1;
            else if (Frequency > x.Frequency)
                return -1;
            else
                return 0;
        }

        static public Node Builder(Heap<Node> tree)
        {
            while (tree.Count > 1)
            {
                Node node = tree.Extract();
                tree.Add(node.Join(tree.Extract()));
            }
            return tree.Extract();
        }
    }
}
