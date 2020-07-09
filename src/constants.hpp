/*!
 * \file constants.hpp
 * \brief Exports constants used throughout the application.
 **/
#pragma once
#include <cstddef> // std::size_t

namespace isp2 {
/*!
 * \brief The relative path from the directory containing the executable
 *        to the MNIST directory.
 **/
const char relativePathFromAppFilePathToMnistDirectory[]
    = "../../../../../data/MNIST/";

/*!
 * \brief File name of the training set label file.
 **/
const char trainingSetLabelFileString[] = "train-labels.idx1-ubyte";

/*!
 * \brief File name of the training set image file.
 **/
const char trainingSetImageFileString[] = "train-images.idx3-ubyte";

/*!
 * \brief File name of the test set label file.
 **/
const char testSetLabelFileString[] = "t10k-labels.idx1-ubyte";

/*!
 * \brief File name of the test set image file.
 **/
const char testSetImageFileString[] = "t10k-images.idx3-ubyte";

/*!
 * \brief The size of the training set label file in bytes.
 *
 * 4 bytes for the magic number
 * 4 bytes for the number of items (60000)
 * 60000 bytes of label data with each byte being a single label.
 **/
const std::size_t trainingSetLabelFileSize = 60008;

/*!
 * \brief The size of the training set image file in bytes.
 *
 * 4 bytes for the magic number
 * 4 bytes for the number of images (60000)
 * 4 bytes for the number of rows (28)
 * 4 bytes for the number of columns (28)
 * 47040000 bytes of image data, with each image occupying 28 * 28 bytes (784
 * bytes per image), making for a total of 47040000 bytes occupied by image data
 **/
const std::size_t trainingSetImageFileSize = 47040016;

/*!
 * \brief The size of the test set label file in bytes.
 *
 * 4 bytes for the magic number
 * 4 bytes for the number of items (10000)
 * 10000 byes of label data with each byte being a single label.
 **/
const std::size_t testSetLabelFileSize = 10008;

/*!
 * \brief The size of the test set image file in bytes.
 *
 * 4 bytes for the magic number
 * 4 bytes for the number of images (10000)
 * 4 bytes for the number of rows (28)
 * 4 bytes for the number of columns (28)
 * 7840000 bytes of image data, with each image occupying 28 * 28 bytes (784
 * bytes per image), making for a total of 7840000 bytes occupied by image data
 **/
const std::size_t testSetImageFileSize = 7840016;

/*!
 * \brief The start offset for label files.
 *
 * The first 8 bytes of a label file don't contain
 * interesting data so we skip over them.
 **/
const std::size_t labelStartOffset = 8;

/*!
 * \brief The start offset for image files.
 *
 * The first 16 bytes of an image file don't
 * contain interesting data so we skip over them.
 **/
const std::size_t imageStartOffset = 16;

/*!
 * \brief The amount of items (images / labels) of the training set.
 **/
const std::size_t trainingItemCount = 60000;

/*!
 * \brief The amount of items (images / labels) of the test set.
 **/
const std::size_t testItemCount = 10000;

/*!
 * \brief The amount of rows that an image has.
 **/
const std::size_t imageRowCount = 28;

/*!
 * \brief The amount of columns that an image has.
 **/
const std::size_t imageColumnCount = 28;

/*!
 * \brief The amount of bytes occupied by a single MNIST greyscale image.
 **/
const std::size_t imageByteCount = imageRowCount * imageColumnCount;

/*!
 * \brief How many double we want to / have to use to represent all
 *        the bytes of an MNIST greyscale image.
 **/
const std::size_t imageDoubleCount = imageByteCount;

/*!
 * \brief The amount of different digits in the decimal (base 10)
 *        number system.
 **/
const std::size_t decimalDigits = 10;
} // namespace isp2
