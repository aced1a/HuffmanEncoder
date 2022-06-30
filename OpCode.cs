

namespace Huffman
{
    struct OpCode
    {
        public ulong Opcode;
        public ulong Len;

        public OpCode(ulong code, ulong len)
        {
            Opcode = code;
            Len = len;
        }
    }
}
