using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Encodes unicode as Javascript escape sequences.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class jsutf
{

	public static void Main(string[] args)
	{
		foreach(string s in File.ReadAllLines(args[0], Encoding.UTF8))
		{
			Console.WriteLine(fix(s));
		}
	}

	private static string fix(string x)
	{
		StringBuilder buf = new StringBuilder();
		for(int i = 0; i < x.Length; ++i)
		{
			char c = x[i];
			if(c == '\t' || c == '\r' || c == '\n' || (c >= ' ' && c <= '~'))
			{
				buf.Append(c);
			}
			else
			{
				buf.Append(string.Format(CultureInfo.InvariantCulture, "\\u{0:X4}", (int) c));
			}
		}
		return buf.ToString();
	}
}
