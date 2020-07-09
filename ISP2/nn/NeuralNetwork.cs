using System.Collections.Generic;

namespace ISP2.nn
{
    /// <summary>
    ///   A simple neural network in C#. Based on the following tutorial:
    ///   https://www.youtube.com/watch?v=L_PByyJ9g-I
    /// </summary>
    public class NeuralNetwork
    {
        private readonly NeuralLayer[] _layers;


        /// <summary>
        ///   Create a new network.
        /// </summary>
        /// <param name="layers">A layer-based list containing the number of neurons in that layer.</param>
        public NeuralNetwork(IReadOnlyList<int> layers)
        {
            _layers = new NeuralLayer[layers.Count - 1];
            for (var i = 0; i < _layers.Length; i++)
            {
                _layers[i] = new NeuralLayer(nrInputs: layers[index: i], nrOutputs: layers[index: i + 1]);
            }
        }


        /// <summary>
        ///   Apply a set of inputs to the network and feed them forward through all layers.
        /// </summary>
        /// <param name="inputs">Inputs for the neurons of the first layer.</param>
        /// <returns>The results of the output layer.</returns>
        public float[] FeedForward(float[] inputs)
        {
            _layers[0].FeedForward(inputs: inputs);
            for (var i = 1; i < _layers.Length; i++)
            {
                _layers[i].FeedForward(inputs: _layers[i - 1].Outputs);
            }

            return _layers[_layers.Length - 1].Outputs;
        }


        /// <summary>
        ///   Perform back-propagation and weight adjustment for the network.
        /// </summary>
        /// <param name="expected">The desired output.</param>
        public void BackPropagation(float[] expected)
        {
            for (var i = _layers.Length - 1; i >= 0; i--)
            {
                if (i == _layers.Length - 1)
                {
                    _layers[i].BackPropagationOutputLayer(expected: expected);
                }
                else
                {
                    _layers[i].BackPropagationHiddenLayer(nextLayer: _layers[i + 1]);
                }
            }

            foreach (var layer in _layers)
            {
                layer.UpdateWeights();
            }
        }
    }
}