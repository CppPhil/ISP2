#include "mainwindow.hpp" // isp2::MainWindow
#include <QApplication>   // QApplication

int main(int argc, char* argv[])
{
    QApplication     a(argc, argv);
    isp2::MainWindow w;
    w.show();

    return a.exec();
}
