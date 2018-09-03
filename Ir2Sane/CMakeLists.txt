cmake_minimum_required(VERSION 3.12)
project(Ir2Sane)

set(CMAKE_CXX_STANDARD 17)

find_package(LLVM REQUIRED CONFIG)

message(STATUS "Found LLVM ${LLVM_PACKAGE_VERSION}")
message(STATUS "Using LLVMConfig.cmake in: ${LLVM_DIR}")

include_directories(${LLVM_INCLUDE_DIRS})
include_directories(/usr/include/llvm-6.0)
include_directories(/usr/include/llvm-c-6.0)
add_definitions(${LLVM_DEFINITIONS})

add_executable(Ir2Sane main.cpp)

# Find the libraries that correspond to the LLVM components
# that we wish to use
llvm_map_components_to_libnames(llvm_libs support core irreader)

# Link against LLVM libraries
target_link_libraries(Ir2Sane ${llvm_libs})
