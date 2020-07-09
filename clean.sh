#!/bin/bash

# Directory containing this bash script.
readonly DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

# Directories
readonly BIN_DIR=$DIR/binaries
readonly BUILD_DIR=$DIR/build

rm -rf $BIN_DIR
rm -rf $BUILD_DIR

rm -f $DIR/Makefile
rm -f $DIR/.qmake.stash 

exit 0
