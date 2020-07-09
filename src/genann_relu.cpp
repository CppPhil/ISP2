#include "genann_relu.hpp"
#include <QtGlobal>  // Q_UNUSED
#include <algorithm> // std::max

namespace isp2 {
double genannReLu(const genann* ann, double a)
{
    Q_UNUSED(ann);
    constexpr double zero = 0.0;
    return std::max(zero, a);
}
} // namespace isp2
