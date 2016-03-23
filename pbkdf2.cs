using System;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Generates a password hash using PBKDF2 with HMACSHA1 as given in IETF RFC 2898.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class pbkdf2
{
	public static void Main(string[] args)
	{
		if(args.Length == 0)
		{
			Console.WriteLine("Generates a password hash using PBKDF2 with HMACSHA1 as given in IETF RFC 2898.");
			Console.WriteLine("syntax: pbkdf2 password (salt=random*|uri|bytes)? (iters=1000)? (len=32)? (enc=base64)?");
			return;
		}
		bool b64 = args.Length > 4 && StringComparer.InvariantCultureIgnoreCase.Compare(args[4], "base64") == 0;
		int len = args.Length > 3 ? int.Parse(args[3]) : 32;
		int iters = args.Length > 2 ? int.Parse(args[2]) : 1000;
		bool gensalt = (args.Length <= 1 || StringComparer.InvariantCultureIgnoreCase.Compare(args[1], "random") == 0);
		byte[] salt = gensalt ? rand(len) : (isuri(args[1]) ? Encoding.UTF8.GetBytes(args[1]) : (b64 ? Convert.FromBase64String(args[1]) : hex(args[1])));
		string pw = args[0];
		Rfc2898DeriveBytes kdf = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(pw), salt, iters);
		byte[] buf = kdf.GetBytes(len);
		Console.WriteLine(string.Format("key= {0}", b64 ? Convert.ToBase64String(buf) : hex(buf)));
		if(gensalt)
		{
			Console.Error.WriteLine(string.Format("salt= {0}", b64 ? Convert.ToBase64String(salt) : hex(salt)));
		}
	}

	private static bool isuri(string u)
	{
		Uri uri;
		return Uri.TryCreate(u, UriKind.Absolute, out uri);
	}

	private static int hex(char c)
	{
		return (c >= '0' && c <= '9') ? (c - '0') : ((c >= 'A' && c <= 'F') ? (10 + (c - 'A')) : ((c >= 'a' && c <= 'f') ? (10 + (c - 'a')) : 0));
	}

	private static byte[] hex(string arg)
	{
		byte[] buf = new byte[arg.Length / 2];
		for(int i = 0; i < buf.Length; ++i)
		{
			buf[i] = (byte)((hex(arg[2 * i]) << 4) | hex(arg[2 * i + 1]));
		}
		return buf;
	}

	private static byte[] rand(int len)
	{
		byte[] buf = new byte[len];
		using(RNGCryptoServiceProvider prng = new RNGCryptoServiceProvider())
		{
			prng.GetBytes(buf);
		}
		return buf;
	}

	private static char hex(int i)
	{
		if(i < 10)
		{
			return (char)('0' + i);
		}
		else
		{
			return (char)('a' + (i - 10));
		}
	}

	private static string hex(byte[] arg)
	{
		StringBuilder buf = new StringBuilder(arg.Length * 2);
		for(int i = 0; i < arg.Length; ++i)
		{
			buf.Append(hex(arg[i] >> 4));
			buf.Append(hex(arg[i] & 15));
		}
		return buf.ToString();
	}
}
