#include "thread.hpp"
#include "constants.hpp"      // isp2::trainingItemCount, isp2::testItemCount
#include "mnist_image.hpp"    // isp2::MnistImage
#include "neural_network.hpp" // isp2::NeuralNetwork
#include <QCoreApplication>   // QCoreApplication::processEvents
#include <QThread>            // QThread
#include <algorithm>          // std::min
#include <array>              // std::array
#include <pl/compiler.hpp>    // PL_COMPILER, PL_COMPILER_MSVC
#include <pl/os.hpp>          // PL_OS, PL_OS_LINUX
#if PL_OS == PL_OS_LINUX
#include <signal.h> // raise, SIGTRAP
#endif              // PL_OS == PL_OS_LINUX
#include <ciso646>  // not

namespace isp2 {
/*!
 * \brief Turns a label into a vector of doubles.
 * \param label The label [0,9].
 * \return The resulting vector.
 **/
static std::vector<double> labelToDoubleVector(pl::byte label)
{
    std::vector<double> retVal
        //   0,   1,   2,   3,   4,   5,   6,   7,   8,   9
        = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};

    // Set the double at the index that is associated with the label
    // to be 1.0
    retVal[label] = 1.0;

    // return the vector.
    return retVal;
}

Thread::Thread(
    std::size_t   trainingCycles,
    std::size_t   testsToRun,
    int           hiddenLayerCount,
    int           hiddenLayerNeuronCount,
    genann_actfun activationFunction,
    double        learningRate,
    IdxFile&      trainingSetLabelFile,
    IdxFile&      trainingSetImageFile,
    IdxFile&      testSetLabelFile,
    IdxFile&      testSetImageFile,
    base_type*    parent)
    : base_type(parent)
    , m_trainingCycles(trainingCycles)
    , m_testsToRun(testsToRun)
    , m_hiddenLayerCount(hiddenLayerCount)
    , m_hiddenLayerNeuronCount(hiddenLayerNeuronCount)
    , m_activationFunction(activationFunction)
    , m_learningRate(learningRate)
    , m_thread(QThread::create(&Thread::threadFunction, this))
    , m_isRunning(false) /* by default we're not running */
    , m_trainingSetLabelFile(trainingSetLabelFile)
    , m_trainingSetImageFile(trainingSetImageFile)
    , m_testSetLabelFile(testSetLabelFile)
    , m_testSetImageFile(testSetImageFile)
{
}

Thread::~Thread()
{
    /* If we haven't ever been run -> do nothing */
    if (not m_isRunning) { return; }

    // Tell the QThread to stop as soon as it can.
    m_thread->quit();

    const unsigned long timeoutMs = 150UL;

    // Spin with a timeout, giving the GUI some processor time.
    // This function (destructor of Thread) is only really ever run on
    // the main thread, or at least that's how it should be.
    while (not m_thread->wait(timeoutMs)) { QCoreApplication::processEvents(); }

    // We're done here -> so we're not running anymore.
    m_isRunning = false;
}

void Thread::start()
{
    moveToThread(m_thread.get());
    m_isRunning = true;
    m_thread->start();
}

void Thread::threadFunction()
{
    runExample();

    QEventLoop eventLoop;
    eventLoop.exec();
}

void Thread::runExample()
{
    // Fetch 'em pointers using their respective offsets (skipping over the
    // worthless bytes in the beginning).
    pl::byte* const trainingSetLabelsStartAddress
        = m_trainingSetLabelFile.offset(labelStartOffset);
    pl::byte* const trainingSetImagesStartAddress
        = m_trainingSetImageFile.offset(imageStartOffset);
    pl::byte* const testSetLabelsStartAddress
        = m_testSetLabelFile.offset(labelStartOffset);
    pl::byte* const testSetImagesStartAddress
        = m_testSetImageFile.offset(imageStartOffset);

    // Use std::min for sanity: We can't process more test images than there
    // actually are.
    const std::size_t testsToRun = std::min(testItemCount, m_testsToRun);

    // How many images we'll be processing in some manner.
    const std::size_t totalImageCount
        = trainingItemCount * m_trainingCycles + testsToRun;

    // Current image counter.
    std::size_t curImage = 0;

    // Create the training images.
    std::array<MnistImage, trainingItemCount> trainingImages;
    for (std::size_t i = 0; i < trainingItemCount; ++i) {
        // The offset we use is i (current image count)
        // times the size of an MNIST image.
        trainingImages[i]
            = MnistImage(trainingSetImagesStartAddress + (i * imageByteCount));
    }

    // Fetch the training labels.
    std::array<pl::byte, trainingItemCount> trainingLabels;
    for (std::size_t i = 0; i < trainingItemCount; ++i) {
        trainingLabels[i] = trainingSetLabelsStartAddress[i];
    }

    // Create the test images in the same way.
    std::array<MnistImage, testItemCount> testImages;
    for (std::size_t i = 0; i < testItemCount; ++i) {
        testImages[i]
            = MnistImage(testSetImagesStartAddress + (i * imageByteCount));
    }

    // Grab 'em test labels.
    std::array<pl::byte, testItemCount> testLabels;
    for (std::size_t i = 0; i < testItemCount; ++i) {
        testLabels[i] = testSetLabelsStartAddress[i];
    }

    // Create the actual NeuralNetwork object.
    NeuralNetwork network(
        imageDoubleCount,
        m_hiddenLayerCount,
        m_hiddenLayerNeuronCount,
        decimalDigits,
        m_learningRate,
        m_activationFunction);

    // trainingCycles times is how often we want to run over
    // all of the training images.
    for (std::size_t i = 0; i < m_trainingCycles; ++i) {
        // Run over the training images.
        for (std::size_t j = 0; j < trainingItemCount; ++j) {
            // Do a backpropagation update.
            if (network.train(
                    trainingImages[j].asDoubleVector(), // Put in the image as a
                                                        // double vector
                    labelToDoubleVector(
                        trainingLabels[j])) // The expected results.
                != NeuralNetwork::Error::NoError) {
                // If an error occurred (cause you put in incorrectly sized
                // vectors) Try to break into the debugger (if there's one
                // attached) This shouldn't ever happen unless you've seriously
                // screwed up.
#if PL_OS == PL_OS_LINUX
                raise(SIGTRAP);
#elif PL_COMPILER == PL_COMPILER_MSVC
                __debugbreak();
#endif
            }

            // Inform the main thread about the new progress.
            emit newProgress(
                static_cast<int>(curImage), static_cast<int>(totalImageCount));
            ++curImage;
        }
    }

    // Test phase
    for (std::size_t i = 0; i < testsToRun; ++i) {
        const std::vector<double> currentTestImageDoubleVector
            = testImages[i].asDoubleVector();

        // Run the feed forward algorithm.
        const tl::expected<std::vector<double>, NeuralNetwork::Error>
            dataExpected = network.run(currentTestImageDoubleVector);

        // If no value was returned then you put in an incorrectly
        // sized vector. Why would you do something like that?
        if (not dataExpected.has_value()) {
            // Try to break into the debugger (if there's one attached)
            // This shouldn't ever happen unless you've seriously screwed up.
#if PL_OS == PL_OS_LINUX
            raise(SIGTRAP);
#elif PL_COMPILER == PL_COMPILER_MSVC
            __debugbreak();
#endif
            continue; // skip over this test image -> there's nothing we can do
                      // to salvage the situation.
        }

        // Grab the actual result vector.
        const std::vector<double>& data = *dataExpected;

        // Grab the current label.
        const pl::byte label = testLabels[i];

        /* Let the main thread know about the test image we've just processed */
        emit newTestImageProcessed(
            QPixmap::fromImage(testImages[i].asQImage()),
            label,
            data[0],
            data[1],
            data[2],
            data[3],
            data[4],
            data[5],
            data[6],
            data[7],
            data[8],
            data[9]);
        emit newProgress(
            static_cast<int>(curImage), static_cast<int>(totalImageCount));
        ++curImage;
    }

    // We're done here now.
    emit newProgress(
        static_cast<int>(totalImageCount), static_cast<int>(totalImageCount));
}
} // namespace isp2
