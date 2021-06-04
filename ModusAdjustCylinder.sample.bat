@ECHO OFF
set VS_DIR=c:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE

rem Include Folders
set "EIGEN_INCLUDE=C:\Users\bcapuana\source\repos\eigen-3.3.9\Eigen"

rem open solution in visual studio
set SLN_FILE=ModusAdjustCylinder.sln

@echo on
SET VS_DIR
SET SLN_FILE
SET VS_EXE=%VS_DIR%\DEVENV.EXE
SET VS_EXE
START "" "%VS_EXE%" "%SLN_FILE%"