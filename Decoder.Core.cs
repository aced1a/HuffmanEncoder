using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    partial class Decoder
    {
        long total;

        bool ReadHeader()
        {
            uint nodesCount = 0;
            try
            {
                nodesCount = reader.ReadUInt32();
                node = new Node(reader.ReadByte(), 0);
            }
            catch (Exception)
            {
                return false;
            }

            return ReadTree(node, nodesCount - 1) && ReadSize(out fileSize);
        }

        bool ReadTree(Node node, uint nodesCount)
        {
            Byte symbol = 0, desc = 0;


            while (nodesCount > 0)
            {
                if (ReadByte(out desc) == -1 || ReadByte(out symbol) == -1)
                    return false;

                if (desc == 76)
                {
                    node.Left = new Node(symbol, 0);
                    node.Left.Prev = node;
                    node = node.Left;
                }
                else if (desc == 82)
                {
                    while (node.Prev.Right != null) node = node.Prev;

                    node.Prev.Right = new Node(symbol, 0);
                    node.Prev.Right.Prev = node.Prev;
                    node = node.Prev.Right;
                }
                else
                    return false;
                nodesCount--;
            }
            return true;
        }

        bool ReadSize(out long size)
        {
            size = 0;
            try
            {
                short flag = reader.ReadInt16();
                if (flag == (90 | (83 << 8)))
                {
                    size = reader.ReadInt64();
                }
                else
                {
                    reader.BaseStream.Position -= 2;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void Decode()
        {
            Node p = node;

            Byte a = 0;
            int count = 0, flag = 0;
            Byte b = 0;

            flag = ReadByte(out a);

            while (flag != -1)
            {

                b = (byte)(a & 1);
                a >>= 1;
                count++;

                if (b == 1)
                {
                    p = p.Right;
                }
                else
                {
                    p = p.Left;
                }

                if (p.Left == null && p.Right == null)
                {

                    if (WriteByte(p.Symbol) == -1) return; 
                    //writer.Write(p.Symbol);
                    p = node;
                    total++;
                }

                if (count == 8)
                {
                    count = 0;
                    flag = ReadByte(out a);
                }
            }
        }

        int WriteByte(byte b)
        {
            try
            {
                writer.Write(b);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        int ReadByte(out Byte b)
        {
            b = 0;
            try
            {
                b = reader.ReadByte();
                return 1;
            }
            catch (EndOfStreamException)
            {
                return -1;
            }
        }
    }
}
