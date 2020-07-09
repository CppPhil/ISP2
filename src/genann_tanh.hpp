#pragma once
#include "../deps/genann/genann.h" // genann

namespace isp2 {
/*!
 * \brief A genann compatible tanh function.
 * \param ann A pointer to a genann object
 * \param a The double value to use.
 * \return The result
 * \note Results in terrible classification results for some odd reason
 *       sigmoid is to be preferred.
 **/
double genannTanh(const genann* ann, double a);
} // namespace isp2
