using System;
using System.Drawing;

namespace ISP2.mnist
{
    public class Image
    {
        private static float MnistPixelToFloat(byte mnistPixelValue)
        {
            return mnistPixelValue / 255.0F;
        }

        private byte[] Data { get; }

        private const uint RowCount = 28;
        private const uint ColumnCount = 28;

        public const uint ByteCount = RowCount * ColumnCount;
        public const uint FloatCount = ByteCount;

        public Image(IdxFile idxFile, uint offset)
        {
            Data = new byte[ByteCount];

            for (uint i = 0; i < ByteCount; ++i)
            {
                Data[i] = idxFile.DataAt(index: offset + i);
            }
        }

        public Image(byte[] data)
        {
            if (data.Length != ByteCount)
            {
                throw new ArgumentException(message: "Unexpected byte array length for Image");
            }

            Data = data;
        }

        public float[] ToFloatArray()
        {
            var result = new float[FloatCount];

            for (var i = 0; i < FloatCount; ++i)
            {
                result[i] = MnistPixelToFloat(mnistPixelValue: Data[i]);
            }

            return result;
        }

        public Bitmap ToBitmap()
        {
            var image = new Bitmap(
                width: (int)ColumnCount, height: (int)RowCount);

            for (var currentRowIndex = 0; currentRowIndex < (int) RowCount; ++currentRowIndex)
            {
                for (var currentColumnIndex = 0; currentColumnIndex < (int) ColumnCount; ++currentColumnIndex)
                {
                    var value = 255 - Data[currentColumnIndex + (currentRowIndex * ColumnCount)];
                    var color = Color.FromArgb(red: value, green: value, blue: value);
                    image.SetPixel(currentColumnIndex, currentRowIndex, color);
                }
            }

            return image;
        }
    }
}