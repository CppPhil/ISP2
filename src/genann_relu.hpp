#pragma once
#include "../deps/genann/genann.h" // genann

namespace isp2 {
/*!
 * \brief A genann compatible implementation of the ReLu activation function.
 * \param ann The artificial neural network, unused.
 * \param a The double input value.
 * \return The resulting value.
 **/
double genannReLu(const genann* ann, double a);
} // namespace isp2
