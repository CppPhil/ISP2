using System;
using System.IO;

namespace ISP2.mnist
{
    public class IdxFile
    {
        private readonly IdxFileMagicNumber _magicNumber;
        private readonly uint[] _dimensionSizes;
        private readonly byte[] _data;

        private static IdxFileMagicNumber ReadMagicNumber(BinaryReader reader)
        {
            var byte1 = reader.ReadByte();
            var byte2 = reader.ReadByte();
            var byte3 = reader.ReadByte();
            var byte4 = reader.ReadByte();

            return new IdxFileMagicNumber(firstZeroByte: byte1, secondZeroByte: byte2, dataKind: byte3,
                dimensions: byte4);
        }

        private static uint ByteSwap(uint v)
        {
            return ((v & 0xFF) << 24) |
                   ((v & 0xFF00) << 8) |
                   ((v & 0xFF0000) >> 8) |
                   ((v & 0xFF000000) >> 24);
        }

        private static uint ReadUint32(BinaryReader reader)
        {
            // The file contains big endian data
            var uint32 = reader.ReadUInt32();

            // If we're little endian we need to turn the bytes around
            if (BitConverter.IsLittleEndian)
            {
                uint32 = ByteSwap(v: uint32);
            }

            return uint32;
        }

        private byte Dimensions()
        {
            return _magicNumber.Dimensions;
        }

        private uint SizeInDimension(byte dimension)
        {
            return _dimensionSizes[dimension];
        }

        private uint[] ReadDimensionSizes(BinaryReader reader)
        {
            var retVal = new uint[Dimensions()];

            for (uint i = 0; i < Dimensions(); ++i)
            {
                retVal[i] = ReadUint32(reader: reader);
            }

            return retVal;
        }

        private byte[] ReadData(BinaryReader reader)
        {
            long bytesToRead = 1;

            for (byte i = 0; i < Dimensions(); ++i)
            {
                bytesToRead *= SizeInDimension(dimension: i);
            }

            var retVal = new byte[bytesToRead];

            for (long i = 0; i < bytesToRead; ++i)
            {
                retVal[i] = reader.ReadByte();
            }

            return retVal;
        }

        public IdxFile(string filePath)
        {
            if (!File.Exists(path: filePath))
            {
                throw new FileNotFoundException(message: $"\"{filePath}\" does not exist!");
            }

            using (var reader = new BinaryReader(input: File.Open(path: filePath, mode: FileMode.Open)))
            {
                _magicNumber = ReadMagicNumber(reader: reader);
                _dimensionSizes = ReadDimensionSizes(reader: reader);
                _data = ReadData(reader: reader);
            }
        }

        public byte DataKind()
        {
            return _magicNumber.DataKind;
        }

        public byte DataAt(uint index)
        {
            return _data[index];
        }

        public int DataSize()
        {
            return _data.Length;
        }
    }
}