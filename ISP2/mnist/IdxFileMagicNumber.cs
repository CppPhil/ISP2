namespace ISP2.mnist
{
    public class IdxFileMagicNumber
    {
        private byte _firstZeroByte;
        private byte _secondZeroByte;

        public byte DataKind { get; }

        public byte Dimensions { get; }

        public static readonly byte KindUnsignedByte = 0x08;
        public static readonly byte KindSignedByte = 0x09;
        public static readonly byte KindShort = 0x0B;
        public static readonly byte KindInt = 0x0C;
        public static readonly byte KindFloat = 0x0D;
        public static readonly byte KindDouble = 0x0E;

        public IdxFileMagicNumber(byte firstZeroByte, byte secondZeroByte, byte dataKind, byte dimensions)
        {
            _firstZeroByte = firstZeroByte;
            _secondZeroByte = secondZeroByte;
            DataKind = dataKind;
            Dimensions = dimensions;
        }
    }
}