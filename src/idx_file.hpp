#pragma once
#include "filesystem_exception.hpp" // isp2::FilesystemException
#include <cstddef>                  // std::size_t
#include <cstdio>                   // std::FILE
#include <pl/byte.hpp>              // pl::byte
#include <pl/noncopyable.hpp>       // PL_NONCOPYABLE
#include <pl/string_view.hpp>       // pl::string_view

namespace isp2 {
/*!
 * \brief Type to represent an IDX file.
 **/
class IdxFile {
public:
    typedef IdxFile this_type;

    PL_NONCOPYABLE(IdxFile);

    /*!
     * \brief Creates an IdxFile object.
     * \param filePath The path to the file to read from.
     * \param buffer The buffer to use to fill with the data from the file.
     * \param bufferSize The size of the buffer in bytes.
     * \throws FilesystemException if an error occurred accessing the file
     *                             system.
     *
     * Fills the buffer with the data from the file.
     **/
    IdxFile(pl::string_view filePath, pl::byte* buffer, std::size_t bufferSize);

    /*!
     * \brief Closes the underlying operating system file stream.
     **/
    ~IdxFile();

    /*!
     * \brief Addresses into the buffer using an offset.
     * \param baseAddressOffset The offset to use relative to the base address.
     * \return The resulting pointer.
     * \warning baseAddressOffset must be less than .size()
     **/
    pl::byte* offset(std::size_t baseAddressOffset);

    /*!
     * \brief Addresses into the buffer using an offset.
     * \param baseAddressOffset The offset to use relative to the base address.
     * \return The resulting pointer.
     * \warning baseAddressOffset must be less than .size()
     **/
    const pl::byte* offset(std::size_t baseAddressOffset) const;

    /*!
     * \brief Queries the size of the IDX file.
     * \return The size of the IDX file in bytes.
     **/
    std::size_t size() const;

private:
    pl::byte* m_baseAddress;  /*!< Pointer to the start of the buffer to fill
                               *   with  IDX file data.
                               **/
    std::size_t m_bufferSize; /*!< The size of the IDX file in bytes */
    std::FILE*  m_file;       /*!< The underlying file stream */
};
} // namespace isp2
