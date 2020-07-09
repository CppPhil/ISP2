using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISP2.mnist;
using ISP2.nn;
using Image = ISP2.mnist.Image;

namespace ISP2
{
    internal class NeuralNetworkRunner
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

        private static BitmapWithValues[] Example(IProgress<long> progress, uint trainingCycles, int testsToRun,
            IEnumerable<int> hiddenLayers)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var mnistDirectory = $"{currentDirectory}/../../../data/MNIST/";

            var trainingImagesIdxFile = new IdxFile(filePath: mnistDirectory + TrainingSetImagesFile);
            var trainingLabelsIdxFile = new IdxFile(filePath: mnistDirectory + TrainingSetLabelsFile);
            var testImagesIdxFile = new IdxFile(filePath: mnistDirectory + TestSetImagesFile);
            var testLabelsIdxFile = new IdxFile(filePath: mnistDirectory + TestSetLabelsFile);

            var trainingImagesCount = trainingImagesIdxFile.DataSize() / Image.ByteCount;
            var trainingLabelsCount = trainingLabelsIdxFile.DataSize();
            var testImagesCount = Math.Min(testImagesIdxFile.DataSize() / Image.ByteCount, testsToRun);
            var testLabelsCount = Math.Min(testLabelsIdxFile.DataSize(), testsToRun);

            var trainingImages = new Image[trainingImagesCount];
            var trainingLabels = new byte[trainingLabelsCount];
            var testImages = new Image[testImagesCount];
            var testLabels = new byte[testLabelsCount];

            var totalImageCount = trainingImagesCount * trainingCycles + testImagesCount;
            var curImage = 0;

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

            var layers = new List<int> {(int) Image.FloatCount};
            layers.AddRange(hiddenLayers);
            layers.Add(digits);

            var network = new NeuralNetwork(layers: layers);

            for (uint i = 0; i < trainingCycles; ++i)
            {
                for (var j = 0; j < trainingImagesCount; ++j)
                {
                    network.FeedForward(inputs: trainingImages[j].ToFloatArray());
                    network.BackPropagation(expected: LabelToFloatArray(label: trainingLabels[j]));

                    progress?.Report(new ProgressReport(curImage, (int) totalImageCount).ToLong());
                    ++curImage;
                }
            }

            // Test phase
            var retVal = new BitmapWithValues[testImagesCount];

            for (uint i = 0; i < testImagesCount; ++i)
            {
                var ary = network.FeedForward(inputs: testImages[i].ToFloatArray());
                var label = testLabels[i];

                var zeroPercentage = 0.0F;
                var onePercentage = 0.0F;
                var twoPercentage = 0.0F;
                var threePercentage = 0.0F;
                var fourPercentage = 0.0F;
                var fivePercentage = 0.0F;
                var sixPercentage = 0.0F;
                var sevenPercentage = 0.0F;
                var eightPercentage = 0.0F;
                var ninePercentage = 0.0F;

                for (var j = 0; j < ary.Length; ++j)
                {
                    var currentValue = (float) Math.Round(value: ary[j], digits: 4);

                    switch (j)
                    {
                        case 0:
                            zeroPercentage = currentValue;
                            break;
                        case 1:
                            onePercentage = currentValue;
                            break;
                        case 2:
                            twoPercentage = currentValue;
                            break;
                        case 3:
                            threePercentage = currentValue;
                            break;
                        case 4:
                            fourPercentage = currentValue;
                            break;
                        case 5:
                            fivePercentage = currentValue;
                            break;
                        case 6:
                            sixPercentage = currentValue;
                            break;
                        case 7:
                            sevenPercentage = currentValue;
                            break;
                        case 8:
                            eightPercentage = currentValue;
                            break;
                        case 9:
                            ninePercentage = currentValue;
                            break;
                    }
                }

                var expected = (int) label;
                var bitmap = testImages[i].ToBitmap();

                retVal[i] = new BitmapWithValues(bitmap, expected, zeroPercentage, onePercentage, twoPercentage,
                    threePercentage, fourPercentage, fivePercentage, sixPercentage, sevenPercentage, eightPercentage,
                    ninePercentage);

                progress?.Report(new ProgressReport(curImage, (int) totalImageCount).ToLong());
                ++curImage;
            }

            progress?.Report(new ProgressReport((int) totalImageCount, (int) totalImageCount).ToLong());

            return retVal;
        }

        public static async Task<BitmapWithValues[]> RunNeuralNetwork(IProgress<long> progress, uint trainingCycles,
            int testsToRun, IReadOnlyList<int> hiddenLayers)
        {
            return await Task.Run(() => Example(progress, trainingCycles, testsToRun, hiddenLayers));
        }
    }
}