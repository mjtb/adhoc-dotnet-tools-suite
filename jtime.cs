using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Prints date/time values and time intervals in standard Internet formats.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class jtime
{
    private static readonly string ISO8601 = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
    [Flags]
	enum PrintFormat
	{
		None			= 0,
		Javascript		= 1 << 0,
		ISO8601 		= 1 << 1,
		RFC1123			= 1 << 2,
		Localized		= 1 << 3,
		All				= 0xF
	}
    private static void syntax()
    {

        Console.WriteLine("Prints date/times as Javascript time value, ISO 8601, RFC1123 and full localized local time and");
        Console.WriteLine("prints the difference between date/time values using ISO 8601 time interval format.");
        Console.WriteLine("syntax: jtime (--fmt Javascript|ISO8601|RFC1123|Localized)* (--now | date/time | time interval)*");
        Console.WriteLine();
        Console.WriteLine("example 1: print the current time plus the time 22000 ms from now");
        Console.WriteLine("\tjtime --now +22000");
        Console.WriteLine("example 2: print the time given an ISO 8601 date/time and that value offset by a time interval");
        Console.WriteLine("\tjtime \"2013-07-26T22:23:44Z\" \"-P4DT8H3M22.222S\"");
        Console.WriteLine("example 3: print the difference between an RFC1123 time and a full localized local time");
        Console.WriteLine("\tjtime \"Fri, 26 Jul 2013 16:31:14 GMT\" \"Friday, July 26, 2013 10:31:14 AM\"");
    }
    private static bool matches(string arg, params string[] args)
    {
        foreach (string a in args)
        {
            if (StringComparer.InvariantCultureIgnoreCase.Compare(a, arg) == 0)
            {
                return true;
            }
        }
        return false;
    }

    private static void print(PrintFormat fmt, DateTime dt, bool indent, int? x = null)
    {
        if (x.HasValue)
        {
            if((fmt & PrintFormat.Javascript) != 0) Console.WriteLine("T{1}:{2}{0}", (long)Math.Round((dt - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds), x.Value, indent ? "\t" : string.Empty);
            if((fmt & PrintFormat.ISO8601) != 0) Console.WriteLine("{1}{0}", dt.ToString(ISO8601, CultureInfo.InvariantCulture), indent ? "\t" : string.Empty);
            if((fmt & PrintFormat.RFC1123) != 0) Console.WriteLine("{1}{0:r}", dt, indent ? "\t" : string.Empty);
            if((fmt & PrintFormat.Localized) != 0) Console.WriteLine("{1}{0:F}", dt.ToLocalTime(), indent ? "\t" : string.Empty);
        }
        else
        {
            if((fmt & PrintFormat.Javascript) != 0) Console.WriteLine("{1}{0}", (long)Math.Round((dt - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds), indent ? "\t" : string.Empty);
            if((fmt & PrintFormat.ISO8601) != 0) Console.WriteLine("{1}{0}", dt.ToString(ISO8601, CultureInfo.InvariantCulture), indent ? "\t" : string.Empty);
            if((fmt & PrintFormat.RFC1123) != 0) Console.WriteLine("{1}{0:r}", dt, indent ? "\t" : string.Empty);
            if((fmt & PrintFormat.Localized) != 0) Console.WriteLine("{1}{0:F}", dt.ToLocalTime(), indent ? "\t" : string.Empty);
        }
    }

    public static string FormatPeriod(TimeSpan ts)
    {
        if (ts == TimeSpan.Zero)
        {
            return "PT0S";
        }
        StringBuilder buf = new StringBuilder();
        if (ts.TotalMilliseconds < 0)
        {
            buf.Append('-');
            ts = ts.Negate();
        }
        buf.Append('P');
        if (ts.TotalDays >= 1)
        {
            buf.AppendFormat("{0}D", ts.Days);
        }
        if (ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 || ts.Milliseconds > 0)
        {
            buf.Append("T");
            if (ts.Hours > 0)
            {
                buf.AppendFormat("{0}H", ts.Hours);
            }
            if (ts.Minutes > 0)
            {
                buf.AppendFormat("{0}M", ts.Minutes);
            }
            if (ts.Seconds > 0 || ts.Milliseconds > 0)
            {
                if (ts.Milliseconds > 0)
                {
                    buf.AppendFormat("{0}.{1:D3}S", ts.Seconds, ts.Milliseconds);
                }
                else
                {
                    buf.AppendFormat("{0}S", ts.Seconds);
                }
            }

        }
        return buf.ToString();
    }

    private static bool TryParsePeriod(string s, out TimeSpan ts)
    {
        Match m = Regex.Match(s, "^(?<pm>(\\+|-)?)(?<ms>\\d+)$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
        if (m.Success)
        {
            bool minus = false;
            Group pm = m.Groups["pm"];
            if (pm.Success)
            {
                if (pm.Value == "-")
                {
                    minus = true;
                }
            }
            Group ms = m.Groups["ms"];
            if (ms.Success)
            {
                long l;
                if (long.TryParse(ms.Value, out l))
                {
                    ts = TimeSpan.FromMilliseconds(l * (minus ? -1 : 1));
                    return true;
                }
            }
            ts = TimeSpan.Zero;
            return false;
        }
        m = Regex.Match(s, "^(?<pm>\\+|\\-)?P((?<d>\\d+(\\.\\d+)?)D)?(T((?<h>\\d+(\\.\\d+)?)H)?((?<m>\\d+(\\.\\d+)?)M)?((?<s>\\d+(\\.\\d+)?)S)?)?$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
        if (m.Success)
        {
            bool minus = false;
            Group pm = m.Groups["pm"];
            if (pm.Success)
            {
                if (pm.Value == "-")
                {
                    minus = true;
                }
            }
            double D = 0, H = 0, M = 0, S = 0;
            Group d = m.Groups["d"];
            if (d.Success)
            {
                if (!double.TryParse(d.Value, out D))
                {
                    ts = TimeSpan.Zero;
                    return false;
                }
            }
            Group h = m.Groups["h"];
            if (h.Success)
            {
                if (!double.TryParse(h.Value, out H))
                {
                    ts = TimeSpan.Zero;
                    return false;
                }
            }
            Group mn = m.Groups["m"];
            if (mn.Success)
            {
                if (!double.TryParse(mn.Value, out M))
                {
                    ts = TimeSpan.Zero;
                    return false;
                }
            }
            Group ss = m.Groups["s"];
            if (ss.Success)
            {
                if (!double.TryParse(ss.Value, out S))
                {
                    ts = TimeSpan.Zero;
                    return false;
                }
            }
            double T = S + 60 * (M + 60 * (H + 24 * D));
            ts = TimeSpan.FromSeconds(T * (minus ? -1 : 1));
            return true;
        }
        ts = TimeSpan.Zero;
        return false;
    }


    private static void print(TimeSpan ts)
    {
        Console.WriteLine("\t{0} ms = {1}", (long)ts.TotalMilliseconds, FormatPeriod(ts));
    }

    public static void Main(string[] args)
    {
        if((args.Length == 1) && matches(args[0], "/?", "/help", "/h", "-help", "-h", "--help", "--h", "--syntax", "/syntax", "-?", "--?"))
        {
            syntax();
            return;
        }
        DateTime nowtime = DateTime.UtcNow;
        DateTime reftime = nowtime;
        List<DateTime> tl = new List<DateTime>();
        PrintFormat fmt = PrintFormat.All;
        for(int argc = 0; argc < args.Length; ++argc)
        {
			string a = args[argc];
            long l = 0;
            DateTime dt;
            TimeSpan ts;
            if (matches(a, "now", "/now", "-now", "--now"))
            {
                reftime = nowtime;
                tl.Add(reftime);
            }
            else if(((argc + 1) < args.Length) && matches(a, "--format", "/format", "--fmt", "/fmt", "-f", "/f"))
            {
				if(fmt == PrintFormat.All)
				{
					fmt = PrintFormat.None;
				}
				a = args[++argc];
				if(matches(a, "Javascript"))
				{
					fmt = (PrintFormat)(fmt | PrintFormat.Javascript);
				}
				else if(matches(a, "ISO8601"))
				{
					fmt = (PrintFormat)(fmt | PrintFormat.ISO8601);
				}
				else if(matches(a, "RFC1123"))
				{
					fmt = (PrintFormat)(fmt | PrintFormat.RFC1123);
				}
				else if(matches(a, "Localized"))
				{
					fmt = (PrintFormat)(fmt | PrintFormat.Localized);
				}
				else
				{
					syntax();
					return;
				}
            }
            else if (long.TryParse(a, out l) && (a[0] != '+') && (a[0] != '-'))
            {
                reftime = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(l);
                tl.Add(reftime);
            }
            else if (DateTime.TryParseExact(a, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out dt) || DateTime.TryParseExact(a, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out dt))
            {
                reftime = dt;
                tl.Add(reftime);
            }
            else if (DateTime.TryParseExact(a, "r", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out dt))
            {
                reftime = dt;
                tl.Add(reftime);
            }
            else if (DateTime.TryParse(a, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal, out dt))
            {
                reftime = dt;
                tl.Add(reftime);
            }
            else if (TryParsePeriod(a, out ts))
            {
                reftime = reftime + ts;
                tl.Add(reftime);
            }
            else
            {
                Console.Error.WriteLine("error: cannot parse {0} as date/time or time interval", a);
            }
        }
        if (tl.Count == 0)
        {
            tl.Add(nowtime);
        }
        if (tl.Count > 0)
        {
            for (int i = 0; i < tl.Count; ++i)
            {
                if (i > 0)
                {
                    Console.WriteLine();
                }
                print(fmt, tl[i], (tl.Count > 1), (tl.Count > 1) ? new int?(i) : null);
                if (i > 0)
                {
                    Console.WriteLine("Difference from T{0}:", i - 1);
                    print(tl[i] - tl[i - 1]);
                }
            }
        }
    }
}
