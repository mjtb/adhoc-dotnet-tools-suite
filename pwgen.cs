using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Generates random passwords.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

public class pwgen
{

    public static void Main(string[] args)
    {

        int strength = 80;
        string template = null;
        if (args.Length > 0)
        {
            try
            {
                strength = int.Parse(args[0]);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("syntax: pwgen (bits=80)? (template=all)?");
                Console.Error.WriteLine("templates:");
                foreach (Template t in Template.Templates)
                {
                    Console.Error.WriteLine(t.ToString());
                }
                Environment.Exit(0);
                return;
            }
            if (args.Length > 1)
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
                    Console.Error.WriteLine("syntax: pwgen (bits=80)? (template=all)?");
                    Console.Error.WriteLine("templates:");
                    foreach (Template t in Template.Templates)
                    {
                        Console.Error.WriteLine(t.ToString());
                    }
                    Environment.Exit(1);
                    return;
                }
            }
        }

        foreach (Template t in Template.Templates)
        {
            if (template == null || t.Name.Equals(template))
            {
                String pw = t.Generate(strength);
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
