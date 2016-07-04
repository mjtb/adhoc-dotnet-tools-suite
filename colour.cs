using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyTitle(@"Colour Arithmetic")]
[assembly: AssemblyDescription("Performs arithmetic on RGB, HSL and HWB colours.")]
[assembly: AssemblyVersion("0.1.*")]

class color
{
    struct Color
    {
        private static readonly Regex HEX6 = new Regex("#(?<r>[0-9A-Fa-f]{2})(?<g>[0-9A-Fa-f]{2})(?<b>[0-9A-Fa-f]{2})"),
            RGB = new Regex("rgb\\(\\s*(?<r>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<g>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<b>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*\\)"),
            HSL = new Regex("hsl\\(\\s*(?<h>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?\\u00b0?)\\s*,\\s*(?<s>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<l>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*\\)"),
            HWB = new Regex("hwb\\(\\s*(?<h>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?\\u00b0?)\\s*,\\s*(?<w>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<b>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*\\)"),
            HEX3 = new Regex("#(?<r>[0-9A-Fa-f])(?<g>[0-9A-Fa-f])(?<b>[0-9A-Fa-f])");
        private static readonly Color[] NAMED = new Color[]
        {
            new Color("aliceblue", 240,248,255),
            new Color("antiquewhite", 250,235,215),
            new Color("aqua", 0,255,255),
            new Color("aquamarine", 127,255,212),
            new Color("azure", 240,255,255),
            new Color("beige", 245,245,220),
            new Color("bisque", 255,228,196),
            new Color("black", 0,0,0),
            new Color("blanchedalmond", 255,235,205),
            new Color("blue", 0,0,255),
            new Color("blueviolet", 138,43,226),
            new Color("brown", 165,42,42),
            new Color("burlywood", 222,184,135),
            new Color("cadetblue", 95,158,160),
            new Color("chartreuse", 127,255,0),
            new Color("chocolate", 210,105,30),
            new Color("coral", 255,127,80),
            new Color("cornflowerblue", 100,149,237),
            new Color("cornsilk", 255,248,220),
            new Color("crimson", 220,20,60),
            new Color("cyan", 0,255,255),
            new Color("darkblue", 0,0,139),
            new Color("darkcyan", 0,139,139),
            new Color("darkgoldenrod", 184,134,11),
            new Color("darkgray", 169,169,169),
            new Color("darkgreen", 0,100,0),
            new Color("darkgrey", 169,169,169),
            new Color("darkkhaki", 189,183,107),
            new Color("darkmagenta", 139,0,139),
            new Color("darkolivegreen", 85,107,47),
            new Color("darkorange", 255,140,0),
            new Color("darkorchid", 153,50,204),
            new Color("darkred", 139,0,0),
            new Color("darksalmon", 233,150,122),
            new Color("darkseagreen", 143,188,143),
            new Color("darkslateblue", 72,61,139),
            new Color("darkslategray", 47,79,79),
            new Color("darkslategrey", 47,79,79),
            new Color("darkturquoise", 0,206,209),
            new Color("darkviolet", 148,0,211),
            new Color("deeppink", 255,20,147),
            new Color("deepskyblue", 0,191,255),
            new Color("dimgray", 105,105,105),
            new Color("dimgrey", 105,105,105),
            new Color("dodgerblue", 30,144,255),
            new Color("firebrick", 178,34,34),
            new Color("floralwhite", 255,250,240),
            new Color("forestgreen", 34,139,34),
            new Color("fuchsia", 255,0,255),
            new Color("gainsboro", 220,220,220),
            new Color("ghostwhite", 248,248,255),
            new Color("gold", 255,215,0),
            new Color("goldenrod", 218,165,32),
            new Color("gray", 128,128,128),
            new Color("green", 0,128,0),
            new Color("greenyellow", 173,255,47),
            new Color("grey", 128,128,128),
            new Color("honeydew", 240,255,240),
            new Color("hotpink", 255,105,180),
            new Color("indianred", 205,92,92),
            new Color("indigo", 75,0,130),
            new Color("ivory", 255,255,240),
            new Color("khaki", 240,230,140),
            new Color("lavender", 230,230,250),
            new Color("lavenderblush", 255,240,245),
            new Color("lawngreen", 124,252,0),
            new Color("lemonchiffon", 255,250,205),
            new Color("lightblue", 173,216,230),
            new Color("lightcoral", 240,128,128),
            new Color("lightcyan", 224,255,255),
            new Color("lightgoldenrodyellow", 250,250,210),
            new Color("lightgray", 211,211,211),
            new Color("lightgreen", 144,238,144),
            new Color("lightgrey", 211,211,211),
            new Color("lightpink", 255,182,193),
            new Color("lightsalmon", 255,160,122),
            new Color("lightseagreen", 32,178,170),
            new Color("lightskyblue", 135,206,250),
            new Color("lightslategray", 119,136,153),
            new Color("lightslategrey", 119,136,153),
            new Color("lightsteelblue", 176,196,222),
            new Color("lightyellow", 255,255,224),
            new Color("lime", 0,255,0),
            new Color("limegreen", 50,205,50),
            new Color("linen", 250,240,230),
            new Color("magenta", 255,0,255),
            new Color("maroon", 128,0,0),
            new Color("mediumaquamarine", 102,205,170),
            new Color("mediumblue", 0,0,205),
            new Color("mediumorchid", 186,85,211),
            new Color("mediumpurple", 147,112,219),
            new Color("mediumseagreen", 60,179,113),
            new Color("mediumslateblue", 123,104,238),
            new Color("mediumspringgreen", 0,250,154),
            new Color("mediumturquoise", 72,209,204),
            new Color("mediumvioletred", 199,21,133),
            new Color("midnightblue", 25,25,112),
            new Color("mintcream", 245,255,250),
            new Color("mistyrose", 255,228,225),
            new Color("moccasin", 255,228,181),
            new Color("navajowhite", 255,222,173),
            new Color("navy", 0,0,128),
            new Color("oldlace", 253,245,230),
            new Color("olive", 128,128,0),
            new Color("olivedrab", 107,142,35),
            new Color("orange", 255,165,0),
            new Color("orangered", 255,69,0),
            new Color("orchid", 218,112,214),
            new Color("palegoldenrod", 238,232,170),
            new Color("palegreen", 152,251,152),
            new Color("paleturquoise", 175,238,238),
            new Color("palevioletred", 219,112,147),
            new Color("papayawhip", 255,239,213),
            new Color("peachpuff", 255,218,185),
            new Color("peru", 205,133,63),
            new Color("pink", 255,192,203),
            new Color("plum", 221,160,221),
            new Color("powderblue", 176,224,230),
            new Color("purple", 128,0,128),
            new Color("red", 255,0,0),
            new Color("rosybrown", 188,143,143),
            new Color("royalblue", 65,105,225),
            new Color("saddlebrown", 139,69,19),
            new Color("salmon", 250,128,114),
            new Color("sandybrown", 244,164,96),
            new Color("seagreen", 46,139,87),
            new Color("seashell", 255,245,238),
            new Color("sienna", 160,82,45),
            new Color("silver", 192,192,192),
            new Color("skyblue", 135,206,235),
            new Color("slateblue", 106,90,205),
            new Color("slategray", 112,128,144),
            new Color("slategrey", 112,128,144),
            new Color("snow", 255,250,250),
            new Color("springgreen", 0,255,127),
            new Color("steelblue", 70,130,180),
            new Color("tan", 210,180,140),
            new Color("teal", 0,128,128),
            new Color("thistle", 216,191,216),
            new Color("tomato", 255,99,71),
            new Color("turquoise", 64,224,208),
            new Color("violet", 238,130,238),
            new Color("wheat", 245,222,179),
            new Color("white", 255,255,255),
            new Color("whitesmoke", 245,245,245),
            new Color("yellow", 255,255,0),
            new Color("yellowgreen", 154,205,50)
        };
        public readonly double r, g, b, h, s, l, w, k;
        public readonly string name;

        private static double pn(string i, double q = 255.0)
        {
            if (i.EndsWith("%"))
            {
                return double.Parse(i.TrimEnd('%')) / 100.0;
            }
            else
            {
                return double.Parse(i) / q;
            }
        }

        private static int pi(double d, int q = 255)
        {
            if (double.IsNaN(d))
            {
                d = 0;
            }
            else if (double.IsInfinity(d))
            {
                d = 1;
            }
            return Math.Max(0, Math.Min(q, (int)Math.Round(Math.Abs(d) * q)));
        }

        private static void hsl(double r, double g, double b, out double h, out double s, out double l)
        {
            double M = Math.Max(Math.Max(r, g), b);
            double m = Math.Min(Math.Min(r, g), b);
            double C = M - m;
            if (C == 0)
            {
                h = 0;
            }
            else if(M == r)
            {
                h = (g - b) / C;
            }
            else if(M == g)
            {
                h = (b - r) / C + 2;
            }
            else
            {
                h = (r - g) / C + 4;
            }
            h %= 6;
            while(h < 0) {
                h += 6;
            }
            h /= 6;
            l = 0.5 * (M + m);
            if (l == 0 || l == 1)
            {
                s = 0;
            }
            else
            {
                s = C / (1 - Math.Abs(2 * l - 1));
            }
        }

        private static void hwb(double r, double g, double b, out double h, out double w, out double k)
        {
            double M = Math.Max(Math.Max(r, g), b);
            double m = Math.Min(Math.Min(r, g), b);
            double C = M - m;
            if (C == 0)
            {
                h = 0;
            }
            else if(M == r)
            {
                h = (g - b) / C;
            }
            else if(M == g)
            {
                h = (b - r) / C + 2;
            }
            else
            {
                h = (r - g) / C + 4;
            }
            h %= 6;
            while(h < 0)
            {
                h += 6;
            }
            h /= 6;
            double V = M;
            double S = (C == 0) ? 0 : (C / V);
            w = (1 - S) * V;
            k = 1 - V;
        }

        private static void hwb_to_rgb(double h, double w, double k, out double r, out double g, out double b)
        {
            double t = w + k;
            if(t > 1)
            {
                w /= t;
                k /= t;
            }
            t = 1 - w - k;
            rgb(h, 1, 0.5, out r, out g, out b);
            r *= t;
            r += w;
            g *= t;
            g += w;
            b *= t;
            b += w;
        }

        private static double rgb(double m1, double m2, double h)
        {
            if(h < 0)
            {
                h += 1;
            }
            if(h > 1)
            {
                h -= 1;
            }
            if(h*6 < 1)
            {
                return m1 + (m2 - m1) * h * 6;
            }
            else if(h*2<1)
            {
                return m2;
            }
            else if(h*3 < 2)
            {
                return m1 + (m2 - m1) * (2.0 / 3.0 - h) * 6;
            }
            else
            {
                return m1;
            }
        }

        private static void rgb(double h, double s, double l, out double r, out double g, out double b)
        {
            double m2 = l <= 0.5 ? l * (s + 1) : (l + s - l * s);
            double m1 = l * 2 - m2;
            r = rgb(m1, m2, h + 1.0 / 3.0);
            g = rgb(m1, m2, h);
            b = rgb(m1, m2, h - 1.0 / 3.0);
        }

        private Color(string name_, int r__, int g__, int b__)
        {
            double r_ = r__ / 255.0, g_ = g__ / 255.0, b_ = b__ / 255.0, t_ = double.NaN;
            if (string.IsNullOrWhiteSpace(name_))
            {
                foreach (Color c in NAMED)
                {
                    if (c.Equals(r_, g_, b_))
                    {
                        name_ = c.name;
                        break;
                    }
                }
            }
            this.name = name_;
            this.r = r_;
            this.g = g_;
            this.b = b_;
            hsl(r_, g_, b_, out this.h, out this.s, out this.l);
            hwb(r_, g_, b_, out t_, out this.w, out this.k);
        }

        public override int GetHashCode()
        {
            return pi(r) | (pi(g) << 8) | (pi(b) << 16);
        }


        public override bool Equals(object obj)
        {
            if (obj is color)
            {
                Color x = (Color)obj;
                return Equals(x.r, x.g, x.b);
            }
            else
            {
                return false;
            }
        }

        private bool Equals(double xr, double xg, double xb)
        {
            return (pi(xr) == pi(this.r))
                && (pi(xg) == pi(this.g))
                && (pi(xb) == pi(this.b));
        }

        public Color(string input)
        {
            double r_ = double.NaN, g_ = double.NaN, b_ = double.NaN, h_ = double.NaN, s_ = double.NaN, l_ = double.NaN, w_ = double.NaN, k_ = double.NaN, t_ = double.NaN;
            string name_ = null;
            Match m = HEX6.Match(input);
            if (m.Success)
            {
                r_ = int.Parse(m.Groups["r"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0;
                g_ = int.Parse(m.Groups["g"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0;
                b_ = int.Parse(m.Groups["b"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0;
                hsl(r_, g_, b_, out h_, out s_, out l_);
                hwb(r_, g_, b_, out t_, out w_, out k_);
            }
            else
            {
                m = RGB.Match(input);
                if (m.Success)
                {
                    r_ = pn(m.Groups["r"].ToString());
                    g_ = pn(m.Groups["g"].ToString());
                    b_ = pn(m.Groups["b"].ToString());
                    hsl(r_, g_, b_, out h_, out s_, out l_);
                    hwb(r_, g_, b_, out t_, out w_, out k_);
                }
                else
                {
                    m = HSL.Match(input);
                    if (m.Success)
                    {
                        h_ = double.Parse(m.Groups["h"].ToString().TrimEnd('\u00B0')) / 360.0;
                        s_ = double.Parse(m.Groups["s"].ToString().TrimEnd('%')) / 100.0;
                        l_ = double.Parse(m.Groups["l"].ToString().TrimEnd('%')) / 100.0;
                        rgb(h_, s_, l_, out r_, out g_, out b_);
                        hwb(r_, g_, b_, out t_, out w_, out k_);
                    }
                    else
                    {
                        m = HEX3.Match(input);
                        if (m.Success)
                        {
                            r_ = int.Parse(m.Groups["r"].ToString() + m.Groups["r"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0;
                            g_ = int.Parse(m.Groups["g"].ToString() + m.Groups["g"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0;
                            b_ = int.Parse(m.Groups["b"].ToString() + m.Groups["b"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0;
                            hsl(r_, g_, b_, out h_, out s_, out l_);
                            hwb(r_, g_, b_, out t_, out w_, out k_);
                        }
                        else
                        {
                            m = HWB.Match(input);
                            if (m.Success)
                            {
                                h_ = double.Parse(m.Groups["h"].ToString().TrimEnd('\u00B0')) / 360.0;
                                w_ = double.Parse(m.Groups["w"].ToString().TrimEnd('%')) / 100.0;
                                k_ = double.Parse(m.Groups["b"].ToString().TrimEnd('%')) / 100.0;
                                hwb_to_rgb(h_, w_, k_, out r_, out g_, out b_);
                                hsl(r_, g_, b_, out t_, out s_, out l_);
                            }
                            else
                            {
                                foreach (Color c in NAMED)
                                {
                                    if (StringComparer.InvariantCultureIgnoreCase.Compare(input, c.name) == 0)
                                    {
                                        r_ = c.r;
                                        g_ = c.g;
                                        b_ = c.b;
                                        h_ = c.h;
                                        s_ = c.s;
                                        l_ = c.l;
                                        w_ = c.w;
                                        k_ = c.k;
                                        name_ = c.name;
                                        break;
                                    }
                                }
                                if (string.IsNullOrWhiteSpace(name_))
                                {
                                    throw new ArgumentException(string.Format("Not a valid color: \"{0}\"", input));
                                }
                            }
                        }
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(name_))
            {
                foreach (Color c in NAMED)
                {
                    if (c.Equals(r_, g_, b_))
                    {
                        name_ = c.name;
                        break;
                    }
                }
            }
            this.r = r_;
            this.g = g_;
            this.b = b_;
            if(h_ == 1)
            {
                h_ = 0;
            }
            this.h = h_;
            this.s = s_;
            this.l = l_;
            this.w = w_;
            this.k = k_;
            this.name = name_;
        }

        public enum StringFormat
        {
            NAME,
            HEX,
            RGB,
            HSL,
            HWB
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            else
            {
                return ToString(StringFormat.HEX);
            }
        }
        public string ToString(StringFormat format)
        {
            switch(format)
            {
                default:
                    throw new ArgumentException("Invalid color string format");
                case StringFormat.NAME:
                    return name;
                case StringFormat.HEX:
                    return string.Format("#{0:X2}{1:X2}{2:X2}", pi(r), pi(g), pi(b));
                case StringFormat.RGB:
                    return string.Format("rgb({0},{1},{2})", pi(r), pi(g), pi(b));
                case StringFormat.HSL:
                    return string.Format("hsl({0},{1}%,{2}%)", pi(h, 360), pi(s, 100), pi(l, 100));
                case StringFormat.HWB:
                    return string.Format("hwb({0},{1}%,{2}%)", pi(h, 360), pi(w, 100), pi(k, 100));
            }
        }

        public static Color operator+(Color a, Color b)
        {
            return new Color(null, pi(a.r + b.r), pi(a.g + b.g), pi(a.b + b.b));
        }

        public static Color operator*(Color a, Color b)
        {
            return new Color(null, pi(a.r * b.r), pi(a.g * b.g), pi(a.b * b.b));
        }

        public static Color interpolate(Color a, Color b, double q)
        {
            double qq = 1 - q;
            return new Color(null, pi(qq * a.r + q * b.r), pi(qq * a.g + q * b.g), pi(qq * a.b + q * b.b));
        }
    }

    static void Main(string[] args)
    {
        Color? current = null;
        double q = double.NaN;
        char op = '\0';
        string pop = null;
        foreach(string a in args)
        {
            if (a == "+")
            {
                op = '+';
                pop = a;
            }
            else if (a == "*")
            {
                op = '*';
                pop = a;
            }
            else if (a.StartsWith("~"))
            {
                op = '~';
                if (a.EndsWith("%"))
                {
                    q = double.Parse(a.Substring(1, a.Length - 2)) / 100;
                }
                else if (a.Length > 1)
                {
                    q = double.Parse(a.Substring(1));
                }
                else
                {
                    q = 0.5;
                }
                pop = a;
            }
            else
            {
				try
				{
	                Color c = new Color(a);
	                Console.WriteLine(string.Format("{0}:\t{1}\t{2}\t{3}\t{4}{5}", a, c.ToString(Color.StringFormat.HEX),
	                    c.ToString(Color.StringFormat.RGB), c.ToString(Color.StringFormat.HSL), c.ToString(Color.StringFormat.HWB),
	                    string.IsNullOrWhiteSpace(c.name) ? "" : string.Format("\t{0}", c.name)));
	                bool pc = true;
	                if (op == '+')
	                {
	                    current = current + c;
	                    op = '\0';
	                }
	                else if (op == '*')
	                {
	                    current = current * c;
	                    op = '\0';
	                }
	                else if (op == '~')
	                {
	                    current = Color.interpolate(current.Value, c, q);
	                    q = double.NaN;
	                    op = '\0';
	                }
	                else
	                {
	                    current = c;
	                    pc = false;
	                }
	                if (pc)
	                {
	                    Console.WriteLine(string.Format("{0}:\t{1}\t{2}\t{3}\t{4}{5}", pop, current.Value.ToString(Color.StringFormat.HEX),
	                        current.Value.ToString(Color.StringFormat.RGB), current.Value.ToString(Color.StringFormat.HSL),  current.Value.ToString(Color.StringFormat.HWB),
	                        string.IsNullOrWhiteSpace(current.Value.name) ? "" : string.Format("\t{0}", current.Value.name)));
	                    pop = null;
	                }
				}
				catch(Exception e)
				{
					Console.Error.WriteLine(e.Message);
				}
            }
        }
    }
}
