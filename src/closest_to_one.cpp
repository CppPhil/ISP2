#include "closest_to_one.hpp"

namespace isp2 {
int closestToOne(const std::array<double, decimalDigits>& ary)
{
    constexpr double one = 1.0; // Compile time constant for the value 1

    std::size_t index
        = 0; // The last index found for which the double was closest to 1.0
             // Starts out as 0 as we haven't looked at anything yet.

    // Iterate over the array.
    for (std::size_t i = 0; i < ary.size(); ++i) {
        // If the distance between the current double value to one
        // is closer than the one of the saved index
        // we update the saved index to be the current index
        // as the current one is apparently closer to one.
        if (one - ary[i] < one - ary[index]) { index = i; }
    }

    // Return the decimal digit for which the double value is closest to 1.0
    return static_cast<int>(index);
}
} // namespace isp2
