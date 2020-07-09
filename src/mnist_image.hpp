#pragma once
#include "constants.hpp" // isp2::imageDoubleCount
#include <QImage>        // QImage
#include <array>         // std::array
#include <cstddef>       // std::size_t
#include <pl/byte.hpp>   // pl::byte
#include <vector>        // std::vector

namespace isp2 {
/*!
 * \brief Type used to represent a single MNIST image in an IDX file.
 **/
class MnistImage {
public:
    /*!
     * \brief Creates an "empty" MnistImage object.
     * \warning The object created can't be used for anything.
     *          The only sane thing to do is replace it using
     *          one of the compiler generated assignment
     *          operators (operator=).
     **/
    MnistImage();

    /*!
     * \brief Creates a valid MnistImage object.
     * \param imageDataBaseAddress The address at which the data for the MNIST
     *                             image in an IDX file starts.
     * \warning The pointer given must be completely valid and the memory region
     *          of the closed range of [imageDataBaseAddress,
     *          imageDataBaseAddress + imageByteCount) must be legally
     *          adressable and must actually contain the actual image
     *          data to use!
     **/
    explicit MnistImage(pl::byte* imageDataBaseAddress);

    /*!
     * \brief Creates an array of doubles from this MNIST image
     *        to be used in the neural network.
     * \return The resulting array.
     **/
    std::array<double, imageDoubleCount> asDoubleArray() const;

    /*!
     * \brief Creates a vector of doubles from this MNIST image
     *        to be used in the neural network.
     * \return The resulting vector.
     * \note The vector returning will be of size imageDoubleCount
     **/
    std::vector<double> asDoubleVector() const;

    /*!
     * \brief Creates a QImage for this MNIST image.
     * \return The resulting QImage.
     * \note The resulting QImage can be put into a QPixmap
     *       which can then be put into a QLabel so that the
     *       greyscale image is visualized.
     **/
    QImage asQImage() const;

private:
    /*!
     * \brief 'Fixes up' an MNIST image.
     * \param baseAddress The address at which the bytes of an MNIST greyscale
     *                    image begin.
     * \param byteCount The size of the MNIST greyscale image in bytes.
     **/
    static void fixupMnistImage(pl::byte* baseAddress, std::size_t byteCount);

    /*!
     * \brief Converts a single pixel to a double.
     * \param mnistPixelValue The pixel.
     * \return The resulting double.
     **/
    static double mnistPixelToDouble(pl::byte mnistPixelValue);

    /*!
     * \brief Fills a buffer with the proper doubles.
     * \param baseAddr The address at which the MNIST greyscale image
     *                 data begins. The size of this buffer must be
     *                 imageByteCount.
     * \param buffer The address at which to put the doubles.
     *               This buffer must be a buffer of doubles which
     *               is imageDoubleCount objects (doubles) large.
     **/
    static void fillWithDoubles(pl::byte* baseAddr, double* buffer);

    pl::byte* m_baseAddress; /*!< The address at which the MNIST greyscale image
                              *   begins
                              **/
};
} // namespace isp2
