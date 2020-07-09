# ISP2

## Date
Created in May of 2019

## Neural networks
This project implements a neural network example application in C++ using the Qt Framework.  

### Windows installation guide
For installation instructions for use with Microsoft Windows see the `data/windows_installation/Windows-Anleitung.pdf` file.  

### Building
On GNU/Linux the project can be built using QMake by invoking
`
qmake -makefile && make
`

### Neural network resources
#### Books
Michael Nielsen book:  
	<http://neuralnetworksanddeeplearning.com/index.html>  
#### Code
Michael Nielsen code:  
    <https://github.com/mnielsen/neural-networks-and-deep-learning>  
#### Videos
Dalski video:  
    <https://www.youtube.com/watch?v=L_PByyJ9g-I>  
3Blue1Brown:  
	<https://www.youtube.com/watch?v=aircAruvnKk>  
	<https://www.youtube.com/watch?v=IHZwWFHWa-w>  
	<https://www.youtube.com/watch?v=Ilg3gGewQ5U>  
	<https://www.youtube.com/watch?v=tIeHLnjs5U8>  

### MNIST data set
This project uses the MNIST data set to classify its hand-written digits.  
See <http://yann.lecun.com/exdb/mnist/> for information on the MNIST data set.  

### Git submodules
#### clang-format
URL: <https://github.com/CppPhil/clang-format/>  
Contains clang-format scripts for source code formatting on GNU/Linux.  

#### expected
URL: <https://github.com/TartanLlama/expected/>  
Header only library exporting the expected type, which contains either a result object or an 'error' object instead.  

#### genann
URL: <https://github.com/codeplea/genann/>  
C library implementing the underlying neural network.  

#### optional
URL: <https://github.com/TartanLlama/optional/>  
Header only library exporting the optional type, which may or may not contain an object of a given type.  

#### philslib
URL: <https://github.com/CppPhil/philslib/>  
Header only library exporting various utilities to make working with C++ more bearable.  

### Generating the documentation
The documentation can be generated using `Doxygen`.  
To generate the documentation use:  
`doxygen ./Doxyfile`  
The generated documentation will be placed in the `doc` subdirectory.  
The documentation can be viewed by opening the generated `index.html` file using a web browser.  

### Directory contents
| name                       | file / directory | description                                                                  |
|----------------------------|------------------|------------------------------------------------------------------------------|
| clean.sh                   | file             | GNU/Linux bash script that deletes QMake generated files.                    |
| data                       | directory        | Contains data for the application, e.g. the MNIST data set.                  |
| deps                       | directory        | Contains the git submodules                                                  |
| Doxyfile                   | file             | Doxyfile used by Doxygen to generate the documentation.                      |
| format.sh                  | file             | GNU/Linux bash script that formats the C++ source code.                     |
| .gitignore                 | file             | The project's .gitignore file, describing what files shall be ignored by Git |
| .gitmodules                | file             | Describes the git submodules used.                                           |
| ISP2.pro                   | file             | The QMake build script used to build the project.                            |
| qmake-destination-path.pri | file             | QMake build script include file to generated destination paths.              |
| qmake-target-platform.pri  | file             | QMake build script include file to detect the platform used.                 |
| README.md                  | file             | This README file                                                             |
| src                        | directory        | The directory containing the C++ source code.                                |
| TODO_LIST.txt              | file             | Text file used to keep track of things to do.                                |




