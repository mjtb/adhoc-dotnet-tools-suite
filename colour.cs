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
        public static readonly Color[] NAMED = new Color[]
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

		public struct RGB
		{
			private static readonly Regex HEX6 = new Regex("#(?<r>[0-9A-Fa-f]{2})(?<g>[0-9A-Fa-f]{2})(?<b>[0-9A-Fa-f]{2})"),
	            DEC = new Regex("rgb\\(\\s*(?<r>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<g>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<b>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*\\)"),
				HEX3 = new Regex("#(?<r>[0-9A-Fa-f])(?<g>[0-9A-Fa-f])(?<b>[0-9A-Fa-f])");
			public double r;
			public double g;
			public double b;
			public RGB(int rgb)
			{
				r = ((rgb >> 16) & 0xFF) / 255.0;
				g = ((rgb >> 8) & 0xFF) / 255.0;
				b = (rgb & 0xFF) / 255.0;
			}
			public static bool TryParse(string s, out RGB rgb)
			{
				Match m = HEX6.Match(s);
	            if (m.Success)
	            {
	                rgb = new RGB(int.Parse(m.Groups["r"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0, int.Parse(m.Groups["g"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0, int.Parse(m.Groups["b"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0);
					return true;
	            }
                m = DEC.Match(s);
                if (m.Success)
                {
                    rgb = new RGB(pn(m.Groups["r"].ToString()), pn(m.Groups["g"].ToString()), pn(m.Groups["b"].ToString()));
					return true;
                }
				m = HEX3.Match(s);
				if (m.Success)
				{
					rgb = new RGB(int.Parse(m.Groups["r"].ToString() + m.Groups["r"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0, int.Parse(m.Groups["g"].ToString() + m.Groups["g"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0, int.Parse(m.Groups["b"].ToString() + m.Groups["b"].ToString(), NumberStyles.AllowHexSpecifier) / 255.0);
					return true;
				}
				rgb = new RGB(double.NaN,double.NaN,double.NaN);
				return false;
			}
			public RGB(double R, double G, double B)
			{
				r = R;
				g = G;
				b = B;
			}
			public string ToHexString()
			{
				return ToHexString(false);
			}
			public string ToHexString(bool three)
			{
				int h = GetHashCode();
				if(three && (((h >> 20) & 0xF) == ((h >> 16) & 0xF)) && (((h >> 12) & 0xF) == ((h >> 8) & 0xF)) && (((h >> 4) & 0xF) == (h & 0xF)))
				{
					return string.Format(CultureInfo.InvariantCulture, "#{0:X3}", (((h >> 16) & 0xF) << 8) | (((h >> 8) & 0xF) << 4) | (h & 0xF));
				}
				else
				{
					return string.Format(CultureInfo.InvariantCulture, "#{0:X6}", h);
				}
			}

			public string ToDecimalString()
			{
				return string.Format(CultureInfo.InvariantCulture, "rgb({0},{1},{2})", pi(r), pi(g), pi(b));
			}

			public override string ToString()
			{
				return ToHexString(false);
			}

			public override int GetHashCode()
			{
				return (pi(r) << 16) | (pi(g) << 8) | pi(b);
			}

			public override bool Equals(object o)
			{
				return (o is RGB) && (GetHashCode() == o.GetHashCode());
			}
		}

		public struct HSL
		{
			private static readonly Regex RE = new Regex("hsl\\(\\s*(?<h>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?\\u00b0?)\\s*,\\s*(?<s>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<l>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*\\)");
			public double h;
			public double s;
			public double l;
			public HSL(double H, double S, double L)
			{
				h = H;
				s = S;
				l = L;
			}
			public static bool TryParse(string s, out HSL hsl)
			{
				Match m = RE.Match(s);
				if (m.Success)
				{
					hsl = new HSL(double.Parse(m.Groups["h"].ToString().TrimEnd('\u00B0')) / 360.0, pn(m.Groups["s"].ToString(), 100), pn(m.Groups["l"].ToString(), 100));
					return true;
				}
				hsl = new HSL(double.NaN, double.NaN, double.NaN);
				return false;
			}
			public HSL(RGB rgb)
			{
				double M = Math.Max(Math.Max(rgb.r, rgb.g), rgb.b);
	            double m = Math.Min(Math.Min(rgb.r, rgb.g), rgb.b);
	            double C = M - m;
	            if (C == 0)
	            {
	                h = 0;
	            }
	            else if(M == rgb.r)
	            {
	                h = (rgb.g - rgb.b) / C;
	            }
	            else if(M == rgb.g)
	            {
	                h = (rgb.b - rgb.r) / C + 2;
	            }
	            else
	            {
	                h = (rgb.r - rgb.g) / C + 4;
	            }
	            h %= 6;
	            while(h < 0)
				{
	                h += 6;
	            }
	            h /= 6;
				if(h == 1)
				{
					h = 0;
				}
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
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "hsl({0},{1}%,{2}%)", pi(h, 360), pi(s, 100), pi(l, 100));
			}
			public override int GetHashCode()
			{
				return (pi(h, 360) << 16) | (pi(s, 100) << 8) | pi(l, 100);
			}
			public override bool Equals(object o)
			{
				return (o is HSL) && (GetHashCode() == o.GetHashCode());
			}
			private static double rho(double m1, double m2, double h)
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

			public RGB ToRGB()
			{
				double m2 = l <= 0.5 ? l * (s + 1) : (l + s - l * s);
	            double m1 = l * 2 - m2;
				return new RGB(rho(m1, m2, h + 1.0 / 3.0), rho(m1, m2, h), rho(m1, m2, h - 1.0 / 3.0));
			}
		}

		public struct HWB
		{
			private static readonly Regex RE = new Regex("hwb\\(\\s*(?<h>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?\\u00b0?)\\s*,\\s*(?<w>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*,\\s*(?<b>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?%?)\\s*\\)");
			public double h;
			public double w;
			public double b;
			public HWB(double H, double W, double B)
			{
				h = H;
				w = W;
				b = B;
			}
			public HWB(RGB rgb)
			{
				double M = Math.Max(Math.Max(rgb.r, rgb.g), rgb.b);
	            double m = Math.Min(Math.Min(rgb.r, rgb.g), rgb.b);
	            double C = M - m;
	            if (C == 0)
	            {
	                h = 0;
	            }
	            else if(M == rgb.r)
	            {
	                h = (rgb.g - rgb.b) / C;
	            }
	            else if(M == rgb.g)
	            {
	                h = (rgb.b - rgb.r) / C + 2;
	            }
	            else
	            {
	                h = (rgb.r - rgb.g) / C + 4;
	            }
	            h %= 6;
	            while(h < 0)
	            {
	                h += 6;
	            }
	            h /= 6;
				if(h == 1)
				{
					h = 0;
				}
	            double V = M;
	            double S = (C == 0) ? 0 : (C / V);
	            w = (1 - S) * V;
	            b = 1 - V;
			}
			public static bool TryParse(string s, out HWB hwb)
			{
				Match m = RE.Match(s);
				if (m.Success)
				{
					hwb = new HWB(double.Parse(m.Groups["h"].ToString().TrimEnd('\u00B0')) / 360.0, pn(m.Groups["w"].ToString(), 100), pn(m.Groups["b"].ToString(), 100));
					return true;
				}
				hwb = new HWB(double.NaN, double.NaN, double.NaN);
				return false;
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "hwb({0},{1}%,{2}%)", pi(h, 360), pi(w, 100), pi(b, 100));
			}
			public override int GetHashCode()
			{
				return (pi(h, 360) << 16) | (pi(w, 100) << 8) | pi(b, 100);
			}
			public override bool Equals(object o)
			{
				return (o is HWB) && (o.GetHashCode() == GetHashCode());
			}
			public RGB ToRGB()
			{
				double W = w;
				double B = b;
				double t = W + B;
	            if(t > 1)
	            {
	                W /= t;
	                B /= t;
	            }
	            t = 1 - W - B;
				HSL hsl = new HSL(h, 1, 0.5);
				RGB rgb = hsl.ToRGB();
	            rgb.r *= t;
	            rgb.r += W;
	            rgb.g *= t;
	            rgb.g += W;
	            rgb.b *= t;
	            rgb.b += W;
				return rgb;
			}
		}

		public struct XYZ
		{
			private static readonly Regex RE = new Regex("xyz\\(\\s*(?<x>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?)\\s*,\\s*(?<y>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?)\\s*,\\s*(?<z>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?)\\s*\\)");
			private static readonly double C = 1.0 / 0.17697;
			private static readonly double[,] M = new double[3,3] {
				{ 0.49000, 0.31000,  0.20000  },
				{ 0.17697, 0.81240,  0.010630 },
				{ 0.0000,  0.010000, 0.99000  }
			};
			private static readonly double[,] N = new double[3,3] {
				{  0.41847,    -0.15866,  -0.082835 },
				{ -0.091169,    0.25243,   0.015708 },
				{  0.00092090, -0.0025498, 0.17860  }
			};
			public double x;
			public double y;
			public double z;

			public XYZ(double X, double Y, double Z)
			{
				x = X;
				y = Y;
				z = Z;
			}
			private static double compandout(double v)
			{
				if(v <= 0.0031308)
				{
					return 12.92 * v;
				}
				else
				{
					return 1.055 * Math.Pow(v, 1 / 2.4) - 0.055;
				}
			}
			private static double compandin(double V)
			{
				if(V <= 0.04045)
				{
					return V / 12.92;
				}
				else
				{
					return Math.Pow((V + 0.055) / 1.055, 2.4);
				}
			}
			public XYZ(RGB srgb)
			{
				RGB rgb = new RGB(compandin(srgb.r), compandin(srgb.g), compandin(srgb.b));
				x = C * (M[0,0] * rgb.r + M[0,1] * rgb.g + M[0,2] * rgb.b);
				y = C * (M[1,0] * rgb.r + M[1,1] * rgb.g + M[1,2] * rgb.b);
				z = C * (M[2,0] * rgb.r + M[2,1] * rgb.g + M[2,2] * rgb.b);
			}
			public static bool TryParse(string s, out XYZ xyz)
			{
				Match m = RE.Match(s);
				if (m.Success)
				{
					xyz = new XYZ(pn(m.Groups["x"].ToString(), 1), pn(m.Groups["y"].ToString(), 1), pn(m.Groups["z"].ToString(), 1));
					return true;
				}
				xyz = new XYZ(double.NaN, double.NaN, double.NaN);
				return false;
			}
			public override int GetHashCode()
			{
				return (pi(x,1023) << 20) | (pi(y,1023) << 10) | pi(z,1023);
			}
			public override bool Equals(object o)
			{
				return (o is XYZ) && (o.GetHashCode() == GetHashCode());
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "xyz({0:0.000},{1:0.000},{2:0.000})", x, y, z);
			}
			public RGB ToRGB()
			{
				double r = N[0,0] * x + N[0,1] * y + N[0,1] * z;
				double g = N[1,0] * x + N[1,1] * y + N[1,1] * z;
				double b = N[2,0] * x + N[2,1] * y + N[2,1] * z;
				return new RGB(compandout(r), compandout(g), compandout(b));
			}
		}
		public struct LAB
		{
			private static readonly Regex RE = new Regex("lab\\(\\s*(?<l>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?)\\s+(?<a>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?)\\s+(?<b>(0|[1-9][0-9]{0,2})(\\.[0-9]+)?)\\s*\\)");
			public double l;
			public double a;
			public double b;
			public LAB(double L, double A, double B)
			{
				l = L;
				a = A;
				b = B;
			}
			public static bool TryParse(string s, out LAB lab)
			{
				Match m = RE.Match(s);
				if (m.Success)
				{
					lab = new LAB(pn(m.Groups["l"].ToString(), 1), pn(m.Groups["a"].ToString(), 1), pn(m.Groups["b"].ToString(), 1));
					return true;
				}
				lab = new LAB(double.NaN, double.NaN, double.NaN);
				return false;
			}
			public LAB(RGB rgb)
			{
				l = double.NaN;
				a = double.NaN;
				b = double.NaN;
			}
			public LAB(XYZ xyz)
			{
				l = double.NaN;
				a = double.NaN;
				b = double.NaN;
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "lab({0:0.000} {1:0.000} {2:0.000})", l, a, b);
			}
			public override int GetHashCode()
			{
				return (pi(l, 100, 0, 1023) << 20) | (pi(a, 160, -160, 1023) << 10) | pi(b, 160, -160, 1023);
			}
			public override bool Equals(object o)
			{
				return (o is LAB) && (o.GetHashCode() == GetHashCode());
			}
			public RGB ToRGB()
			{
				return new RGB(double.NaN, double.NaN, double.NaN);
			}
			public XYZ ToXYZ()
			{
				return new XYZ(double.NaN, double.NaN, double.NaN);
			}
		}
        public readonly RGB rgb;
		public readonly HSL hsl;
		public readonly HWB hwb;
		public readonly LAB lab;
		public readonly XYZ xyz;
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
			return pi(d, 1, 0, q);
		}
		private static int pi(double d, double m, double n, int q)
		{
			if (double.IsNaN(d))
            {
				return 0;
            }
            else if (double.IsInfinity(d))
            {
				if(d > 0)
				{
					d = m;
				}
				else
				{
					d = n;
				}
            }
			return Math.Max(0, Math.Min(q, (int)Math.Round((d - n) / (m - n) * q)));
		}

		private Color(string name_, int r, int g, int b)
        {
            rgb = new RGB(r / 255.0, g / 255.0, b / 255.0);
			hsl = new HSL(rgb);
			hwb = new HWB(rgb);
			lab = new LAB(rgb);
			xyz = new XYZ(rgb);
            if (string.IsNullOrWhiteSpace(name_))
            {
                foreach (Color c in NAMED)
                {
                    if (c.rgb.Equals(rgb))
                    {
                        name_ = c.name;
                        break;
                    }
                }
            }
            name = name_;
        }

        public override int GetHashCode()
        {
			return rgb.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                Color x = (Color)obj;
                return rgb.Equals(x.rgb);
            }
            else
            {
                return false;
            }
        }

        public Color(string input)
        {
			string name_ = null;
			if(RGB.TryParse(input, out rgb))
			{
				hsl = new HSL(rgb);
				hwb = new HWB(rgb);
				lab = new LAB(rgb);
				xyz = new XYZ(rgb);
			}
			else if(HSL.TryParse(input, out hsl))
			{
				rgb = hsl.ToRGB();
				hwb = new HWB(rgb);
				lab = new LAB(rgb);
				xyz = new XYZ(rgb);
			}
			else if(HWB.TryParse(input, out hwb))
			{
				rgb = hwb.ToRGB();
				hsl = new HSL(rgb);
				lab = new LAB(rgb);
				xyz = new XYZ(rgb);
			}
			else if(LAB.TryParse(input, out lab))
			{
				rgb = lab.ToRGB();
				hsl = new HSL(rgb);
				hwb = new HWB(rgb);
				xyz = new XYZ(rgb);
			}
			else if(XYZ.TryParse(input, out xyz))
			{
				rgb = xyz.ToRGB();
				hsl = new HSL(rgb);
				hwb = new HWB(rgb);
				lab = new LAB(xyz);
			}
			else
			{
				foreach (Color c in NAMED)
				{
					if (StringComparer.InvariantCultureIgnoreCase.Compare(input, c.name) == 0)
					{
						rgb = c.rgb;
						hsl = c.hsl;
						hwb = c.hwb;
						lab = c.lab;
						xyz = c.xyz;
						name_ = c.name;
						break;
					}
				}
				if (string.IsNullOrWhiteSpace(name_))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Not a valid color: \"{0}\"", input));
				}
			}
            if (string.IsNullOrWhiteSpace(name_))
            {
                foreach (Color c in NAMED)
                {
                    if (c.rgb.Equals(rgb))
                    {
                        name_ = c.name;
                        break;
                    }
                }
            }
			name = name_;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            else
            {
                return rgb.ToString();
            }
        }

        public static Color operator+(Color a, Color b)
        {
            return new Color(null, pi(a.rgb.r + b.rgb.r), pi(a.rgb.g + b.rgb.g), pi(a.rgb.b + b.rgb.b));
        }

        public static Color operator*(Color a, Color b)
        {
            return new Color(null, pi(a.rgb.r * b.rgb.r), pi(a.rgb.g * b.rgb.g), pi(a.rgb.b * b.rgb.b));
        }

        public static Color interpolate(Color a, Color b, double q)
        {
            double qq = 1 - q;
            return new Color(null, pi(qq * a.rgb.r + q * b.rgb.r), pi(qq * a.rgb.g + q * b.rgb.g), pi(qq * a.rgb.b + q * b.rgb.b));
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
	                Console.WriteLine(string.Format("{0}:\t{1}\t{2}\t{3}\t{4}\t{5}{6}", a, c.rgb.ToHexString(),
	                    c.rgb.ToDecimalString(), c.hsl.ToString(), c.hwb.ToString(), c.xyz.ToString(),
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
	                    Console.WriteLine(string.Format("{0}:\t{1}\t{2}\t{3}\t{4}\t{5}{6}", pop, current.Value.rgb.ToHexString(),
	                        current.Value.rgb.ToDecimalString(), current.Value.hsl.ToString(),  current.Value.hwb.ToString(), current.Value.xyz.ToString(),
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
