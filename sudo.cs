using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Runs a command using UAC elevated privileges.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class Sudo
{
private static readonly string[] DOS_BUILTIN_COMMANDS = new string[] {
	"ASSOC", "BREAK", "CALL", "CD", "CHCP", "CHDIR", "CLS", "COLOR", "COPY", "DEL", "DIR", "ECHO",
	"ENDLOCAL", "ERASE", "EXIT", "FOR", "FTYPE", "GOTO", "GRAFTABL", "IF", "MD", "MKDIR", "MKLINK",
	"MOVE", "PATH", "PAUSE", "POPD", "PROMPT", "PUSHD", "RD", "REM", "REN", "RENAME", "SET", "SETLOCAL",
	"SHIFT", "START", "TIME", "TITLE", "TYPE", "VER", "VERIFY", "VOL"
};

private static void syntax()
{
	Console.WriteLine("Runs a command using UAC elevated privileges.");
	Console.WriteLine("syntax: sudo (--no-wait) (--no-pause) [command] [args...]");
}

private static bool matches(string arg, params string[] tests)
{
	foreach(string t in tests)
	{
		if(StringComparer.InvariantCultureIgnoreCase.Compare(arg, t) == 0)
		{
			return true;
		}
	}
	return false;
}

private static string driveLetter(string path)
{
  string root = Path.GetPathRoot(path);
  int colon = root.IndexOf(':');
  if(colon > 0)
  {
	return root.Substring(0, colon + 1);
  }
  else
  {
	return root;
  }
}

[DllImport("shlwapi.dll")]
[return: MarshalAs(UnmanagedType.Bool)]
private static extern bool PathFindOnPath([In,Out] StringBuilder pszFile, [In] string[] ppszOtherDirs);

private static readonly string QUOTABLES = "< >&()[]{}^=;!\'\"+,`~";

private static string IsDosBuiltinCommand(string command)
{
	foreach(string XX in DOS_BUILTIN_COMMANDS)
	{
		if(StringComparer.InvariantCultureIgnoreCase.Compare(command, XX) == 0)
		{
			return XX;
		}
	}
	return null;
}

private static string Locate(string command)
{
	string builtin = IsDosBuiltinCommand(command);
	if(!string.IsNullOrWhiteSpace(builtin))
	{
		return builtin;
	}
	if(Path.IsPathRooted(command))
	{
		if(File.Exists(command))
		{
			return command;
		}
	}
	else
	{
		string fullPath = Path.GetFullPath(command);
		if(File.Exists(fullPath))
		{
			return fullPath;
		}
		else
		{
			StringBuilder buf = new StringBuilder(32000);
			buf.Append(command);
			if(PathFindOnPath(buf, null))
			{
				return buf.ToString();
			}
		}
	}
	return null;
}

[STAThread]
public static void Main(string[] args)
{
	if(args.Length == 0 || matches(args[0], "/?", "-?", "--?", "/h", "-h", "--h", "/help", "-help", "--help", "/syntax", "-syntax", "--syntax"))
	{
		syntax();
		Environment.ExitCode = 1;
		return;
	}
	bool nowait = false;
	bool nopause = false;
	int startIndex = 0;
	for(; startIndex < args.Length; ++startIndex)
	{
		if(matches(args[startIndex], "--no-wait", "--nowait", "/no-wait", "/nowait"))
		{
			nowait = true;
		}
		else if(matches(args[startIndex], "--no-pause", "--nopause", "/no-pause", "/nopause"))
		{
			nopause = true;
		}
		else
		{
			break;
		}
	}
	if(startIndex > 0)
	{
		if(args.Length == startIndex)
		{
			syntax();
			Environment.ExitCode = 1;
			return;
		}
		string[] nargs = new string[args.Length - startIndex];
		Array.Copy(args, startIndex, nargs, 0, nargs.Length);
		args = nargs;
	}
	string cmd = Environment.ExpandEnvironmentVariables(args[0]);
	string command = Locate(cmd);
	if(command == null)
	{
		command = Locate(cmd + ".com");
		if(command == null)
		{
			command = Locate(cmd + ".exe");
			if(command == null)
			{
				command = Locate(cmd + ".bat");
				if(command == null)
				{
					command = Locate(cmd + ".cmd");
					if(command == null)
					{
						Console.WriteLine("error: command not found: {0}", cmd);
						Environment.ExitCode = 1;
						return;
					}
				}
			}
		}
	}
	StringBuilder commandline = new StringBuilder();
	string cd = Environment.CurrentDirectory;
	string comspec = Environment.GetEnvironmentVariable("ComSpec");
	if(StringComparer.InvariantCultureIgnoreCase.Compare(command,comspec) == 0)
	{
		if(args.Length == 1)
		{
			nowait = true;
			nopause = true;
		}
	}
	commandline.Append(comspec);
	commandline.Append(" /C ");
	commandline.Append(driveLetter(cd));
	commandline.Append(" & CD ");
	if(cd.IndexOf(' ') >= 0)
	{
	  commandline.Append('\"');
	  commandline.Append(cd);
	  commandline.Append('\"');
	}
	else
	{
	  commandline.Append(cd);
	}
	commandline.Append(" & ");
	if(command.IndexOf(' ') >= 0)
	{
		commandline.Append('\"');
		commandline.Append(command);
		commandline.Append('\"');
	}
	else
	{
		commandline.Append(command);
	}
	command = comspec;
	for(int i = 1; i < args.Length; ++i)
	{
		string a = Environment.ExpandEnvironmentVariables(args[i]);
		commandline.Append(' ');
		bool quotable = false;
		for(int j = 0; j < a.Length; ++j)
		{
			if(QUOTABLES.IndexOf(a[j]) >= 0)
			{
				quotable = true;
			}
		}
		if(quotable)
		{
			commandline.Append('\"');
			for(int j = 0; j < a.Length; ++j)
			{
				char c = a[j];
				if(c == '\"')
				{
					commandline.Append('\"');
					commandline.Append('\"');
					commandline.Append('\"');
				}
				else
				{
					commandline.Append(c);
				}
			}
			commandline.Append('\"');
		}
		else
		{
			commandline.Append(a);
		}
	}
	if(!nopause)
	{
		commandline.Append(" & PAUSE");
	}
	Console.WriteLine(commandline);
	ProcessStartInfo psi = new ProcessStartInfo(command, commandline.ToString());
	psi.Verb = "runas";
	psi.UseShellExecute = true;
	psi.WorkingDirectory = Path.GetDirectoryName(command);
	Process p = Process.Start(psi);
	if(!nowait)
	{
	  p.WaitForExit();
	  Environment.ExitCode = p.ExitCode;
	}
	else
	{
	  Environment.ExitCode = 0;
	}
	return;
}

}
