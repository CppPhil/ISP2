#include "idx_file.hpp"

namespace isp2 {
const char readBinaryFileOpenMode[] = "rb";

IdxFile::IdxFile(
    pl::string_view filePath,
    pl::byte*       buffer,
    std::size_t     bufferSize)
    : m_baseAddress(buffer)
    , m_bufferSize(bufferSize)
    , m_file(std::fopen(filePath.data(), readBinaryFileOpenMode))
{
    if (m_file == nullptr) {
        PL_THROW_WITH_SOURCE_INFO(
            FilesystemException,
            "Could not open \"" + filePath.to_string() + "\"");
    }

    // Yes, memory mapping the files into your virtual address space is
    // faster. Unfortunately Qt wouldn't let me do that because their
    // implementation is broken for larger files ...
    const std::size_t res
        = std::fread(m_baseAddress, sizeof(pl::byte), m_bufferSize, m_file);

    if (res != m_bufferSize) {
        std::fclose(m_file);

        PL_THROW_WITH_SOURCE_INFO(
            FilesystemException,
            "Could not read all of \"" + filePath.to_string() + "\"");
    }
}

IdxFile::~IdxFile() { std::fclose(m_file); }

pl::byte* IdxFile::offset(std::size_t baseAddressOffset)
{
    return m_baseAddress + baseAddressOffset;
}

const pl::byte* IdxFile::offset(std::size_t baseAddressOffset) const
{
    return const_cast<this_type*>(this)->offset(baseAddressOffset);
}

std::size_t IdxFile::size() const { return m_bufferSize; }
} // namespace isp2
