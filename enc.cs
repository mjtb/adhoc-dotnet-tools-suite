using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Encodes strings in various ways.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class MainClass
{
/*
 * Additional changes.
 */
static Encoding utf8 = new UTF8Encoding(false);
static Random random = new Random();
static readonly string mark= "-_.!-*'()";
static string matches(string a, params string[] B) {
	foreach(string b in B) {
		if(StringComparer.InvariantCultureIgnoreCase.Compare(a,b) == 0) {
			return b;
		}
	}
	return null;
}
static string urienc(string s)
{
   byte[] b = utf8.GetBytes(s);
   StringBuilder buf = new StringBuilder();
   for(int i = 0; i < b.Length; i++)
   {
	bool is_unreserved = false;
	if((b[i] >= 'A' && b[i] <= 'Z') || (b[i] >= 'a' && b[i] <= 'z') || (b[i] >= '0' && b[i] <= '9'))
	{
		is_unreserved = true;
	}
	else
        for(int j = 0; j < mark.Length; j++)
	{
		if(b[i] ==mark[j])
		{
			is_unreserved = true;
			break;
		}
	}
	if(is_unreserved)
	{
		buf.Append((char)b[i]);
	}
	else
	{
		buf.AppendFormat("%{0:X2}",b[i]);
	}
   }
   return buf.ToString();
}
static string hex(string s) {
	return hex(utf8.GetBytes(s));
}
static string hex(byte[] b) {
	StringBuilder buf = new StringBuilder();
	for(int i = 0; i < b.Length; i++) {
		buf.AppendFormat(string.Format("{0:x2}", b[i]));
	}
	return buf.ToString();
}
static string cpp(byte[] b) {
	StringBuilder buf = new StringBuilder();
	int col = 0;
	for(int i = 0; i < b.Length; i++) {
		if(col > 0)
		{
			buf.Append(", ");
		}
		buf.AppendFormat(string.Format("0x{0:X2}", b[i]));
		++col;
		if(col == 16)
		{
			if(i < (b.Length - 1))
			{
				buf.Append(", \\\r\n");
			}
			col = 0;
		}
	}
	return buf.ToString();
}
static byte[] unhex(string s) {
	MemoryStream ms = new MemoryStream();
	int high = 0;
	bool bhigh = false;
	for(int i = 0; i < s.Length; ++i) {
		bool cb = false;
		int c = 0;
		if((s[i] >= '0') && (s[i] <= '9')) {
			cb = true;
			c = s[i] - '0';
		} else if((s[i] >= 'a') && (s[i] <= 'f')) {
			cb = true;
			c = 10 + (s[i] - 'a');
		} else if((s[i] >= 'A') && (s[i] <= 'F')) {
			cb = true;
			c = 10 + (s[i] - 'A');
		}
		if(cb) {
			if(bhigh) {
				ms.WriteByte((byte)((high << 4) | c));
				bhigh = false;
				high = 0;
			} else {
				high = c;
				bhigh = true;
			}
		}
	}
	return ms.ToArray();
}
static string hash(HashAlgorithm alg, byte[] b)
{
	using(alg)
	{
		return hex(alg.ComputeHash(b));
	}
}
static string md5(string s) {
	return md5(utf8.GetBytes(s));
}
static string md5(byte[] b) {
	return hash(MD5.Create(), b);
}
static string sha256(string s) {
	return sha256(utf8.GetBytes(s));
}
static string sha256(byte[] b) {
	return hash(SHA256.Create(), b);
}
static Dictionary<char,string> charname_map = null;
static string unicode(char uchar)
{
	if(charname_map == null)
	{
		string tmpfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "www.unicode.org", "Public", "UNIDATA", "UnicodeData.txt");
		if(!File.Exists(tmpfile))
		{
			Console.Error.WriteLine("info: downloading http://www.unicode.org/Public/UNIDATA/UnicodeData.txt to {0}...", tmpfile);
			Directory.CreateDirectory(Path.GetDirectoryName(tmpfile));
			using(WebClient wc = new WebClient())
			{
				try
				{
					wc.DownloadFile("http://www.unicode.org/Public/UNIDATA/UnicodeData.txt", tmpfile);
				}
				catch(Exception e)
				{
					Console.Error.WriteLine("warning: can\'t download http://www.unicode.org/Public/UNIDATA/UnicodeData.txt; no character names given");
					Console.Error.WriteLine("reason: {0}", e);
				}
			}
		}
		if(File.Exists(tmpfile))
		{
			// Copyright (C) 2011 Triynko. Used under a Creative Commons license: http://creativecommons.org/licenses/by-sa/2.5/
			// See http://stackoverflow.com/questions/2087682/finding-out-unicode-character-name-in-net
			string[] unicodedata = File.ReadAllLines( tmpfile, Encoding.UTF8 );
			charname_map = new Dictionary<char,string>( 65536 );
			for (int i = 0; i < unicodedata.Length; i++)
			{
				string[] fields = unicodedata[i].Split( ';' );
				int char_code = int.Parse( fields[0], NumberStyles.HexNumber );
				string char_name = fields[1];
				if (char_code >= 0 && char_code <= 0xFFFF) //UTF-16 BMP code points only
				{
					bool is_range = char_name.EndsWith( ", First>" );
					if (is_range) //add all characters within a specified range
					{
						char_name.Replace( ", First", String.Empty ); //remove range indicator from name
						fields = unicodedata[++i].Split( ';' );
						int end_char_code = int.Parse( fields[0], NumberStyles.HexNumber );
						if (!fields[1].EndsWith( ", Last>" ))
							throw new Exception( "Expected end-of-range indicator." );
						for (int code_in_range = char_code; code_in_range <= end_char_code; code_in_range++)
						{
							charname_map.Add( (char)code_in_range, char_name );
						}
					}
					else
					{
						charname_map.Add( (char)char_code, char_name );
					}
				}
			}
		}
	}
	byte[] b = utf8.GetBytes(new string(uchar, 1));
	StringBuilder buf = new StringBuilder();
	buf.AppendFormat("U+{0:X4} ", (int)uchar);
	if(charname_map != null && charname_map.ContainsKey(uchar))
	{
		buf.AppendFormat("({0}) ", charname_map[uchar]);
	}
	buf.AppendFormat(" UTF-8:");
	for(int i = 0; i < b.Length; ++i)
	{
		buf.AppendFormat(" 0x{0:X2}", b[i]);
	}
	return buf.ToString();
}

static string getit(string[] args) {
	string s;
	if(args.Length==2) {
		StreamReader f = new StreamReader(new FileStream(args[1], FileMode.Open, FileAccess.Read), utf8);
		s = f.ReadToEnd();
		f.Close();
	} else {
		s = Console.In.ReadToEnd();
	}
	return s;
}
static byte[] getib(string[] args) {
	if(args.Length==2) {
		FileStream f = new FileStream(args[1], FileMode.Open, FileAccess.Read);
		byte[] b = new byte[f.Length];
		f.Read(b, 0, b.Length);
		f.Close();
		return b;
	} else {
		return utf8.GetBytes(Console.In.ReadToEnd());
	}
}
static void Main(string[] args)
{
if((args.Length==1 || args.Length==2) && args[0]=="base64t") {
	Console.WriteLine(Convert.ToBase64String(utf8.GetBytes(getit(args))));
} else if((args.Length==1 || args.Length==2) && args[0]=="base64") {
	Console.WriteLine(Convert.ToBase64String(getib(args)));
} else if((args.Length==1 || args.Length==2) && args[0]=="un-base64") {
	byte[] temp = Convert.FromBase64String(getit(args));
	Stream str = Console.OpenStandardOutput();
 	str.Write(temp, 0, temp.Length);
} else if((args.Length > 1) && args[0]=="utf8") {
	for(int i = 1; i < args.Length; ++i) {
		int cch = 0;
		if(args[i].StartsWith("U+", StringComparison.InvariantCultureIgnoreCase) && int.TryParse(args[i].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out cch))
		{
			Console.WriteLine(unicode((char)cch));
		}
		else
		{
			Console.Error.WriteLine("error: can\'t parse {0} as U+XXXX unicode code point", args[i]);
		}
	}
} else if((args.Length==1 || args.Length==2) && args[0]=="hex") {
	Console.WriteLine(hex(getib(args)));
} else if((args.Length==1 || args.Length==2) && args[0]=="cpp") {
	Console.WriteLine(cpp(getib(args)));
} else if((args.Length==1 || args.Length==2) && args[0]=="un-hex") {
	byte[] temp = unhex(getit(args));
	Stream str = Console.OpenStandardOutput();
 	str.Write(temp, 0, temp.Length);
} else if(args.Length>=1 && args[0]=="uri") {
	for(int i = 1; i < args.Length; i++) {
		Console.WriteLine(urienc(args[i]));
	}
} else if((args.Length==1 || args.Length==2) && args[0]=="md5") {
	Console.WriteLine(md5(getib(args)));
} else if((args.Length==1 || args.Length==2) && args[0]=="sha256") {
	Console.WriteLine(md5(getib(args)));
} else if(args.Length==7 && args[0] == "digest") {
	string username = args[1];
	string password = args[2];
	string realm = args[3];
	string nonce = args[4];
	string method = args[5];
	string uri = args[6];
	Console.WriteLine(md5(md5(username+":"+realm+":"+password)+":"+nonce+":"+md5(method+":"+uri)));
} else if(args.Length==1 && args[0]=="nonce") {
	StringBuilder b = new StringBuilder();
	b.AppendFormat("{0:yyyy-MM-ddTHH:mm:ss}Z", DateTime.Now.ToUniversalTime());
	b.Append(':');
	string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	for(int i = 13 + random.Next(7); i > 0; i--)
	{
		b.Append(ALPHABET[random.Next(ALPHABET.Length)]);
	}
	Console.WriteLine(Convert.ToBase64String(utf8.GetBytes(b.ToString())));

} else {
	Console.Error.WriteLine("Various encoding utilities.");
	Console.Error.WriteLine("enc utf8 U+XXXXXX*                     -- prints the UTF-8 bytes for the given code point");
	Console.Error.WriteLine("enc hex filename?                      -- hex encoding");
	Console.Error.WriteLine("enc cpp filename?                      -- hex as C integers encoding");
	Console.Error.WriteLine("enc un-hex filename?                   -- reverses hex encoding");
	Console.Error.WriteLine("enc base64 filename?                   -- base64 encoding");
	Console.Error.WriteLine("enc base64t filename?                  -- base64 encoding after utf8 encoding");
	Console.Error.WriteLine("enc un-base64 filename?                -- reverses base64 encoding");
	Console.Error.WriteLine("enc md5 filename?                      -- compute MD5 hash");
	Console.Error.WriteLine("enc sha256 filename?                   -- compute SHA2-256 hash");
	Console.Error.WriteLine("enc nonce                              -- generate random nonce for digest encoding");
	Console.Error.WriteLine("enc digest username password realm nonce method uri");
	Console.Error.WriteLine("                                       -- generate WWW-Authenticate for digest encoding");
	Console.Error.WriteLine("enc uri string*                        -- generate URI encoding of string");
}




}
}
