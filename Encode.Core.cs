using System;
using System.IO;


namespace Huffman
{
    partial class Encoder
    {
        long total;
        int header_size;

        void FillFrequencyTable()
        {
            Byte symbol;
            while (ReadByte(out symbol) != -1)
            {
                if (FrequencyTable.ContainsKey(symbol))
                {
                    FrequencyTable[symbol]++;
                }
                else
                {
                    FrequencyTable.Add(symbol, 1);
                }
            }
            reader.BaseStream.Position = 0;
        }

        void Emplace()
        {
            foreach (var symbol in FrequencyTable)
            {
                Tree.Add
                (
                    new Node(symbol.Key, symbol.Value)
                );
            }
        }

        void MakeOpcodes(Node node, ulong code, ulong len)
        {
            if (node.Left == null && node.Right == null)
            {
                int mod = (int)(32 - len);
                OpCodes[node.Symbol] = new OpCode(code >> mod, len);
            }
            else
            {
                code >>= 1;
                len++;

                MakeOpcodes(node.Left, code, len);
                MakeOpcodes(node.Right, code | 0x80000000, len);
            }
            NodeCount++;
        }

        void WriteHeader()
        {
            writer.Write(NodeCount);
            WriteTree(node);
            writer.Write((short)(90 | (83 << 8)));
            writer.Write(reader.BaseStream.Length);
            header_size += (4 + 2 + 8);
        }

        void WriteTree(Node node)
        {
            writer.Write(node.Symbol);
            header_size++;

            if (node.Left != null)
            {
                writer.Write((byte)76);
                header_size++;
                WriteTree(node.Left);
            }
            if (node.Right != null)
            {
                writer.Write((byte)82);
                header_size++;
                WriteTree(node.Right);
            }
        }

        void Encode()
        {
            int outBitsCount = 0;
            Byte outByte = 0, symbol = 0;

            while (ReadByte(out symbol) != -1)
            {
                PutCode(symbol, ref outBitsCount, ref outByte);
            }
            EndPut(outBitsCount, outByte);
        }

        void PutCode(Byte symbol, ref int bitsCount, ref Byte outByte)
        {
            ulong opcode = OpCodes[symbol].Opcode;
            ulong len = OpCodes[symbol].Len;

            while (len > 0)
            {
                while (bitsCount < 8 && len > 0)
                {
                    outByte >>= 1;
                    outByte |= (Byte)((opcode & 1) << 7);
                    opcode >>= 1;

                    len--;
                    bitsCount++;
                }
                if (bitsCount == 8)
                {
                    writer.Write(outByte);
                    outByte = 0;
                    bitsCount = 0;
                    total++;
                }
            }
        }

        void EndPut(int bitsCount, Byte outByte)
        {
            if (bitsCount != 0)
            {
                outByte >>= (8 - bitsCount);
                writer.Write(outByte);
                total++;
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
