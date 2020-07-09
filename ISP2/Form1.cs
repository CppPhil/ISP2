using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ISP2
{
    public partial class Form1 : Form
    {
        private const int BitmapColumn = 0;
        private const int ExpectedColumn = 1;
        private const int ZeroPercentageColumn = 2;
        private const int OnePercentageColumn = 3;
        private const int TwoPercentageColumn = 4;
        private const int ThreePercentageColumn = 5;
        private const int FourPercentageColumn = 6;
        private const int FivePercentageColumn = 7;
        private const int SixPercentageColumn = 8;
        private const int SevenPercentageColumn = 9;
        private const int EightPercentageColumn = 10;
        private const int NinePercentageColumn = 11;

        private readonly Predicate<char> isNonNumeric = character => character != '0'
                                                                     && character != '1'
                                                                     && character != '2'
                                                                     && character != '3'
                                                                     && character != '4'
                                                                     && character != '5'
                                                                     && character != '6'
                                                                     && character != '7'
                                                                     && character != '8'
                                                                     && character != '9';

        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;

            const int columns = 12;
            tableLayoutPanel.ColumnCount = columns;
            tableLayoutPanel.RowCount = 1;
            var bitmapLabel = new Label {Text = "image"};
            var expectedLabel = new Label {Text = "expected"};
            var zeroPercentageLabel = new Label {Text = "0"};
            var onePercentageLabel = new Label {Text = "1"};
            var twoPercentageLabel = new Label {Text = "2"};
            var threePercentageLabel = new Label {Text = "3"};
            var fourPercentageLabel = new Label {Text = "4"};
            var fivePercentageLabel = new Label {Text = "5"};
            var sixPercentageLabel = new Label {Text = "6"};
            var sevenPercentageLabel = new Label {Text = "7"};
            var eightPercentageLabel = new Label {Text = "8"};
            var ninePercentageLabel = new Label {Text = "9"};

            var c = tableLayoutPanel.Controls;
            const int row = 0;
            c.Add(bitmapLabel, BitmapColumn, row);
            c.Add(expectedLabel, ExpectedColumn, row);
            c.Add(zeroPercentageLabel, ZeroPercentageColumn, row);
            c.Add(onePercentageLabel, OnePercentageColumn, row);
            c.Add(twoPercentageLabel, TwoPercentageColumn, row);
            c.Add(threePercentageLabel, ThreePercentageColumn, row);
            c.Add(fourPercentageLabel, FourPercentageColumn, row);
            c.Add(fivePercentageLabel, FivePercentageColumn, row);
            c.Add(sixPercentageLabel, SixPercentageColumn, row);
            c.Add(sevenPercentageLabel, SevenPercentageColumn, row);
            c.Add(eightPercentageLabel, EightPercentageColumn, row);
            c.Add(ninePercentageLabel, NinePercentageColumn, row);
        }

        private static void Add(TableLayoutControlCollection controls, Image image, int column, int row)
        {
            controls.Add(new PictureBox {Image = image}, column, row);
        }

        private static void Add(TableLayoutControlCollection controls, int value, int column, int row)
        {
            controls.Add(new Label {Text = value.ToString()}, column, row);
        }

        private static void Add(TableLayoutControlCollection controls, float value, int column, int row)
        {
            controls.Add(new Label {Text = value.ToString(CultureInfo.CurrentCulture)}, column, row);
        }

        private async void ButtonRun_Click(object sender, EventArgs e)
        {
            var progress = new Progress<long>(value =>
            {
                labelProgress.Text
                    = $"processed {value >> 32} of {(int) value} images ({(int) ((double) (value >> 32) / (int) value * 100.0)}%)";
                labelProgress.Refresh();
                //Application.DoEvents();
            });
            buttonRun.Enabled = false;

            const int defaultTrainingCycles = 2;
            const int defaultTestsToRun = 500;

            var bitmapWithValues =
                await NeuralNetworkRunner.RunNeuralNetwork(progress,
                    (uint) ToInt.ToInteger(textBoxTrainingCycles.Text, defaultTrainingCycles),
                    ToInt.ToInteger(textBoxTestsToRun.Text, defaultTestsToRun),
                    HiddenLayersTextToList(textBoxHiddenLayers.Text));

            var rowCount = bitmapWithValues.Length + 1;
            tableLayoutPanel.RowCount = rowCount;
            var c = tableLayoutPanel.Controls;

            var correctlyDetectedBitmapCount = 0;

            for (var i = 0; i < bitmapWithValues.Length; ++i)
            {
                labelProgress.Text =
                    $"rendering test image {i + 1} of {bitmapWithValues.Length} ({(int) ((double) (i + 1) / bitmapWithValues.Length * 100.0)}%)";
                Application.DoEvents();
                var cur = bitmapWithValues[i];

                if (ClosestToOne(new[]
                {
                    cur.ZeroPercentage, cur.OnePercentage, cur.TwoPercentage, cur.ThreePercentage, cur.FourPercentage,
                    cur.FivePercentage, cur.SixPercentage, cur.SevenPercentage, cur.EightPercentage, cur.NinePercentage
                }) == cur.Expected)
                    ++correctlyDetectedBitmapCount;

                Add(c, cur.Bitmap, BitmapColumn, i + 1);
                Add(c, cur.Expected, ExpectedColumn, i + 1);
                Add(c, cur.ZeroPercentage, ZeroPercentageColumn, i + 1);
                Add(c, cur.OnePercentage, OnePercentageColumn, i + 1);
                Add(c, cur.TwoPercentage, TwoPercentageColumn, i + 1);
                Add(c, cur.ThreePercentage, ThreePercentageColumn, i + 1);
                Add(c, cur.FourPercentage, FourPercentageColumn, i + 1);
                Add(c, cur.FivePercentage, FivePercentageColumn, i + 1);
                Add(c, cur.SixPercentage, SixPercentageColumn, i + 1);
                Add(c, cur.SevenPercentage, SevenPercentageColumn, i + 1);
                Add(c, cur.EightPercentage, EightPercentageColumn, i + 1);
                Add(c, cur.NinePercentage, NinePercentageColumn, i + 1);
            }

            labelStatistics.Text =
                $"correctly classified {correctlyDetectedBitmapCount} of {bitmapWithValues.Length} ({(double) correctlyDetectedBitmapCount / bitmapWithValues.Length * 100.0}%)";

            tableLayoutPanel.ColumnStyles.Clear();
            for (var i = 0; i < tableLayoutPanel.ColumnCount; i++)
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            tableLayoutPanel.RowStyles.Clear();
            for (var i = 0; i < tableLayoutPanel.RowCount; i++)
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tableLayoutPanel.Refresh();
        }

        private static void DeleteByPredicate(Control control, Predicate<char> predicate)
        {
            var currentText = control.Text;
            var list = currentText.ToList();
            list.RemoveAll(predicate);

            var sb = new StringBuilder();
            foreach (var c in list) sb.Append(c);

            control.Text = sb.ToString();
        }

        private void DeleteNonNumeric(Control textBox)
        {
            DeleteByPredicate(textBox, isNonNumeric);
        }

        private void TextBoxTrainingCycles_TextChanged(object sender, EventArgs e)
        {
            DeleteNonNumeric(textBoxTrainingCycles);
        }

        private void TextBoxTestsToRun_TextChanged(object sender, EventArgs e)
        {
            DeleteNonNumeric(textBoxTestsToRun);
        }

        private void TextBoxHiddenLayers_TextChanged(object sender, EventArgs e)
        {
            DeleteByPredicate(textBoxHiddenLayers, character => isNonNumeric(character) && character != ',');
        }

        private static List<int> HiddenLayersTextToList(string text)
        {
            var ary = text.Split(',');

            var result = new List<int>();

            foreach (var s in ary)
                if (int.TryParse(s, out var value))
                    result.Add(value);

            if (result.Count == 0) result.Add(25);

            return result;
        }

        private static int ClosestToOne(IReadOnlyList<float> floats)
        {
            const float one = 1.0F;

            var index = 0;

            for (var i = 0; i < floats.Count; ++i)
                if (one - floats[i] < one - floats[index])
                    index = i;

            return index;
        }
    }
}