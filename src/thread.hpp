#pragma once
#include "../deps/genann/genann.h" // genann_actfun
#include "idx_file.hpp"            // isp2::IdxFile
#include <QObject>                 // QObject
#include <QPixmap>                 // QPixmap
#include <atomic>                  // std::atomic_bool
#include <cstddef>                 // std::size_t
#include <functional>              // std::function
#include <memory>                  // std::unique_ptr
#include <pl/byte.hpp>             // pl::byte
#include <pl/noncopyable.hpp>      // PL_NONCOPYABLE
#include <vector>                  // std::vector

class QThread;

namespace isp2 {
/*!
 * \brief The Thread type used to actually run
 *        the neural network.
 **/
class Thread : public QObject {
    Q_OBJECT

public:
    typedef QObject base_type;

    PL_NONCOPYABLE(Thread);

    /*!
     * \brief Creates a Thread object.
     * \param trainingCycles The amount of training cycles to run.
     * \param testsToRun How many test image to run over.
     * \param hiddenLayerCount How many hidden layers to use.
     * \param hiddenLayerNeuronCount How many neurons per hidden layer.
     * \param activationFunction The activation function to use.
     * \param learningRate The learning rate to use.
     * \param trainingSetLabelFile The training set label file.
     * \param trainingSetImageFile The training set image file.
     * \param testSetLabelFile The test set label file.
     * \param testSetImageFile The test set image file.
     * \param parent The QObject parent.
     * \warning The thread assumes exclusive access to the IDX
     *          files! Don't touch them while the thread still exists!
     **/
    Thread(
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
        base_type*    parent = nullptr);

    /*!
     * \brief Shuts down the thread.
     **/
    virtual ~Thread() override;

    /*!
     * \brief Starts the thread.
     **/
    void start();

signals:
    /*!
     * \brief Signal that is emitted when progress is made.
     * \param currentImage The current image count.
     * \param totalImages The count of total images.
     * \warning Must be connected using a QueuedConnection!
     **/
    void newProgress(int currentImage, int totalImages);

    /*!
     * \brief Signal that is emitted when a new test image
     *        has been processed.
     * \param bitmap The QPixmap containing the MNIST greyscale
     *               image, ready to be rendered in the GUI.
     * \param expectedLabel The digit in the image according to the label file.
     * \param zero How much the neural network thinks it's a 0.
     * \param one How much the neural network thinks it's a 1.
     * \param two How much the neural network thinks it's a 2.
     * \param three How much the neural network thinks it's a 3.
     * \param four How much the neural network thinks it's a 4.
     * \param five How much the neural network thinks it's a 5.
     * \param six How much the neural network thinks it's a 6.
     * \param seven How much the neural network thinks it's a 7.
     * \param eight How much the neural network thinks it's a 8.
     * \param nine How much the neural network thinks it's a 9.
     * \warning Must be connected using a QueuedConnection!
     **/
    void newTestImageProcessed(
        QPixmap  bitmap,
        pl::byte expectedLabel,
        double   zero,
        double   one,
        double   two,
        double   three,
        double   four,
        double   five,
        double   six,
        double   seven,
        double   eight,
        double   nine);

private:
    /*!
     * \brief The underlying thread function that is actually run.
     **/
    void threadFunction();

    /*!
     * \brief Routine to run the neural network example.
     **/
    void runExample();

    const std::size_t m_trainingCycles; /*!< The amount of training cycles */
    const std::size_t m_testsToRun; /*!< The amount of test images to process */
    const int         m_hiddenLayerCount; /*!< How many hidden layers */
    const int
        m_hiddenLayerNeuronCount; /*!< How many neurons per hidden layer */
    const genann_actfun
                 m_activationFunction; /*!< The activation function to use. */
    const double m_learningRate;       /*!< The learning rate */
    const std::unique_ptr<QThread>
        m_thread; /*!< The underyling QThread object */
    std::atomic_bool
        m_isRunning; /*!< State that saves whether the thread is running */
    IdxFile&
        m_trainingSetLabelFile; /*!< Reference to the training set label file */
    IdxFile&
             m_trainingSetImageFile; /*!< Reference to the training set image file */
    IdxFile& m_testSetLabelFile; /*!< Reference to the test set label file */
    IdxFile& m_testSetImageFile; /*!< Reference to the test set image file */
};
} // namespace isp2
