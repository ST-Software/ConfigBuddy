IF NOT EXIST C:\WIN\NUL GOTO 64bit
call "C:\Program Files\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86
goto :build
:64bit
  call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86

:build
msbuild default.targets /t:ReleaseBuild