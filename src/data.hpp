/*!
 * \file data.hpp
 * \brief Exports functions that give access to global buffers
 *        that shall be used to hold the MNIST files.
 **/
#pragma once
#include <pl/byte.hpp> // pl::byte

namespace isp2 {
/*!
 * \brief Returns a pointer to the start of the training set label file buffer.
 * \return The requested pointer.
 * \warning The memory region addressable from the pointer returned is within
 *          the half open range of [p, p + trainingSetLabelFileSize)
 *          accessing beyond the bounds specified invokes undefined behavior!
 * \warning The buffer returned may not be accessed concurrently!
 **/
pl::byte* trainingSetLabelFileBuffer();

/*!
 * \brief Returns a pointer to the start of the training set image file buffer.
 * \return The requested pointer.
 * \warning The memory region addressable from the pointer returned is within
 *          the half open range of [p, p + trainingSetImageFileSize)
 *          accessing beyond the bounds specified invokes undefined behavior!
 * \warning The buffer returned may not be accessed concurrently!
 **/
pl::byte* trainingSetImageFileBuffer();

/*!
 * \brief Returns a pointer to the start of the test set label file buffer.
 * \return The requested pointer.
 * \warning The memory region addressable from the pointer returned is within
 *          the half open range of [p, p + testSetLabelFileSize)
 *          accessing beyond the bounds specified invokes undefined behavior!
 * \warning The buffer returned may not be accessed concurrently!
 **/
pl::byte* testSetLabelFileBuffer();

/*!
 * \brief Returns a pointer to the start of the test set image file buffer.
 * \return The requested pointer.
 * \warning The memory region addressable from the pointer returned is within
 *          the half open range of [p, p + testSetImageFileSize)
 *          accessing beyond the bounds specified invokes undefined behavior!
 * \warning The buffer returned may not be accessed concurrently!
 **/
pl::byte* testSetImageFileBuffer();
} // namespace isp2
