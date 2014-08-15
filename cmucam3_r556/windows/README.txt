This directory contains all of the software you will need to compile
and download code to the CMUcam3.  The main source tree "cc3" should be 
located in the top most directory. Install the files in the following order:

1) cygwin-install
   Contains a self-contained Cygwin installer.  The README.txt file contains
   installation instructions

2) arm-2008q1-126-arm-none-eabi.exe
   This is the arm GCC compiler.  Install this after Cygwin is installed.

3) flash.isp.utility.lpc2000.zip
   This is the serial flash utility that allows you to download firmware to
   the CMUcam3.

4) dotnetfx35setup.exe
   This is the .NET runtime environment from Microsoft.  This is required to
   run the CMUcam3 Frame Grabber utility.

5) CMUcam3 Frame Grabber.exe
   This is a simple application that lets you grab frames and adjust various
   settings once the CMUcam2 emulation firmware is loaded on your CMUcam3.
