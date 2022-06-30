using System;
using System.Collections.Generic;
using System.IO;



namespace Huffman
{
    public partial class Encoder
    {
        //string Input, Output;

        public long Total { get => total;  }

        Byte[] buffer;
        BinaryReader reader;
        BinaryWriter writer;

        Node node;
        
        Heap<Node> Tree;
        Dictionary<Byte, uint> FrequencyTable;
        OpCode[] OpCodes;
        int NodeCount;

        public byte[] Buffer
        {
            get => buffer;
        }


        public Encoder()
        {
           
            //Tree = new Heap<Node>();

           // FrequencyTable = new Dictionary<byte, uint>();
           // OpCodes = new OpCode[256];
        }

        public void EncodeString(string data) 
        {
            Clear();
            byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(data);
           
            using(reader = new BinaryReader(new MemoryStream(inputBuffer))) 
            {
                FillFrequencyTable();
                Emplace();
                node = Node.Builder(Tree);
                MakeOpcodes(node, 0, 0);

                using (writer = GetWriter(inputBuffer.Length + header_size)) 
                {
                    WriteHeader();
                    Encode();
                    Resize();
                }
            }
            
        }

        public void EncodeFile(string input) 
        {
            Clear();
            using (reader = new BinaryReader(new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                FillFrequencyTable();
                Emplace();
                node = Node.Builder(Tree);
                MakeOpcodes(node, 0, 0);

                using (writer = GetWriter(reader.BaseStream.Length + header_size))
                {
                    WriteHeader();
                    Encode();
                    Resize();
                }
            }
        }
       

        BinaryWriter GetWriter(long size)
        {
            if (size == 0) return null;

            buffer = new byte[size];
            return new BinaryWriter(new MemoryStream(buffer));
        }

        void Resize()
        {
            try
            {
                Array.Resize(ref buffer, (int)(total + header_size));
            }
            catch (Exception)
            {

            }
        }

        public bool WriteInFile(string output)
        {
            try
            {
                using (writer = new BinaryWriter(new FileStream(output, FileMode.Create, FileAccess.Write)))
                {
                    //Array.Resize(ref buffer, (int) total);
                    writer.Write(buffer);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void Clear()
        {
            node = null;
            Tree = new Heap<Node>();
            OpCodes = new OpCode[256];
            FrequencyTable = new Dictionary<byte, uint>();
            buffer = null;
            total = 0;
            header_size = 0;
        }



    }
}
