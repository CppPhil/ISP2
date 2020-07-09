#include "genann_tanh.hpp"
#include <QtGlobal> // Q_UNUSED
#include <cmath>    // std::tanh

namespace isp2 {
double genannTanh(const genann* ann, double a)
{
    Q_UNUSED(ann);
    return std::tanh(a);
}
} // namespace isp2
