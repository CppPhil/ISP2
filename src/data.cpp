#include "data.hpp"
#include "constants.hpp" // isp2::trainingSetLabelFileSize, ...

namespace isp2 {
pl::byte* trainingSetLabelFileBuffer()
{
    static pl::byte buf[trainingSetLabelFileSize] = {0};
    return buf;
}

pl::byte* trainingSetImageFileBuffer()
{
    static pl::byte buf[trainingSetImageFileSize] = {0};
    return buf;
}

pl::byte* testSetLabelFileBuffer()
{
    static pl::byte buf[testSetLabelFileSize] = {0};
    return buf;
}

pl::byte* testSetImageFileBuffer()
{
    static pl::byte buf[testSetImageFileSize] = {0};
    return buf;
}
} // namespace isp2
