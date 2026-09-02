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
 --alt-cc="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjCL\bin\Debug\net8.0-windows\Arm64XDualObjCL.exe" ^
 --alt-ld="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLINK\bin\Debug\net8.0-windows\Arm64XDualObjLINK.exe" ^
 --alt-ar="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLIB\bin\Debug\net8.0-windows\Arm64XDualObjLIB.exe"
```

```bat
PgBuilder.exe ^
 openssl-arm64x-build ^
 V:\psqlodbc-for-win10-arm64\\openssl-arm64x-release-2 ^
 V:\psqlodbc-for-win10-arm64\\openssl-arm64x-release-2-inst-3-6-4 ^
 --vc-cl="H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\cl.exe" ^
 --vc-link="H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\link.exe" ^
 --vc-lib="H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\lib.exe"
```

```bat
PgBuilder.exe ^
 openssl-arm64x-pack ^
 V:\psqlodbc-for-win10-arm64\\openssl-arm64x-release-2-inst-3-6-4 ^
 --version 3.6.4
```

## PostgreSQL

```bat
SET CL_EXE=H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\cl.exe
SET LINK_EXE=H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\link.exe
SET LIB_EXE=H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC\14.44.35207\bin\Hostx64\arm64\lib.exe

PATH %PATH%;H:\DL\win_flex_bison-2.5.25

V:\psqlodbc-for-win10-arm64\PgBuilder\PgBuilder\bin\Debug\net8.0\PgBuilder.exe ^
 postgresql-arm64x-prepare ^
 V:\psqlodbc-for-win10-arm64\postgres ^
 --meson meson.exe ^
 --prefix     V:\psqlodbc-for-win10-arm64\postgres-17-11-arm64x-release-install ^
 --build-dir  V:\psqlodbc-for-win10-arm64\postgres-17-11-arm64x-release-build ^
 --pkg-config     V:\psqlodbc-for-win10-arm64\Toolings\PkgConfigAlternative\bin\Debug\net8.0\pkg-config.exe ^
 --pkg-config-dir V:\psqlodbc-for-win10-arm64\postgres-arm64x-pkg-config ^
 --alt-cc="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjCL\bin\Debug\net8.0-windows\Arm64XDualObjCL.exe" ^
 --alt-ld="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLINK\bin\Debug\net8.0-windows\Arm64XDualObjLINK.exe" ^
 --alt-ar="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLIB\bin\Debug\net8.0-windows\Arm64XDualObjLIB.exe"
```
