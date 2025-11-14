using CommandLine;
using NLog;
using System.Diagnostics;
using System.Text;

// "H:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Auxiliary\Build\vcvarsamd64_arm64.bat"

namespace PgBuilder
{
    internal class Program
    {
        [Verb("openssl-arm64x-prepare", HelpText = "Prepare OpenSSL for building.")]
        private class OpenSSLPrepareOpt
        {
            [Value(0, Required = true, HelpText = "Path to the OpenSSL source directory.")]
            public string SourceDir { get; set; } = "";

            [Option("alt-cc", Required = true, HelpText = "Path to the Arm64XDualObjCL executable.")]
            public string CC { get; set; } = "";

            [Option("alt-ld", Required = true, HelpText = "Path to the Arm64XDualObjLINK executable.")]
            public string LD { get; set; } = "";

            [Option("alt-ar", Required = true, HelpText = "Path to the Arm64XDualObjLIB executable.")]
            public string AR { get; set; } = "";
        }

        [Verb("openssl-arm64x-build", HelpText = "Build OpenSSL from source.")]
        private class OpenSSLBuildOpt
        {
            [Value(0, Required = true, HelpText = "Path to the OpenSSL source directory.")]
            public string SourceDir { get; set; } = "";

            [Value(1, Required = true, HelpText = "Path to the destination directory for the build output.")]
            public string DestDir { get; set; } = "";

            [Option("vc-cl", Required = true, HelpText = "Path to the VC cl.exe executable.")]
            public string VC_CL { get; set; } = "";

            [Option("vc-link", Required = true, HelpText = "Path to the VC link.exe executable.")]
            public string VC_LINK { get; set; } = "";

            [Option("vc-lib", Required = true, HelpText = "Path to the VC lib.exe executable.")]
            public string VC_LIB { get; set; } = "";

            [Option("silent", HelpText = "Suppress output messages during the build process.")]
            public bool Silent { get; set; }
        }

        [Verb("postgresql-arm64x-prepare", HelpText = "Prepare PostgreSQL for building.")]
        private class PostgreSQLPrepareOpt
        {
            [Value(0, Required = true, HelpText = "Path to the PostgreSQL source directory.")]
            public string SourceDir { get; set; } = "";

            [Option("meson", Required = true, HelpText = "Path to the meson.exe executable.")]
            public string Meson { get; set; } = "";

            [Option("prefix", Required = true, HelpText = "Path to the destination directory for the build output.")]
            public string Prefix { get; set; } = "";

            [Option("build-dir", Required = true, HelpText = "Path to the build directory where the build files will be generated.")]
            public string BuildDir { get; set; } = "";

            [Option("pkg-config", Required = true, HelpText = "Path to the pkg-config.exe executable.")]
            public string PkgConfig { get; set; } = "";

            [Option("pkg-config-dir", Required = true, HelpText = "Path to the directory where pkg-config files will be installed.")]
            public string PkgConfigDir { get; set; } = "";

            [Option("alt-cc", Required = true, HelpText = "Path to the Arm64XDualObjCL executable.")]
            public string CC { get; set; } = "";

            [Option("alt-ld", Required = true, HelpText = "Path to the Arm64XDualObjLINK executable.")]
            public string LD { get; set; } = "";

            [Option("alt-ar", Required = true, HelpText = "Path to the Arm64XDualObjLIB executable.")]
            public string AR { get; set; } = "";
        }

        [Verb("psqlodbc-arm64x-batch-build", HelpText = "Batch build PostgreSQL ODBC driver with OpenSSL and PostgreSQL.")]
        private class PsqlodbcBatchBuildOpt
        {
            [Value(0, Required = true, HelpText = "Path to the psqlodbc source directory.")]
            public string SourceDir { get; set; } = "";

            [Option("batch-build-root", Required = true, HelpText = "Path to the batch build root directory where the project directories will be generated.")]
            public string BatchBuildDir { get; set; } = "";

            [Option("cmake", Required = true, HelpText = "Path to the cmake.exe executable.")]
            public string CMake { get; set; } = "";

            [Option("msbuild", Required = true, HelpText = "Path to the msbuild.exe executable.")]
            public string MSBuild { get; set; } = "";

            [Option("wix", Required = true, HelpText = "Path to the wix.exe executable.")]
            public string Wix { get; set; } = "";

            [Option("pg-inc", Required = true, HelpText = "PG_INC")]
            public string PgInc { get; set; } = "";

            [Option("pg-lib", Required = true, HelpText = "PG_LIB")]
            public string PgLib { get; set; } = "";

            [Option("LIBPQBINDIR", Required = true, HelpText = "$(LIBPQBINDIR)\\libpq.dll")]
            public string LIBPQBINDIR { get; set; } = "";

            [Option("LIBPQMSVCDLL", Default = "", HelpText = "")]
            public string LIBPQMSVCDLL { get; set; } = "";

            [Option("LIBPQMSVCSYS", Default = "vcruntime140.dll", HelpText = "vcruntime140.dll")]
            public string LIBPQMSVCSYS { get; set; } = "";

            [Option("PODBCMSVCDLL", Default = "", HelpText = "")]
            public string PODBCMSVCDLL { get; set; } = "";

            [Option("PODBCMSVPDLL", Default = "", HelpText = "msvcp140.dll")]
            public string PODBCMSVPDLL { get; set; } = "";

            [Option("PODBCMSVCSYS", Default = "", HelpText = "")]
            public string PODBCMSVCSYS { get; set; } = "";

            [Option("PODBCMSVPSYS", Default = "", HelpText = "")]
            public string PODBCMSVPSYS { get; set; } = "";

            [Option("full-version", Default = "17.00.0006", HelpText = "Full version of psqlODBC setup (e.g., 17.00.0006).")]
            public string FullVersion { get; set; } = null!;

            [Option("subloc", Default = "1700", HelpText = "psqlODBC sublocation (e.g., 1700 for version 17.00).")]
            public string SubLoc { get; set; } = null!;

            [Option("skip-build", Default = false, HelpText = "Skip the build process.")]
            public bool SkipBuild { get; set; }

            [Option("skip-deploy", Default = false, HelpText = "Skip the deploy process.")]
            public bool SkipDeploy { get; set; }

            [Option("skip-wix", Default = false, HelpText = "Skip the WiX packaging process.")]
            public bool SkipWix { get; set; }

            [Option("LIBPQMEM", HelpText = "File names only, because they must be placed at this location: `$(LIBPQBINDIR)\\$(LIBPQMEM0)`. The typical required files are `libcrypto-3-arm64.dll` and `libssl-3-arm64.dll`", Min = 1, Max = 10)]
            public IEnumerable<string> LIBPQMEM { get; set; } = new string[0];
        }

        static int Main(string[] args)
        {
            try
            {
                return Parser.Default.ParseArguments<OpenSSLPrepareOpt, OpenSSLBuildOpt, PostgreSQLPrepareOpt, PsqlodbcBatchBuildOpt>(args)
                    .MapResult<OpenSSLPrepareOpt, OpenSSLBuildOpt, PostgreSQLPrepareOpt, PsqlodbcBatchBuildOpt, int>(
                        DoOpenSSLPrepare,
                        DoOpenSSLBuild,
                        DoPostgreSQLPrepare,
                        DoPsqlodbcBatchBuild,
                        ex => 1
                    );
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static int DoPsqlodbcBatchBuild(PsqlodbcBatchBuildOpt o)
        {
            var logger = LogManager.GetLogger("PsqlodbcBatchBuild");
            var stepLogger = LogManager.GetLogger("Step");
            var cmakeLogger = LogManager.GetLogger("CMake");
            var msbuildLogger = LogManager.GetLogger("MSBuild");
            var wixLogger = LogManager.GetLogger("wix");
            var deployLogger = LogManager.GetLogger("deploy");
            var cmakeArch = "ARM64";
            var configuration = "RelWithDebInfo";
            var pgBin = $"C:/Program Files/psqlODBC/{o.FullVersion}/bin";
            var libpqMemories = new List<string>(o.LIBPQMEM ?? Array.Empty<string>());

            Directory.CreateDirectory(o.BatchBuildDir);

            var instBase = Path.Combine(o.BatchBuildDir, "INSTBASE");
            Directory.CreateDirectory(instBase);

            var binBase = Path.Combine(o.BatchBuildDir, "BINBASE");
            Directory.CreateDirectory(binBase);

            var sourceDir = o.SourceDir;
            var batchBuildDir = o.BatchBuildDir;

            void CMake(params string[] args)
            {
                var psi = new ProcessStartInfo(
                    o.CMake,
                    string.Join(" ", args)
                )
                {
                    UseShellExecute = false,
                    WorkingDirectory = batchBuildDir,
                };
                cmakeLogger.Info($"\"{psi.FileName}\" {psi.Arguments}");
                var p = Process.Start(psi)!;
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    cmakeLogger.Error($"CMake failed by ExitCode {p.ExitCode}.");
                    throw new Exception($"CMake failed by ExitCode {p.ExitCode}.");
                }
            }

            void FixAndBuildSln(string slnPath)
            {
                {
                    logger.Debug($"Cloning ARM64 projects to ARM64EC in {slnPath}...");

                    new LibBuildHelper.SlnCloneCp(
                        new LibBuildHelper.CMakeVCXProjSetupArm64x()
                    )
                        .Run(
                            slnPath: slnPath,
                            projectNames: "*",
                            copyFrom: "ARM64",
                            copyTo: "ARM64EC",
                            updateProjectFile: true
                        );
                }

                {
                    var psi = new ProcessStartInfo(
                        o.MSBuild,
                        $"\"{slnPath}\" /p:Configuration=\"{configuration}\" /p:Platform=\"ARM64EC\""
                    )
                    {
                        UseShellExecute = false,
                        WorkingDirectory = batchBuildDir,
                    };
                    msbuildLogger.Info($"\"{psi.FileName}\" {psi.Arguments}");
                    var p = Process.Start(psi)!;
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                        msbuildLogger.Error($"MSBuild({slnPath}) failed by ExitCode {p.ExitCode}.");
                        throw new Exception($"MSBuild({slnPath}) failed by ExitCode {p.ExitCode}.");
                    }
                }
            }

            if (!o.SkipBuild)
            {
                #region Common Library Build
                {
                    var project = "pgxalib";
                    var buildAs = "pgxalib";

                    stepLogger.Info($"Building {buildAs}...");

                    CMake(
                        "-G \"Visual Studio 17 2022\"",
                        $"-A \"{cmakeArch}\"",
                        $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                        $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                    );
                    FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                }
                #endregion

                #region ANSI Build
                {
                    {
                        var project = "pgenlist_implib";
                        var buildAs = "pgenlista_implib";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=YES",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }

                    {
                        var project = "psqlodbc";
                        var buildAs = "psqlodbca";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=YES",
                            "-D MEMORY_DEBUG:BOOL=NO",
                            "-D MSDTC:BOOL=YES",
                            $"-D PG_INC:PATH=\"{InsertPath(o.PgInc)}\"",
                            $"-D PG_LIB:PATH=\"{InsertPath(o.PgLib)}\"",
                            $"-D PGENLIST_LIB:PATH=\"{Path.Combine(batchBuildDir, $"pgenlista_implib-{cmakeArch}", configuration)}\"",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }

                    {
                        var project = "pgenlist";
                        var buildAs = "pgenlista";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=YES",
                            $"-D PSQLODBC_LIB:PATH=\"{Path.Combine(batchBuildDir, $"psqlodbca-{cmakeArch}", configuration)}\"",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }

                    {
                        var project = "psqlsetup";
                        var buildAs = "psqlsetupa";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=YES",
                            $"-D PSQLODBC_LIB:PATH=\"{Path.Combine(batchBuildDir, $"psqlodbca-{cmakeArch}", configuration)}\"",
                            $"-D PGENLIST_LIB:PATH=\"{Path.Combine(batchBuildDir, $"pgenlista-{cmakeArch}", configuration)}\"",
                            $"-D PG_BIN:PATH=\"{pgBin}\"",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }
                }
                #endregion

                #region Unicode Build
                {
                    {
                        var project = "pgenlist_implib";
                        var buildAs = "pgenlistw_implib";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=NO",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }

                    {
                        var project = "psqlodbc";
                        var buildAs = "psqlodbcw";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=NO",
                            "-D MEMORY_DEBUG:BOOL=NO",
                            "-D MSDTC:BOOL=YES",
                            $"-D PG_INC:PATH=\"{InsertPath(o.PgInc)}\"",
                            $"-D PG_LIB:PATH=\"{InsertPath(o.PgLib)}\"",
                            $"-D PGENLIST_LIB:PATH=\"{Path.Combine(batchBuildDir, $"pgenlistw_implib-{cmakeArch}", configuration)}\"",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }

                    {
                        var project = "pgenlist";
                        var buildAs = "pgenlistw";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=NO",
                            $"-D PSQLODBC_LIB:PATH=\"{Path.Combine(batchBuildDir, $"psqlodbcw-{cmakeArch}", configuration)}\"",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }

                    {
                        var project = "psqlsetup";
                        var buildAs = "psqlsetupw";

                        stepLogger.Info($"Building {buildAs}...");

                        CMake(
                            "-D ANSI_VERSION:BOOL=NO",
                            $"-D PSQLODBC_LIB:PATH=\"{Path.Combine(batchBuildDir, $"psqlodbcw-{cmakeArch}", configuration)}\"",
                            $"-D PGENLIST_LIB:PATH=\"{Path.Combine(batchBuildDir, $"pgenlistw-{cmakeArch}", configuration)}\"",
                            $"-D PG_BIN:PATH=\"{pgBin}\"",
                            "-G \"Visual Studio 17 2022\"",
                            $"-A \"{cmakeArch}\"",
                            $"-S \"{Path.Combine(sourceDir, $"winbuild2/{project}")}\"",
                            $"-B \"{Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}")}\""
                        );
                        FixAndBuildSln(Path.Combine(batchBuildDir, $"{buildAs}-{cmakeArch}", $"{project}.sln"));
                    }
                }
                #endregion
            }

            #region Deploy BINBASE
            if (!o.SkipDeploy)
            {
                {
                    void DeployFile(string sourceFileName, string destFileName, bool overwrite)
                    {
                        deployLogger.Info($"Deploying `{sourceFileName}` to `{destFileName}` (overwrite={overwrite})...");
                        File.Copy(sourceFileName, destFileName, overwrite);
                    }

                    var ANSIFOLDER = Path.Combine(binBase, "x64_ANSI_Release");
                    Directory.CreateDirectory(ANSIFOLDER);

                    var UNICODEFOLDER = Path.Combine(binBase, "x64_Unicode_Release");
                    Directory.CreateDirectory(UNICODEFOLDER);

                    DeployFile(
                        sourceFileName: Path.Combine(batchBuildDir, $"psqlodbca-{cmakeArch}", configuration, "psqlodbc30a.dll"),
                        destFileName: Path.Combine(ANSIFOLDER, "psqlodbc30a.dll"),
                        overwrite: true
                    );

                    DeployFile(
                        sourceFileName: Path.Combine(batchBuildDir, $"pgenlista-{cmakeArch}", configuration, "pgenlista.dll"),
                        destFileName: Path.Combine(ANSIFOLDER, "pgenlista.dll"),
                        overwrite: true
                    );

                    DeployFile(
                        sourceFileName: Path.Combine(batchBuildDir, $"psqlodbcw-{cmakeArch}", configuration, "psqlodbc35w.dll"),
                        destFileName: Path.Combine(UNICODEFOLDER, "psqlodbc35w.dll"),
                        overwrite: true
                    );

                    DeployFile(
                        sourceFileName: Path.Combine(batchBuildDir, $"pgenlistw-{cmakeArch}", configuration, "pgenlist.dll"),
                        destFileName: Path.Combine(UNICODEFOLDER, "pgenlist.dll"),
                        overwrite: true
                    );

                    DeployFile(
                        sourceFileName: Path.Combine(batchBuildDir, $"pgxalib-{cmakeArch}", configuration, "pgxalib.dll"),
                        destFileName: Path.Combine(UNICODEFOLDER, "pgxalib.dll"),
                        overwrite: true
                    );
                }
            }
            #endregion

            #region WiX

            if (!o.SkipWix)
            {
                void WixBuild(params string[] args)
                {
                    var psi = new ProcessStartInfo(
                        o.Wix,
                        string.Join(
                            " ",
                            new string[] { "build", "--nologo", "-arch x64" }
                                .Concat(args)
                        )
                    )
                    {
                        UseShellExecute = false,
                        WorkingDirectory = Path.Combine(o.SourceDir, "installer"),
                    };
                    wixLogger.Info($"\"{psi.FileName}\" {psi.Arguments}");
                    var p = Process.Start(psi)!;
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                        wixLogger.Error($"wix failed by ExitCode {p.ExitCode}.");
                        throw new Exception($"wix failed by ExitCode {p.ExitCode}.");
                    }
                }

                // wix build --nologo -arch $CPUTYPE
                // $libpqRelArgs
                // -d "VERSION=$VERSION"
                // -d "SUBLOC=$SUBLOC"
                // -d "LIBPQBINDIR=$LIBPQBINDIR"
                // -d "LIBPQMSVCDLL=$LIBPQMSVCDLL"
                // -d "LIBPQMSVCSYS=$LIBPQMSVCSYS"
                // -d "PODBCMSVCDLL=$PODBCMSVCDLL"
                // -d "PODBCMSVPDLL=$PODBCMSVPDLL"
                // -d "PODBCMSVCSYS=$PODBCMSVCSYS"
                // -d "PODBCMSVPSYS=$PODBCMSVPSYS"
                // -d "NoPDB=$NoPDB"
                // -d "BINBASE=$BINBASE"
                // -o $INSTBASE\psqlodbc_$CPUTYPE.msm
                // psqlodbcm_cpu.wxs
                WixBuild(
                    $"-d \"LIBPQMEM0={libpqMemories.Skip(0).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM1={libpqMemories.Skip(1).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM2={libpqMemories.Skip(2).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM3={libpqMemories.Skip(3).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM4={libpqMemories.Skip(4).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM5={libpqMemories.Skip(5).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM6={libpqMemories.Skip(6).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM7={libpqMemories.Skip(7).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM8={libpqMemories.Skip(8).FirstOrDefault()}\"",
                    $"-d \"LIBPQMEM9={libpqMemories.Skip(9).FirstOrDefault()}\"",
                    $"-d \"VERSION={o.FullVersion}\"",
                    $"-d \"SUBLOC={o.SubLoc}\"",
                    $"-d \"LIBPQBINDIR={o.LIBPQBINDIR}\"",
                    $"-d \"LIBPQMSVCDLL={o.LIBPQMSVCDLL}\"",
                    $"-d \"LIBPQMSVCSYS={o.LIBPQMSVCSYS}\"",
                    $"-d \"PODBCMSVCDLL={o.PODBCMSVCDLL}\"",
                    $"-d \"PODBCMSVPDLL={o.PODBCMSVPDLL}\"",
                    $"-d \"PODBCMSVCSYS={o.PODBCMSVCSYS}\"",
                    $"-d \"PODBCMSVPSYS={o.PODBCMSVPSYS}\"",
                    $"-d \"NoPDB=True\"",
                    $"-d \"BINBASE={binBase}\"",
                    $"-o \"{Path.Combine(instBase, "psqlodbc_x64.msm")}\"",
                    $"\"{Path.Combine(o.SourceDir, "installer", "psqlodbcm_cpu.wxs")}\""
                );

                // wix build --nologo -arch $CPUTYPE
                // -ext WixToolset.UI.wixext
                // -d "VERSION=$VERSION"
                // -d "SUBLOC=$SUBLOC"
                // -d "INSTBASE=$INSTBASE"
                // -o $INSTBASE\psqlodbc_$CPUTYPE.msi
                // psqlodbc_cpu.wxs
                WixBuild(
                    $"-ext WixToolset.UI.wixext",
                    $"-d \"VERSION={o.FullVersion}\"",
                    $"-d \"SUBLOC={o.SubLoc}\"",
                    $"-d \"INSTBASE={instBase}\"",
                    $"-o \"{Path.Combine(instBase, "psqlodbc_x64.msi")}\"",
                    $"\"{Path.Combine(o.SourceDir, "installer", "psqlodbc_cpu.wxs")}\""
                );
            }

            #endregion

            return 0;
        }

        private static string InsertPath(string path)
        {
            return path.Replace("\\", "/");
        }

        private static int DoPostgreSQLPrepare(PostgreSQLPrepareOpt o)
        {
            Directory.CreateDirectory(o.BuildDir);

            {
                var writer = new StringWriter() { NewLine = "\n", };
                writer.WriteLine("[binaries]");
                writer.WriteLine("c = '{0}'", o.CC);
                writer.WriteLine("cpp = '{0}'", o.CC);
                writer.WriteLine("ar = '{0}'", o.AR);
                writer.WriteLine("c_ld = '{0}'", o.LD);
                writer.WriteLine("cpp_ld = '{0}'", o.LD);
                writer.WriteLine("link = '{0}'", o.LD);
                writer.WriteLine("lld-link = '{0}'", o.LD);
                writer.WriteLine("pkg-config = '{0}'", o.PkgConfig);
                writer.WriteLine();
                writer.WriteLine("[host_machine]");
                writer.WriteLine("system = 'windows'");
                writer.WriteLine("cpu_family = 'aarch64'");
                writer.WriteLine("cpu = 'arm64'");
                writer.WriteLine("endian = 'little'");

                File.WriteAllText(
                    Path.Combine(o.BuildDir, "arm64x.txt"),
                    writer.ToString(),
                    new UTF8Encoding(false)
                );
            }

#if true
            var psi = new ProcessStartInfo(
                o.Meson,
                string.Join(
                    " ",
                    "setup",
                    "--cross-file \"" + Path.Combine(o.BuildDir, "arm64x.txt") + "\"",
                    "--prefix \"" + o.Prefix + "\"",
                    "--backend ninja",
                    "--buildtype release",
                    "--pkg-config-path \"" + o.PkgConfigDir + "\"",
                    "-Dplpython=disabled",
                    "-Dldap=disabled",
                    "-Dssl=openssl",
                    "-Dplperl=disabled",
                    "-Dreadline=disabled",
                    "\"" + o.BuildDir + "\""
                )
            )
            {
                UseShellExecute = false,
                WorkingDirectory = o.SourceDir,
            };
            var p = Process.Start(psi)!;
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                Console.Error.WriteLine("Failed to configure PostgreSQL.");
                return p.ExitCode;
            }
#endif


            return 0;
        }

        private static int DoOpenSSLBuild(OpenSSLBuildOpt o)
        {
            var nmakeOpts = (o.Silent) ? " /S" : "";

            int NMake(string args)
            {
                var psi = new ProcessStartInfo(
                    "nmake.exe",
                    $"{nmakeOpts} {args}"
                )
                {
                    UseShellExecute = false,
                    WorkingDirectory = o.SourceDir,
                };
                psi.Environment["CL_EXE"] = o.VC_CL;
                psi.Environment["LINK_EXE"] = o.VC_LINK;
                psi.Environment["LIB_EXE"] = o.VC_LIB;
                var p = Process.Start(psi)!;
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    Console.Error.WriteLine("Failed to build OpenSSL.");
                    return p.ExitCode;
                }

                return 0;
            }

            {
                var errorlevel = NMake($" DESTDIR=\"{o.DestDir}\" ");
                if (errorlevel != 0)
                {
                    return errorlevel;
                }
            }
            {
                var errorlevel = NMake($" DESTDIR=\"{o.DestDir}\" install ");
                if (errorlevel != 0)
                {
                    return errorlevel;
                }
            }

            return 0;
        }

        private static int DoOpenSSLPrepare(OpenSSLPrepareOpt o)
        {
#if true
            var psi = new ProcessStartInfo(
                "perl.exe",
                @" Configure VC-WIN64-ARM "
            )
            {
                UseShellExecute = false,
                WorkingDirectory = o.SourceDir,
            };
            var p = Process.Start(psi)!;
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                Console.Error.WriteLine("Failed to configure OpenSSL.");
                return p.ExitCode;
            }
#endif

            // CC="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjCL\bin\Debug\net8.0-windows\Arm64XDualObjCL.exe"
            // LD="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLINK\bin\Debug\net8.0-windows\Arm64XDualObjLINK.exe"
            // AR="V:\psqlodbc-for-win10-arm64\Toolings\Arm64XDualObjLIB\bin\Debug\net8.0-windows\Arm64XDualObjLIB.exe"

            var makefilePath = Path.Combine(o.SourceDir, "makefile");
            var makefile = File.ReadAllLines(makefilePath, Encoding.UTF8);
            {
                for (int y = 0; y < makefile.Length; y++)
                {
                    if (false) { }
                    else if (makefile[y].StartsWith("CC="))
                    {
                        makefile[y] = $"CC=\"{o.CC}\"";
                    }
                    else if (makefile[y].StartsWith("LD="))
                    {
                        makefile[y] = $"LD=\"{o.LD}\"";
                    }
                    else if (makefile[y].StartsWith("AR="))
                    {
                        makefile[y] = $"AR=\"{o.AR}\"";
                    }
                }
            }
            File.WriteAllLines(makefilePath, makefile, new UTF8Encoding(false));
            return 0;
        }
    }
}
