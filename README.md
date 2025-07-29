# PgBuilder

## OpenSSL

Run

```bat
call "C:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Auxiliary\Build\vcvarsamd64_arm64.bat"
PATH %PATH%;C:\Strawberry\perl\bin
```

```bat
PgBuilder.exe ^
 openssl-arm64x-prepare ^
 V:\psqlodbc-for-win10-arm64\\openssl-arm64x-release-2 ^
 --cc="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjCL\bin\Debug\net8.0-windows\Arm64XDualObjCL.exe" ^
 --ld="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLINK\bin\Debug\net8.0-windows\Arm64XDualObjLINK.exe" ^
 --ar="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLIB\bin\Debug\net8.0-windows\Arm64XDualObjLIB.exe"
```

```bat
PgBuilder.exe ^
 openssl-arm64x-build ^
 V:\psqlodbc-for-win10-arm64\\openssl-arm64x-release-2 ^
 V:\psqlodbc-for-win10-arm64\\openssl-arm64x-release-2-inst ^
 --vc-cl="H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\cl.exe" ^
 --vc-link="H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\link.exe" ^
 --vc-lib="H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\lib.exe"
```
