using System;
using System.IO;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Column wraps text content from stdin to stdout.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class Wrap
{

	public static void syntax() {
		Console.Error.WriteLine("Column wraps text content from stdin to stdout");
		Console.Error.WriteLine("wrap (\\d+) --first-line=(\\d+)");
		Environment.ExitCode = 1;
	}

	public static void Main(string[] args) {
		if(args.Length < 1) {
			syntax();
			return;
		}
		int limit = 0;
		if(!int.TryParse(args[0], out limit)) {
			Console.Error.WriteLine("Error: cannot parse \"{0}\" as an integer greater than 0", args[0]);
			syntax();
			return;
		}
		if(limit < 1) {
			Console.Error.WriteLine("Error: cannot parse \"{0}\" as an integer greater than 0", args[0]);
			syntax();
			return;
		}
		int firstLine = limit;
		if(args.Length > 1) {
			if(args[1].StartsWith("--first-line=")) {
				string fn = args[1].Substring(13, args[1].Length - 13);
				Console.Error.WriteLine(fn);
				if(!int.TryParse(fn, out firstLine)) {
					Console.Error.WriteLine("Error: cannot parse the value in \"--first-line={0}\" as an integer greater than 0", args[0]);
					syntax();
					return;
				}
				if(firstLine < 1) {
					Console.Error.WriteLine("Error: cannot parse the value in \"--first-line={0}\" as an integer greater than 0", args[0]);
					syntax();
					return;
				}
			} else {
				Console.Error.WriteLine("Error: unrecognized command-line option: \"{0}\"", args[1]);
				syntax();
				return;
			}
		}
		string s = Console.ReadLine();
		if(!string.IsNullOrWhiteSpace(s)) {
			emit(s, firstLine, limit);
		}
		while(true) {
			s = Console.ReadLine();
			if(s == null) {
				break;
			}
			emit(s, limit, limit);
		}
	}

	public static void emit(string s, int firstLine, int limit) {
		bool f = false;
		int j = 0;
		for(int i = 0; i < s.Length; ++i) {
			if((f && (j >= limit)) || (!f && (j >= firstLine))) {
				Console.WriteLine();
				j = 0;
				f = true;
			}
			Console.Write(s[i]);
			++j;
		}
		if(j > 0) {
			Console.WriteLine();
		}
	}
}
