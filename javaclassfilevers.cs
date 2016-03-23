using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Prints out the major version of Java .class files.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class javaclassfilevers
{
	static void printSyntax()
	{
		Console.WriteLine("Prints out the major version of Java .class files.");
		Console.WriteLine("syntax: javaclassfilevers [dirs|file.class]*");
	}

	static string matches(string x, params string[] cand)
	{
		foreach(string c in cand)
		{
			if(StringComparer.InvariantCultureIgnoreCase.Compare(x,c) == 0)
			{
				return c;
			}
		}
		return null;
	}

	static void Main(string[] args)
	{
		if(args.Length > 0 && matches(args[0], "--help", "/help", "-h", "/h", "-?", "/?", "--syntax", "/syntax") != null)
		{
			printSyntax();
		}
		else if(args.Length == 0)
		{
			if(!processFileOrDirectory("."))
			{
				Console.Error.WriteLine("warn: no Java *.class files found");
			}
		}
		else
		{
			bool b = false;
			foreach(string x in args)
			{
				if(processFileOrDirectory(x))
				{
					b = true;
				}
			}
			if(!b)
			{
				Console.Error.WriteLine("warn: no Java *.classs files found");
			}
		}
	}

	static bool processFileOrDirectory(string x)
	{
		byte[] buf = new byte[4 + 2 + 2];
		if(x.ToLower(CultureInfo.InvariantCulture).EndsWith(".class") && File.Exists(x))
		{
			return processFile(x, buf);
		}
		else if(Directory.Exists(x))
		{
			bool rv = false;
			foreach (string file in Directory.EnumerateFiles(x, "*.class", SearchOption.AllDirectories))
			{
				if(processFile(file, buf))
				{
					rv = true;
				}
			}
			return rv;
		}
		else
		{
			Console.Error.WriteLine(string.Format("warn: argument \"{0}\" does not appear to be a not a Java .class file or a directory; skipped", x));
			return false;
		}
	}

	static bool processFile(string file, byte[] buf)
	{
		try
		{
			FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
			int n = fs.Read(buf, 0, buf.Length);
			fs.Close();
			int version;
			if (processHeaderBytes(buf, n, out version))
			{
				Console.WriteLine("{0}: {1} ({2})", file, version, lu(version));
				return true;
			}
		}
		catch { }
		return false;
	}

	static string lu(int version)
	{
		if(version < 45)
		{
			return "Unknown";
		}
		else if(version <= 48)
		{
			return string.Format("JDK 1.{0}", 1 + version - 45);
		}
		else if(version <= 52)
		{
			return string.Format("Java {0}", 5 + version - 49);
		}
		else
		{
			return string.Format("Unknown, likely Java {0}", version - 52 + 8);
		}
	}

	static bool processHeaderBytes(byte[] buf, int len, out int version)
	{
		if (len >= buf.Length && buf[0] == 0xCA && buf[1] == 0xFE && buf[2] == 0xBA && buf[3] == 0xBE)
		{
			version = (buf[6] << 8) | buf[7];
			return true;
		}
		else
		{
			version = 0;
			return false;
		}
	}
}
