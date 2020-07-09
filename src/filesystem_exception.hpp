#pragma once
#include <pl/except.hpp> // PL_DEFINE_EXCEPTION_TYPE

namespace isp2 {
PL_DEFINE_EXCEPTION_TYPE(FilesystemException, std::runtime_error);
} // namespace isp2
