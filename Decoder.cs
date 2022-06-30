using System;
using System.IO;


namespace Huffman
{
    public partial class Decoder
    {
       // string Input, Output;
        long fileSize;

        Byte[] buffer;
        BinaryReader reader;
        BinaryWriter writer;

        Node node;

        public byte[] Buffer
        {
            get => buffer;
        }

        public Decoder()//string in_file, string out_file)
        {
            //Input = in_file;
            //Output = out_file;

        }

        public bool DecodeString(string data)
        {
            Clear();
            byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(data);

            using (reader = new BinaryReader(new MemoryStream(inputBuffer)))
            {
                if (ReadHeader())
                {
                    using (writer = GetWriter())
                    {
                        if (writer == null) return false;

                        Decode();
                        //Resize();
                    }
                }
                else
                    return false;
            }
            return true;
        }

        public bool DecodeFile(string input)
        {
            Clear();
            using (reader = new BinaryReader(new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                if (ReadHeader())
                {
                    using (writer = GetWriter())
                    {
                        if (writer == null) return false;

                        Decode();
                        //Resize();
                    }
                }
                else
                    return false;
            }
            return true;
        }

        BinaryWriter GetWriter()
        {
            if (fileSize == 0) return null;

            buffer = new byte[fileSize];
            return new BinaryWriter(new MemoryStream(buffer));
        }

        //void Resize()
        //{
        //    try
        //    {
        //        Array.Resize(ref buffer, (int)total);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}


        public bool WriteInFile(string output)
        {
            try
            {
                using (writer = new BinaryWriter(new FileStream(output, FileMode.Create, FileAccess.Write)))
                {
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
            fileSize = 0;
            node = null;
            buffer = null;
            total = 0;
        }

    }
}


