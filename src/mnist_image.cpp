#include "mnist_image.hpp"
#include <cstring> // std::memcpy
#include <new>     // operator new[]

namespace isp2 {
MnistImage::MnistImage() : m_baseAddress(nullptr) {}

MnistImage::MnistImage(pl::byte* imageDataBaseAddress)
    : m_baseAddress(imageDataBaseAddress)
{
}

std::array<double, imageDoubleCount> MnistImage::asDoubleArray() const
{
    std::array<double, imageDoubleCount> retVal;
    fillWithDoubles(m_baseAddress, retVal.data());
    return retVal;
}

std::vector<double> MnistImage::asDoubleVector() const
{
    std::vector<double> retVal(imageDoubleCount);
    fillWithDoubles(m_baseAddress, retVal.data());
    return retVal;
}

/*!
 * \brief Cleanup routine for the QImages to free the dynamic memory
 *        allocated for them.
 * \param info
 **/
static void mnistQImageCleanupHandler(void* info)
{
    // It's actually a byte pointer
    pl::byte* memory = static_cast<pl::byte*>(info);

    // Free the memory.
    delete[] memory;
}

QImage MnistImage::asQImage() const
{
    // Allocate dynamic memory for the greyscale image data.
    pl::byte* memory = new pl::byte[imageByteCount];

    // Copy the greyscale image data into the buffer just allocated.
    std::memcpy(memory, m_baseAddress, imageByteCount);

    // 'Fix up' the image.
    // We only do this for these pieces of dynamically allocated memory
    // rather than for the actual global buffers as to not modify them
    // so that the visualizations of the MNIST images show up the same
    // in the GUI all the time.
    // This is actually less efficient (of course) but it doesn't
    // noticably impact performance.
    fixupMnistImage(memory, imageByteCount);

    return QImage(
        /* data */ memory,
        /* width */ imageColumnCount,
        /* height */ imageRowCount,
        /* format */ QImage::Format_Grayscale8,
        /* cleanupFunction */ &mnistQImageCleanupHandler,
        /* cleanupInfo */ memory);
}

void MnistImage::fixupMnistImage(pl::byte* baseAddress, std::size_t byteCount)
{
    using namespace pl::literals::integer_literals;

    for (std::size_t i = 0; i < byteCount; ++i) {
        baseAddress[i] = 0xFF_byte - baseAddress[i];
    }
}

double MnistImage::mnistPixelToDouble(pl::byte mnistPixelValue)
{
    // Convert an MNIST pixel [0,255] to a double [0.0,1.0].
    // By dividing by 255.0 we scale the value down to the range of [0.0,1.0]
    // which the neural network expects.
    return mnistPixelValue / 255.0;
}

void MnistImage::fillWithDoubles(pl::byte* baseAddr, double* buffer)
{
    static_assert(
        imageByteCount == imageDoubleCount,
        "imageByteCount and imageDoubleCount must be equal!");

    for (std::size_t i = 0; i < imageByteCount; ++i) {
        buffer[i] = mnistPixelToDouble(baseAddr[i]);
    }
}
} // namespace isp2
