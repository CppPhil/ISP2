using System;

namespace ISP2.nn
{
    public class NeuralLayer
    {
        private float[] _inputs; // Neurons in the previous layer.
        private readonly float[,] _weightsDelta;

        public float[] Outputs { get; } // Neurons in the current layer.
        public float[,] Weights { get; }
        public float[] Gamma { get; }

        private const float LearningRate = 0.01f;


        /// <summary>
        ///   Create a new layer.
        /// </summary>
        /// <param name="nrInputs">Number of ingoing connections (#neurons of previous layer).</param>
        /// <param name="nrOutputs">Number of outgoing connections (#neurons in this layer).</param>
        public NeuralLayer(int nrInputs, int nrOutputs)
        {
            _inputs = new float[nrInputs];
            Outputs = new float[nrOutputs];
            Weights = new float[nrOutputs, nrInputs];
            _weightsDelta = new float[nrOutputs, nrInputs];
            Gamma = new float[nrOutputs];

            // Initialize the weights (random number between -0.5 and 0.5).
            var random = new Random();
            for (var i = 0; i < Outputs.Length; i++)
            {
                for (var j = 0; j < _inputs.Length; j++)
                {
                    Weights[i, j] = (float) random.NextDouble() - 0.5f;
                }
            }
        }


        /// <summary>
        ///   Feed-forward function for the layer. It calculates the weighted
        ///   input and calls the activation function (=output) for all neurons.
        /// </summary>
        /// <param name="inputs">List of input values.</param>
        public void FeedForward(float[] inputs)
        {
            _inputs = inputs;
            for (var i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = 0;
                for (var j = 0; j < _inputs.Length; j++)
                {
                    Outputs[i] += _inputs[j] * Weights[i, j];
                }

                // Outputs[i] = (float) Math.Tanh(value: Outputs[i]);
                Outputs[i] = (float) Sigmoid(value: Outputs[i]);
            }
        }


        /// <summary>
        ///   Back-propagation for the output layer.
        ///   This methods calculates the weight deltas based on the output deviation.
        /// </summary>
        /// <param name="expected">Desired output.</param>
        public void BackPropagationOutputLayer(float[] expected)
        {
            for (var i = 0; i < Outputs.Length; i++)
            {
                var outputPrime = 1 - Outputs[i] * Outputs[i]; // tanh'
                var costPrime = Outputs[i] - expected[i]; // mse'
                Gamma[i] = costPrime * outputPrime;
                for (var j = 0; j < _inputs.Length; j++)
                {
                    _weightsDelta[i, j] = Gamma[i] * _inputs[j];
                }
            }
        }


        /// <summary>
        ///   Back-propagation for a hidden (or input) layer. This methods calculates
        ///   the weight deltas based on the gammas and deltas of the succeeding layer.
        /// </summary>
        /// <param name="nextLayer">Reference to the next layer.</param>
        public void BackPropagationHiddenLayer(NeuralLayer nextLayer)
        {
            for (var i = 0; i < Outputs.Length; i++)
            {
                Gamma[i] = 0;
                for (var j = 0; j < nextLayer.Gamma.Length; j++)
                {
                    Gamma[i] += nextLayer.Gamma[j] * nextLayer.Weights[j, i];
                }

                var outputPrime = 1 - Outputs[i] * Outputs[i]; // tanh'
                Gamma[i] *= outputPrime;
                for (var j = 0; j < _inputs.Length; j++)
                {
                    _weightsDelta[i, j] = Gamma[i] * _inputs[j];
                }
            }
        }


        /// <summary>
        ///   Update the weights of this layer based on the deltas and the learning rate.
        /// </summary>
        public void UpdateWeights()
        {
            for (var i = 0; i < Outputs.Length; i++)
            {
                for (var j = 0; j < _inputs.Length; j++)
                {
                    Weights[i, j] -= _weightsDelta[i, j] * LearningRate;
                }
            }
        }

        private static double Sigmoid(double value)
        {
            return 1.0 / (1.0 + Math.Exp(-value));
        }
    }
}