using CommandLine;
using System.Diagnostics;
using System.Text;

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

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<OpenSSLPrepareOpt, OpenSSLBuildOpt, PostgreSQLPrepareOpt>(args)
                .MapResult<OpenSSLPrepareOpt, OpenSSLBuildOpt, PostgreSQLPrepareOpt, int>(
                    DoOpenSSLPrepare,
                    DoOpenSSLBuild,
                    DoPostgreSQLPrepare,
                    ex => 1
                );
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
