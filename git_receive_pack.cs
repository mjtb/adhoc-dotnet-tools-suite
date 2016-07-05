using System;
using System.Reflection;
using MJTB;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyTitle(@"Git receive-pack Handler")]
[assembly: AssemblyDescription("Delegates git-receive-pack via SSH to installed Git for Windows.")]
[assembly: AssemblyVersion("0.1.*")]

public class GitReceivePack
{
    public static void Main(string[] args)
    {
        Environment.Exit(GitWrap.Execute("git-receive-pack", args));
    }
}
