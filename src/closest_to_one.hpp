#pragma once
#include "constants.hpp" // isp2::decimalDigits
#include <array>         // std::array

namespace isp2 {
/*!
 * \brief Determines the decimal number for which the double value is closest to
 *        one.
 * \param ary An array of 10 double values.
 *            ary[0] contains the double value for the
 *            decimal digit 0,
 *            ary[1] contains the double value for the
 *            decimal digit 1,
 *            ary[2] contains the double value for the
 *            decimal digit 2,
 *            ary[3] contains the double value for the
 *            decimal digit 3,
 *            ary[4] contains the double value for the
 *            decimal digit 4,
 *            ary[5] contains the double value for the
 *            decimal digit 5,
 *            ary[6] contains the double value for the
 *            decimal digit 6,
 *            ary[7] contains the double value for the
 *            decimal digit 7,
 *            ary[8] contains the double value for the
 *            decimal digit 8,
 *            ary[9] contains the double value for the
 *            decimal digit 9.
 * \return The decimal digit for which the associated double value was closest
 *         to 1.0 Will always be within the closed range of [0,9]
 **/
int closestToOne(const std::array<double, decimalDigits>& ary);
} // namespace isp2
