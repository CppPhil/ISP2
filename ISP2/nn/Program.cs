using System;
using System.IO;
using NeuralNetwork.mnist;

namespace NeuralNetwork
{
    public static class Program
    {
        private const string TrainingSetImagesFile = "train-images.idx3-ubyte";
        private const string TrainingSetLabelsFile = "train-labels.idx1-ubyte";
        private const string TestSetImagesFile = "t10k-images.idx3-ubyte";
        private const string TestSetLabelsFile = "t10k-labels.idx1-ubyte";

        private static float[] LabelToFloatArray(byte label)
        {
            const uint digitCount = 10;
            var retVal = new float[digitCount];

            for (uint i = 0; i < digitCount; ++i)
            {
                retVal[i] = 0.0F;
            }

            retVal[label] = 1.0F;

            return retVal;
        }

        private static void Main()
        {
            Example01();
            Console.WriteLine(value: "");
            Example();
        }

        private static void Example()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var mnistDirectory = $"{currentDirectory}/data/MNIST/";

            var trainingImagesIdxFile = new IdxFile(filePath: mnistDirectory + TrainingSetImagesFile);
            var trainingLabelsIdxFile = new IdxFile(filePath: mnistDirectory + TrainingSetLabelsFile);
            var testImagesIdxFile = new IdxFile(filePath: mnistDirectory + TestSetImagesFile);
            var testLabelsIdxFile = new IdxFile(filePath: mnistDirectory + TestSetLabelsFile);

            var trainingImagesCount = trainingImagesIdxFile.DataSize() / Image.ByteCount;
            var trainingLabelsCount = trainingLabelsIdxFile.DataSize();
            var testImagesCount = testImagesIdxFile.DataSize() / Image.ByteCount;
            var testLabelsCount = testLabelsIdxFile.DataSize();

            var trainingImages = new Image[trainingImagesCount];
            var trainingLabels = new byte[trainingLabelsCount];
            var testImages = new Image[testImagesCount];
            var testLabels = new byte[testLabelsCount];

            for (uint i = 0; i < trainingImagesCount; ++i)
            {
                trainingImages[i] = new Image(idxFile: trainingImagesIdxFile, offset: i * Image.ByteCount);
            }

            for (uint i = 0; i < trainingLabelsCount; ++i)
            {
                trainingLabels[i] = trainingLabelsIdxFile.DataAt(index: i);
            }

            for (uint i = 0; i < testImagesCount; ++i)
            {
                testImages[i] = new Image(idxFile: testImagesIdxFile, offset: i * Image.ByteCount);
            }

            for (uint i = 0; i < testLabelsCount; ++i)
            {
                testLabels[i] = testLabelsIdxFile.DataAt(index: i);
            }

            const int digits = 10;
            var network = new NeuralNetwork(layers: new int[] {(int) Image.FloatCount, 25, digits});

            for (var i = 0; i < 1; ++i)
            {
                for (var j = 0; j < trainingImagesCount; ++j)
                {
                    network.FeedForward(inputs: trainingImages[j].ToFloatArray());
                    network.BackPropagation(expected: LabelToFloatArray(label: trainingLabels[j]));
                }
            }

            // Test phase
            for (uint i = 0; i < testImagesCount; ++i)
            {
                var ary = network.FeedForward(inputs: testImages[i].ToFloatArray());
                var label = testLabels[i];

                for (var j = 0; j < ary.Length; ++j)
                {
                    Console.WriteLine(value: $"{j}: {Math.Round(value: ary[j], digits: 4)}");
                }

                Console.WriteLine(value: $"Expected: {(int) label}");
                Console.WriteLine(value: "");
            }
        }

        private static void Example01()
        {
            /* Dreifaches X-OR. Wahr, wenn 1 oder 3 Bits gesetzt sind.
             * 0 0 0   => 0
             * 0 0 1   => 1
             * 0 1 0   => 1
             * 0 1 1   => 0
             * 1 0 0   => 1
             * 1 0 1   => 0
             * 1 1 0   => 0
             * 1 1 1   => 1
             */

            var network = new NeuralNetwork(layers: new[] {3, 25, 1});

            for (var i = 0; i < 10000; i++)
            {
                network.FeedForward(inputs: new float[] {0, 0, 0});
                network.BackPropagation(expected: new float[] {0});
                network.FeedForward(inputs: new float[] {0, 0, 1});
                network.BackPropagation(expected: new float[] {1});
                network.FeedForward(inputs: new float[] {0, 1, 0});
                network.BackPropagation(expected: new float[] {1});
                network.FeedForward(inputs: new float[] {0, 1, 1});
                network.BackPropagation(expected: new float[] {0});
                network.FeedForward(inputs: new float[] {1, 0, 0});
                network.BackPropagation(expected: new float[] {1});
                network.FeedForward(inputs: new float[] {1, 0, 1});
                network.BackPropagation(expected: new float[] {0});
                network.FeedForward(inputs: new float[] {1, 1, 0});
                network.BackPropagation(expected: new float[] {0});
                network.FeedForward(inputs: new float[] {1, 1, 1});
                network.BackPropagation(expected: new float[] {1});
            }


            // Output after learning.
            Console.WriteLine(value: "0,0,0  [0]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {0, 0, 0})[0],
                                         digits: 4));
            Console.WriteLine(value: "0,0,1  [1]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {0, 0, 1})[0],
                                         digits: 4));
            Console.WriteLine(value: "0,1,0  [1]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {0, 1, 0})[0],
                                         digits: 4));
            Console.WriteLine(value: "0,1,1  [0]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {0, 1, 1})[0],
                                         digits: 4));
            Console.WriteLine(value: "1,0,0  [1]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {1, 0, 0})[0],
                                         digits: 4));
            Console.WriteLine(value: "1,0,1  [0]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {1, 0, 1})[0],
                                         digits: 4));
            Console.WriteLine(value: "1,1,0  [0]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {1, 1, 0})[0],
                                         digits: 4));
            Console.WriteLine(value: "1,1,1  [1]: " +
                                     Math.Round(value: network.FeedForward(inputs: new float[] {1, 1, 1})[0],
                                         digits: 4));
        }
    }
}