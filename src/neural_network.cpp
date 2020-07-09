#include "neural_network.hpp"
#include <cstddef> // std::size_t
#include <new>     // std::bad_alloc

namespace isp2 {
NeuralNetwork::NeuralNetwork(
    int           inputs,
    int           hiddenLayers,
    int           hidden,
    int           outputs,
    double        learningRate,
    genann_actfun activationFunction)
    : m_inputs(inputs)
    , m_hiddenLayers(hiddenLayers)
    , m_hidden(hidden)
    , m_outputs(outputs)
    , m_learningRate(learningRate)
    , m_genann(genann_init(m_inputs, m_hiddenLayers, m_hidden, m_outputs))
{
    if (m_genann == nullptr) { throw std::bad_alloc(); }

    m_genann->activation_hidden = activationFunction;

    // Always use the cached sigmoid activation function for the output layer.
    m_genann->activation_output = &genann_act_sigmoid_cached;
}

NeuralNetwork::~NeuralNetwork() { genann_free(m_genann); }

NeuralNetwork::Error NeuralNetwork::train(
    const std::vector<double>& inputs,
    const std::vector<double>& desiredOutputs)
{
    if (inputs.size() != static_cast<std::size_t>(m_inputs)) {
        return Error::InvalidInputVectorSize;
    }

    if (desiredOutputs.size() != static_cast<std::size_t>(m_outputs)) {
        return Error::InvalidOutputVectorSize;
    }

    genann_train(
        m_genann, inputs.data(), desiredOutputs.data(), m_learningRate);
    return Error::NoError;
}

[[nodiscard]] tl::expected<std::vector<double>, NeuralNetwork::Error>
NeuralNetwork::run(const std::vector<double>& inputs)
{
    if (inputs.size() != static_cast<std::size_t>(m_inputs)) {
        return tl::make_unexpected(Error::InvalidInputVectorSize);
    }

    // genann_run returns a pointer into the innards of the genann object
    // which refers to a memory region that is occupied by the resulting
    // output vector of the size of the amount of neurons in the output
    // layer of the neural network.
    const double* ret = genann_run(m_genann, inputs.data());

    // Copy the result on out.
    return std::vector<double>(ret, ret + m_outputs);
}
} // namespace isp2
