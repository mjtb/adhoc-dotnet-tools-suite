using System;
using System.Reflection;
using MJTB;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyTitle(@"Git upload-archive Handler")]
[assembly: AssemblyDescription("Delegates git-upload-archive via SSH to installed Git for Windows.")]
[assembly: AssemblyVersion("0.1.*")]

public class GitUploadArchive
{
    public static void Main(string[] args)
    {
        Environment.Exit(GitWrap.Execute("git-upload-archive", args));
    }
}
