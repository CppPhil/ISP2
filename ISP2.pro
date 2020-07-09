include(qmake-target-platform.pri)
include(qmake-destination-path.pri)

QT          += core gui widgets

TARGET       = ISP2
TEMPLATE     = app

CONFIG      += c++17

DESTDIR      = $$PWD/binaries/$$DESTINATION_PATH
OBJECTS_DIR  = $$PWD/build/$$DESTINATION_PATH/.obj
MOC_DIR      = $$PWD/build/$$DESTINATION_PATH/.moc
RCC_DIR      = $$PWD/build/$$DESTINATION_PATH/.qrc
UI_DIR       = $$PWD/build/$$DESTINATION_PATH/.ui

DEFINES     += \
    NOMINMAX \
    _CRT_SECURE_NO_WARNINGS \
    _SCR_SECURE_NO_WARNINGS

DEFINES     *= QT_USE_QSTRINGBUILDER
DEFINES     *= NDEBUG # Disable assertions in genann

CONFIG(release, debug|release) {
    CONFIG  += optimize_full
}

SOURCES     += \
    src/closest_to_one.cpp \
    src/data.cpp \
    src/genann_relu.cpp \
    src/genann_tanh.cpp \
    src/idx_file.cpp \
    src/main.cpp \
    src/mainwindow.cpp \
    src/mnist_image.cpp \
    src/neural_network.cpp \
    src/thread.cpp \
    $$PWD/deps/genann/genann.c

HEADERS     += \
    src/closest_to_one.hpp \
    src/constants.hpp \
    src/data.hpp \
    src/filesystem_exception.hpp \
    src/genann_relu.hpp \
    src/genann_tanh.hpp \
    src/idx_file.hpp \
    src/mainwindow.hpp \
    src/mnist_image.hpp \
    src/neural_network.hpp \
    src/thread.hpp \
    $$PWD/deps/genann/genann.h

FORMS       += \
    src/mainwindow.ui

INCLUDEPATH += \
    deps/philslib/include \
    deps/optional/include \
    deps/expected/include
