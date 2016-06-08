using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Generates passwords encoded in various formats from random bits or PBKDF2 hashes.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

public class pwgen
{

	static readonly string syntax = "syntax: pwgen (bits=80)? (template=all)? (key:hex|(pw:utf salt:[uri|hex] iters=1000))?";
    public static void Main(string[] args)
    {
        int strength = 80;
        string template = null;
		byte[] entropy = null;
        if (args.Length > 0)
        {
            try
            {
                strength = int.Parse(args[0]);
            }
            catch (Exception)
            {
                Console.Error.WriteLine(syntax);
                Console.Error.WriteLine("templates:");
                foreach (Template t in Template.Templates)
                {
                    Console.Error.WriteLine(t.ToString());
                }
                Environment.Exit(0);
                return;
            }
			if(strength < 8 || strength % 8 != 0)
			{
				strength = ((Math.Max(strength, 8) + 7) / 8) * 8;
				Console.Error.WriteLine(string.Format("warning: increasing strength to {0} bits", strength));
			}
            if (args.Length > 1)
            {
				if(StringComparer.OrdinalIgnoreCase.Compare(args[1], "all") != 0
					&& StringComparer.OrdinalIgnoreCase.Compare(args[1], "any") != 0
					&& args[1] != "*")
				{
	                foreach (Template t in Template.Templates)
	                {
	                    if (StringComparer.InvariantCultureIgnoreCase.Compare(args[1], t.Name) == 0)
	                    {
	                        template = t.Name;
	                        break;
	                    }
	                }
	                if (template == null)
	                {
	                    Console.Error.WriteLine(syntax);
	                    Console.Error.WriteLine("templates:");
	                    foreach (Template t in Template.Templates)
	                    {
	                        Console.Error.WriteLine(t.ToString());
	                    }
	                    Environment.Exit(1);
	                    return;
	                }
				}
				if(args.Length > 2)
				{
					if(args.Length == 3)
					{
						entropy = hex(args[2]);
						if(entropy.Length * 8 < strength)
						{
							Console.Error.WriteLine(string.Format("warning: requested strength={0} bits but key provided is only {1} bits; reducing strength", strength, entropy.Length * 8));
							strength = entropy.Length * 8;
						}
						else if(entropy.Length * 8 > strength)
						{
							Console.Error.WriteLine(string.Format("warning: requested strength={0} bits but key provided is {1} bits; truncating key", strength, entropy.Length * 8));
							byte[] buf = new byte[(strength + 7) / 8];
							Array.Copy(entropy, 0, buf, 0, buf.Length);
							entropy = buf;
						}
					}
					else if(args.Length >= 4)
					{
						string pw = args[2];
						byte[] salt;
						if(StringComparer.OrdinalIgnoreCase.Compare(args[3], "random") == 0)
						{
							salt = rand((strength + 7) / 8);
							Console.Error.WriteLine(string.Format("salt= {0}", hex(salt)));
						}
						else if(isuri(args[3]))
						{
							salt = Encoding.UTF8.GetBytes(args[3]);
						}
						else
						{
							salt = hex(args[3]);
						}
						if(salt.Length < 8)
						{
							Console.Error.WriteLine("warning: padding salt with zeros on the right to a minimum of 64 bits");
							byte[] buf = new byte[8];
							Array.Copy(salt, 0, buf, 0, salt.Length);
							salt = buf;
						}
						int iters = 1000;
						if(args.Length > 4)
						{
							try
							{
								iters = int.Parse(args[4]);
							}
							catch(Exception)
							{
								Console.Error.WriteLine(syntax);
				                Console.Error.WriteLine("templates:");
				                foreach (Template t in Template.Templates)
				                {
				                    Console.Error.WriteLine(t.ToString());
				                }
				                Environment.Exit(0);
				                return;
							}
						}
						Rfc2898DeriveBytes kdf = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(pw), salt, iters);
						entropy = kdf.GetBytes((strength + 7) / 8);
					}
				}
            }
        }
		if(entropy == null)
		{
			entropy = rand((strength + 7) / 8);
		}
        foreach (Template t in Template.Templates)
        {
            if (template == null || t.Name.Equals(template))
            {
                String pw = t.Encode(entropy);
                if (template == null)
                {
                    Console.WriteLine(string.Format(
                            "{0} => {1} [{2} chars]", t.Name, pw, pw.Length));
                }
                else
                {
                    Console.WriteLine(pw);
                }
            }
        }
        Environment.Exit(0);
    }

	private static bool isuri(string u)
	{
		Uri uri;
		return Uri.TryCreate(u, UriKind.Absolute, out uri);
	}

	private static int hex(char c)
	{
		return (c >= '0' && c <= '9') ? (c - '0') : ((c >= 'A' && c <= 'F') ? (10 + (c - 'A')) : ((c >= 'a' && c <= 'f') ? (10 + (c - 'a')) : -1));
	}

	private static byte[] hex(string arg)
	{
		int n = 0;
		for(int i = 0; i < arg.Length; ++i)
		{
			if(hex(arg[i]) >= 0)
			{
				n++;
			}
		}
		byte[] buf = new byte[(n+1)/2];
		n = -1;
		for(int i = 0, j = 0; i < arg.Length; ++i)
		{
			int c = hex(arg[i]);
			if(c >= 0)
			{
				if(n < 0)
				{
					n = c;
				}
				else
				{
					buf[j] = (byte)((n << 4) | c);
					++j;
					n = -1;
				}
			}
		}
		if(n >= 0)
		{
			buf[buf.Length - 1] = (byte)(n << 4);
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

    public class Template
    {

        public static readonly Template Decimal;
        public static readonly Template Hexadecimal;
        public static readonly Template Alphanumeric;
        public static readonly Template NCName;
        public static readonly Template QWERTY;
        public static readonly Template Latin_1;
        public static readonly Template LGC;
        public static readonly Template Nmtoken;
		public static readonly Template Xmlid;
        public static readonly Template[] Templates;

        public class CharRange
        {
            public static readonly char EnDash = '\u2013';
            public readonly char Min, Max;

            public CharRange(char c)
            {
                Min = c;
                Max = c;
            }

            public CharRange(char m, char n)
            {
                Min = m;
                Max = n;
            }

            private static String ToString(char c, bool quote)
            {
                if (c > ' ' && c <= '~')
                {
                    if (quote)
                    {
                        return string.Format("\"{0}\"", c);
                    }
                    else
                    {
                        return char.ToString(c);
                    }
                }
                else
                {
                    return string.Format("#x{0:X}", (int)c);
                }
            }

            public override string ToString()
            {
                if (Min == Max)
                {
                    return ToString(Min, true);
                }
                else
                {
                    StringBuilder buf = new StringBuilder();
                    buf.Append('[');
                    buf.Append(ToString(Min, false));
                    buf.Append(EnDash);
                    buf.Append(ToString(Max, false));
                    buf.Append(']');
                    return buf.ToString();
                }
            }

            public int Size
            {
                get
                {
                    return Max - Min + 1;
                }
            }

            public char this[int offset]
            {
                get
                {
                    return (char)(Min + offset);
                }
            }

        }

        public class CharRanges
        {

            public readonly List<CharRange> Ranges;

            public CharRanges()
            {
                Ranges = new List<CharRange>();
            }

            public CharRanges Add(CharRange r)
            {
                Ranges.Add(r);
                return this;
            }

            public CharRanges Add(char c)
            {
                Ranges.Add(new CharRange(c));
                return this;
            }

            public CharRanges Add(char m, char n)
            {
                Ranges.Add(new CharRange(m, n));
                return this;
            }

            public int Size
            {
                get
                {
                    int s = 0;
                    foreach (CharRange r in Ranges)
                    {
                        s += r.Size;
                    }
                    return s;
                }
            }

            public char this[int offset]
            {
                get
                {
                    foreach (CharRange r in Ranges)
                    {
                        int s = r.Size;
                        if (offset < s)
                        {
                            return r[offset];
                        }
                        else
                        {
                            offset -= s;
                        }
                    }
                    return '\0';
                }
            }

            public override string ToString()
            {
                StringBuilder buf = new StringBuilder();
                foreach (CharRange r in Ranges)
                {
                    if (buf.Length > 0)
                    {
                        buf.Append(" | ");
                    }
                    buf.Append(r.ToString());
                }
                return buf.ToString();
            }

            public bool Empty
            {
                get
                {
                    {
                        return Ranges.Count == 0;
                    }
                }

            }
        }

        public readonly String Name;
        public readonly CharRanges First;
        public readonly CharRanges Ranges;

        public Template(String n)
        {
            Name = n;
            First = new CharRanges();
            Ranges = new CharRanges();
        }

        public Template Add(CharRange r)
        {
            Ranges.Add(r);
            return this;
        }

        public Template Add(char c)
        {
            Ranges.Add(c);
            return this;
        }

        public Template Add(char m, char n)
        {
            Ranges.Add(m, n);
            return this;
        }

        public Template AddFirst(CharRange r)
        {
            First.Add(r);
            return this;
        }

        public Template AddFirst(char c)
        {
            First.Add(c);
            return this;
        }

        public Template AddFirst(char m, char n)
        {
            First.Add(m, n);
            return this;
        }

        public bool HasFirst
        {
            get
            {
                return !First.Empty;
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(Name);
            buf.Append(" ::= ");
            if (HasFirst)
            {
                buf.Append('(');
                buf.Append(First.ToString());
                buf.Append(')');
                buf.Append(' ');
            }
            buf.Append('(');
            buf.Append(Ranges.ToString());
            buf.Append(')');
            buf.Append(HasFirst ? '*' : '+');
            return buf.ToString();
        }

        public String Encode(byte[] bytes)
        {
            StringBuilder buf = new StringBuilder();
            BigInteger i = BigInteger.Abs(new BigInteger(bytes));
            double bits = bytes.Length * 8, b;
            int f, j;
            char c;
            BigInteger g, r, q;
            if (HasFirst)
            {
                f = First.Size;
                g = new BigInteger(f);
                q = BigInteger.DivRem(i, g, out r);
                i = q;
                j = (int)r;
                c = First[j];
                buf.Append(c);
                b = Math.Log(f) / Math.Log(2);
                bits -= b;
            }
            f = Ranges.Size;
            g = new BigInteger(f);
            b = Math.Log(f) / Math.Log(2);
            while (bits > 0)
            {
                q = BigInteger.DivRem(i, g, out r);
                i = q;
                j = (int)r;
                c = Ranges[j];
                buf.Append(c);
                bits -= b;
            }
            return buf.ToString();
        }

        public string Generate(int bits)
        {
            RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
            byte[] buf = new byte[(bits + 7) / 8];
            rnd.GetBytes(buf);
            return Encode(buf);
        }

        static Template()
        {
            Decimal = new Template("Decimal")
					.Add('0', '9');
            Hexadecimal = new Template("Hexadecimal")
					.Add('0', '9')
                	.Add('A', 'F');
            Alphanumeric = new Template("Alphanumeric")
					.Add('0', '9')
                    .Add('a', 'z');
            NCName = new Template("NCName")
					.AddFirst('A', 'Z')
                    .AddFirst('a', 'z')
					.AddFirst('_')
					.Add('0', '9')
                    .Add('A', 'Z')
					.Add('a', 'z')
					.Add('_')
					.Add('.')
					.Add('-');
            QWERTY = new Template("QWERTY")
					.Add('!', '~');
            Latin_1 = new Template("Latin-1")
					.Add('!', '~')
					.Add('\u00A1', '\u00FF');
            LGC = new Template("LGC")
					.Add('!', '~')
                    .Add('\u00A1', '\u024F')
					.Add('\u0370', '\u0373')
                    .Add('\u037B', '\u037F')
					.Add('\u0386')
                    .Add('\u0388', '\u038A')
					.Add('\u038C')
                    .Add('\u038E', '\u03A1')
					.Add('\u03A3', '\u03FF')
                    .Add('\u0400', '\u0482')
					.Add('\u048A', '\u04FF');
			Xmlid = new Template("xml:id")
					.AddFirst('A', 'Z')
                    .AddFirst('_')
					.AddFirst('a', 'z')
					.AddFirst('\u00C0', '\u00D6')
                    .AddFirst('\u00D8', '\u00F6')
					.AddFirst('\u00F8', '\u02FF')
                    .AddFirst('\u0370', '\u037D')
					.AddFirst('\u037F', '\u1FFF')
                    .AddFirst('\u200C', '\u200D')
					.AddFirst('\u2070', '\u218F')
                    .AddFirst('\u2C00', '\u2FEF')
					.AddFirst('\u3001', '\uD7FF')
                    .AddFirst('\uF900', '\uFDCF')
					.AddFirst('\uFDF0', '\uFFFD')
					.Add('A', 'Z')
                    .Add('_')
					.Add('a', 'z')
					.Add('\u00C0', '\u00D6')
                    .Add('\u00D8', '\u00F6')
					.Add('\u00F8', '\u02FF')
                    .Add('\u0370', '\u037D')
					.Add('\u037F', '\u1FFF')
                    .Add('\u200C', '\u200D')
					.Add('\u2070', '\u218F')
                    .Add('\u2C00', '\u2FEF')
					.Add('\u3001', '\uD7FF')
                    .Add('\uF900', '\uFDCF')
					.Add('\uFDF0', '\uFFFD')
					.Add('-')
                    .Add('.')
					.Add('0', '9')
					.Add('\u00B7')
                    .Add('\u0300', '\u036F')
					.Add('\u203F', '\u2040');
            Nmtoken = new Template("Nmtoken")
					.Add('A', 'Z')
					.Add('_')
					.Add('a', 'z')
					.Add('\u00C0', '\u00D6')
					.Add('\u00D8', '\u00F6')
					.Add('\u00F8', '\u02FF')
					.Add('\u0370', '\u037D')
					.Add('\u037F', '\u1FFF')
					.Add('\u200C', '\u200D')
					.Add('\u2070', '\u218F')
					.Add('\u2C00', '\u2FEF')
					.Add('\u3001', '\uD7FF')
					.Add('\uF900', '\uFDCF')
					.Add('\uFDF0', '\uFFFD')
					.Add('-')
					.Add('.')
					.Add('0', '9')
					.Add('\u00B7')
					.Add('\u0300', '\u036F')
					.Add('\u203F', '\u2040');
            Templates = new Template[] { Decimal, Hexadecimal, Alphanumeric,
                    NCName, QWERTY, Latin_1, LGC, Xmlid, Nmtoken };
        }
    }
}
