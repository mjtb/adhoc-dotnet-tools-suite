using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyTitle(@"Colour Arithmetic")]
[assembly: AssemblyDescription("Performs arithmetic on RGB, HSL and HWB colours.")]
[assembly: AssemblyVersion("0.1.*")]

    struct Colour
    {
        public static readonly Colour[] Standard = new Colour[]
        {
            new Colour("alice blue", 240,248,255),
            new Colour("antique white", 250,235,215),
            new Colour("aqua", 0,255,255),
            new Colour("aquamarine", 127,255,212),
            new Colour("azure", 240,255,255),
            new Colour("beige", 245,245,220),
            new Colour("bisque", 255,228,196),
            new Colour("black", 0,0,0),
            new Colour("blanched almond", 255,235,205),
            new Colour("blue", 0,0,255),
            new Colour("blue violet", 138,43,226),
            new Colour("brown", 165,42,42),
            new Colour("burly wood", 222,184,135),
            new Colour("cadet blue", 95,158,160),
            new Colour("chartreuse", 127,255,0),
            new Colour("chocolate", 210,105,30),
            new Colour("coral", 255,127,80),
            new Colour("cornflower blue", 100,149,237),
            new Colour("cornsilk", 255,248,220),
            new Colour("crimson", 220,20,60),
            new Colour("cyan", 0,255,255),
            new Colour("dark blue", 0,0,139),
            new Colour("dark cyan", 0,139,139),
            new Colour("dark goldenrod", 184,134,11),
            new Colour("dark green", 0,100,0),
            new Colour("dark grey", 169,169,169),
            new Colour("dark khaki", 189,183,107),
            new Colour("dark magenta", 139,0,139),
            new Colour("dark olive green", 85,107,47),
            new Colour("dark orange", 255,140,0),
            new Colour("dark orchid", 153,50,204),
            new Colour("dark red", 139,0,0),
            new Colour("dark salmon", 233,150,122),
            new Colour("dark sea green", 143,188,143),
            new Colour("dark slate blue", 72,61,139),
            new Colour("dark slate grey", 47,79,79),
            new Colour("dark turquoise", 0,206,209),
            new Colour("dark violet", 148,0,211),
            new Colour("deep pink", 255,20,147),
            new Colour("deep sky blue", 0,191,255),
            new Colour("dim grey", 105,105,105),
            new Colour("dodger blue", 30,144,255),
            new Colour("firebrick", 178,34,34),
            new Colour("floral white", 255,250,240),
            new Colour("forest green", 34,139,34),
            new Colour("fuchsia", 255,0,255),
            new Colour("gainsboro", 220,220,220),
            new Colour("ghost white", 248,248,255),
            new Colour("gold", 255,215,0),
            new Colour("goldenrod", 218,165,32),
            new Colour("green", 0,128,0),
            new Colour("green yellow", 173,255,47),
            new Colour("grey", 128,128,128),
            new Colour("honeydew", 240,255,240),
            new Colour("hot pink", 255,105,180),
            new Colour("indian red", 205,92,92),
            new Colour("indigo", 75,0,130),
            new Colour("ivory", 255,255,240),
            new Colour("khaki", 240,230,140),
            new Colour("lavender", 230,230,250),
            new Colour("lavender blush", 255,240,245),
            new Colour("lawn green", 124,252,0),
            new Colour("lemon chiffon", 255,250,205),
            new Colour("light blue", 173,216,230),
            new Colour("light coral", 240,128,128),
            new Colour("light cyan", 224,255,255),
            new Colour("light goldenrod yellow", 250,250,210),
            new Colour("light green", 144,238,144),
            new Colour("light grey", 211,211,211),
            new Colour("light pink", 255,182,193),
            new Colour("light salmon", 255,160,122),
            new Colour("light sea green", 32,178,170),
            new Colour("light sky blue", 135,206,250),
            new Colour("light slate grey", 119,136,153),
            new Colour("light steel blue", 176,196,222),
            new Colour("light yellow", 255,255,224),
            new Colour("lime", 0,255,0),
            new Colour("lime green", 50,205,50),
            new Colour("linen", 250,240,230),
            new Colour("magenta", 255,0,255),
            new Colour("maroon", 128,0,0),
            new Colour("medium aquamarine", 102,205,170),
            new Colour("medium blue", 0,0,205),
            new Colour("medium orchid", 186,85,211),
            new Colour("medium purple", 147,112,219),
            new Colour("medium sea green", 60,179,113),
            new Colour("medium slate blue", 123,104,238),
            new Colour("medium spring green", 0,250,154),
            new Colour("medium turquoise", 72,209,204),
            new Colour("medium violet red", 199,21,133),
            new Colour("midnight blue", 25,25,112),
            new Colour("mint cream", 245,255,250),
            new Colour("misty rose", 255,228,225),
            new Colour("moccasin", 255,228,181),
            new Colour("navajo white", 255,222,173),
            new Colour("navy", 0,0,128),
            new Colour("old lace", 253,245,230),
            new Colour("olive", 128,128,0),
            new Colour("olive drab", 107,142,35),
            new Colour("orange", 255,165,0),
            new Colour("orange red", 255,69,0),
            new Colour("orchid", 218,112,214),
            new Colour("pale goldenrod", 238,232,170),
            new Colour("pale green", 152,251,152),
            new Colour("pale turquoise", 175,238,238),
            new Colour("pale violet red", 219,112,147),
            new Colour("papaya whip", 255,239,213),
            new Colour("peach puff", 255,218,185),
            new Colour("peru", 205,133,63),
            new Colour("pink", 255,192,203),
            new Colour("plum", 221,160,221),
            new Colour("powder blue", 176,224,230),
            new Colour("purple", 128,0,128),
            new Colour("red", 255,0,0),
            new Colour("rosy brown", 188,143,143),
            new Colour("royal blue", 65,105,225),
            new Colour("saddle brown", 139,69,19),
            new Colour("salmon", 250,128,114),
            new Colour("sandy brown", 244,164,96),
            new Colour("sea green", 46,139,87),
            new Colour("seashell", 255,245,238),
            new Colour("sienna", 160,82,45),
            new Colour("silver", 192,192,192),
            new Colour("sky blue", 135,206,235),
            new Colour("slate blue", 106,90,205),
            new Colour("slate grey", 112,128,144),
            new Colour("snow", 255,250,250),
            new Colour("spring green", 0,255,127),
            new Colour("steel blue", 70,130,180),
            new Colour("tan", 210,180,140),
            new Colour("teal", 0,128,128),
            new Colour("thistle", 216,191,216),
            new Colour("tomato", 255,99,71),
            new Colour("turquoise", 64,224,208),
            new Colour("violet", 238,130,238),
            new Colour("wheat", 245,222,179),
            new Colour("white", 255,255,255),
            new Colour("white smoke", 245,245,245),
            new Colour("yellow", 255,255,0),
            new Colour("yellow green", 154,205,50)
        };

		public struct RGB
		{
			public const double R_MIN = 0, R_MAX = 1, G_MIN = 0, G_MAX = 1, B_MIN = 0, B_MAX = 1, R_PRECISION = 0.0025, G_PRECISION = 0.0025, B_PRECISION = 0.0025;
			private static readonly Regex HEX6 = new Regex("#(?<r>[0-9A-Fa-f]{2})(?<g>[0-9A-Fa-f]{2})(?<b>[0-9A-Fa-f]{2})"),
	            DEC = new Regex("rgb\\(\\s*(?<r>(0|[1-9]\\d{0,2})(\\.\\d+)?%?|\\*)\\s*,\\s*(?<g>(0|[1-9]\\d{0,2})(\\.\\d+)?%?|\\*)\\s*,\\s*(?<b>(0|[1-9]\\d{0,2})(\\.\\d+)?%?|\\*)\\s*\\)"),
				HEX3 = new Regex("#(?<r>[0-9A-Fa-f])(?<g>[0-9A-Fa-f])(?<b>[0-9A-Fa-f])");
			private static double unlin(double v)
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
			private static double mklin(double V)
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
			public double r;
			public double linr { get { return mklin(r); } set { r = unlin(value); } }
			public double g;
			public double ling { get { return mklin(g); } set { g = unlin(value); } }
			public double b;
			public double linb { get { return mklin(b); } set { b = unlin(value); } }
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
				return string.Format(CultureInfo.InvariantCulture, "rgb({0},{1},{2})", ps(r, 0, 255), ps(g, 0, 255), ps(b, 0, 255));
			}

			public string ToPercentageString()
			{
				return string.Format(CultureInfo.InvariantCulture, "rgb({0}%,{1}%,{2}%)", ps(r, digits(R_PRECISION), 100), ps(g, digits(G_PRECISION), 100), ps(b, digits(B_PRECISION), 100));
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

			public static bool operator==(RGB a, RGB b)
			{
				return a.GetHashCode() == b.GetHashCode();
			}
			public static bool operator!=(RGB a, RGB b)
			{
				return a.GetHashCode() != b.GetHashCode();
			}
			public static bool operator<(RGB a, RGB b)
			{
				return a.GetHashCode() < b.GetHashCode();
			}
			public static bool operator>(RGB a, RGB b)
			{
				return a.GetHashCode() > b.GetHashCode();
			}

			public static double operator-(RGB a, RGB b)
			{
				return Math.Sqrt(Math.Pow(a.r - b.r, 2) + Math.Pow(a.g - b.g, 2) + Math.Pow(a.b - b.b, 2));
			}

			public static double[] operator%(RGB a, RGB b)
			{
				double[] rv = new double[3];
				rv[0] = a.r - b.r;
				rv[1] = a.g - b.g;
				rv[2] = a.b - b.b;
				return rv;
			}

			public bool IsValid
			{
				get
				{
					const double M = -0.5/255.0, N = 255.5/255.0;
					return r >= M && r <= N && g >= M && g <= N && b >= M && b <= N;
				}
			}
			public RGB Clip()
			{
				if(IsValid)
				{
					return this;
				}
				else
				{
					RGB rv = new RGB();
					rv.r = Math.Min(1, Math.Max(0, r));
					rv.g = Math.Min(1, Math.Max(0, g));
					rv.b = Math.Min(1, Math.Max(0, b));
					return rv;
				}
			}
			public RGB Fix()
			{
				if(IsValid)
				{
					return this;
				}
				LCH origch = new LCH(this);
				LAB origab = origch.ToLAB();
				RGB clippr = Clip();
				double clippd = origab - new LAB(clippr);
				LCH Q = origch;
				RGB R = clippr;
				double D = clippd;
				bool B = false;
				for(int t = 0; t <= 15; ++t)
				{
					for(int u = 0; u <= 20; ++u)
					{
						for(int v = 0; v <= 40; ++v)
						{
							for(int x = 0; x <= 7; ++x)
							{
								LCH q = new LCH();
								q.c = Math.Max(0, Math.Min(230, origch.c + v * ((x & 1) == 0 ? 1 : -1)));
								q.l = Math.Max(0, Math.Min(100, origch.l + u * (((x >> 1) & 1) == 0 ? 1 : -1)));
								q.h = origch.h + t * (((x >> 2) & 1) == 0 ? 1 : -1);
								while(q.h < 0)
								{
									q.h += 360;
								}
								while(q.h > 360)
								{
									q.h -= 360;
								}
								RGB rx = q.ToRGB();
								if(rx.IsValid)
								{
									double d = origab - q.ToLAB();
									if(d < D)
									{
										B = true;
										D = d;
										Q = q;
										R = rx;
									}
								}
							}
						}
					}
				}
				if(B)
				{
					return R;
				}
				return clippr;
			}

			public RGB Interpolate(RGB q, double alpha)
			{
				double beta = 1 - alpha;
				RGB rv = new RGB();
				rv.r = alpha * r + beta * q.r;
				rv.g = alpha * g + beta * q.g;
				rv.b = alpha * b + beta * q.b;
				return rv;
			}
		}

		public struct HSL
		{
			public const double H_MIN = 0, H_MAX = 360, S_MIN = 0, S_MAX = 1, L_MIN = 0, L_MAX = 1, H_PRECISION = 0.00025, S_PRECISION = 0.001, L_PRECISION = 0.001;
			private static readonly Regex RE = new Regex("hsl\\(\\s*(?<h>(0|[1-9]\\d{0,2})(\\.\\d+)?°?|\\*)\\s*,\\s*(?<s>(0|[1-9]\\d{0,2})(\\.\\d+)?%?|\\*)\\s*,\\s*(?<l>(0|[1-9]\\d{0,2})(\\.\\d+)?%?|\\*)\\s*\\)");
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
					hsl = new HSL(pm(pn(m.Groups["h"].ToString(), 360), 360), pn(m.Groups["s"].ToString(), 100), pn(m.Groups["l"].ToString(), 100));
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
				return string.Format(CultureInfo.InvariantCulture, "hsl({0},{1}%,{2}%)", ps(h, digits(H_PRECISION), 360), ps(s, digits(S_PRECISION * 100), 100), ps(l, digits(L_PRECISION * 100), 100));
			}
			public override int GetHashCode()
			{
				return (pi(h, H_MAX, H_MIN, 1023) << 20) | (pi(s, S_MAX, S_MIN, 1023) << 10) | pi(l, L_MAX, L_MIN, 1023);
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

			public static bool operator==(HSL a, HSL b)
			{
				return a.GetHashCode() == b.GetHashCode();
			}
			public static bool operator!=(HSL a, HSL b)
			{
				return a.GetHashCode() != b.GetHashCode();
			}
			public static bool operator<(HSL a, HSL b)
			{
				return a.GetHashCode() < b.GetHashCode();
			}
			public static bool operator>(HSL a, HSL b)
			{
				return a.GetHashCode() > b.GetHashCode();
			}

			public static double operator-(HSL a, HSL b)
			{
				double ax = a.s * Math.Cos(a.h * Math.PI / 180.0);
				double ay = a.s * Math.Sin(a.h * Math.PI / 180.0);
				double bx = b.s * Math.Cos(b.h * Math.PI / 180.0);
				double by = b.s * Math.Sin(b.h * Math.PI / 180.0);
				return Math.Sqrt(Math.Pow(a.l - b.l, 2) + Math.Pow(ax - bx, 2) + Math.Pow(ay - by, 2));
			}

			public static double[] operator%(HSL a, HSL b)
			{
				double[] rv = new double[3];
				rv[0] = RotSub(a.h, b.h);
				rv[1] = a.s - b.s;
				rv[2] = a.l - b.l;
				return rv;
			}


			public HSL Interpolate(HSL q, double alpha)
			{
				double beta = 1 - alpha;
				HSL rv = new HSL();
				rv.h = RotMul(alpha, h, beta, q.h);
				rv.s = alpha * s + beta * q.s;
				rv.l = alpha * l + beta * q.l;
				return rv;
			}

		}

		public struct HWB
		{
			public const double H_MIN = 0, H_MAX = 360, W_MIN = 0, W_MAX = 1, B_MIN = 0, B_MAX = 1, H_PRECISION = 0.00025, W_PRECISION = 0.0025, B_PRECISION = 0.0025;
			private static readonly Regex RE = new Regex("hwb\\(\\s*(?<h>(0|[1-9]\\d{0,2})(\\.\\d+)?°?|\\*)\\s*,\\s*(?<w>(0|[1-9]\\d{0,2})(\\.\\d+)?%?|\\*)\\s*,\\s*(?<b>(0|[1-9]\\d{0,2})(\\.\\d+)?%?|\\*)\\s*\\)");
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
					hwb = new HWB(pm(pn(m.Groups["h"].ToString(), 360), 360), pn(m.Groups["w"].ToString(), 100), pn(m.Groups["b"].ToString(), 100));
					return true;
				}
				hwb = new HWB(double.NaN, double.NaN, double.NaN);
				return false;
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "hwb({0},{1}%,{2}%)", ps(h, digits(H_PRECISION), 360), ps(w, digits(W_PRECISION * 100), 100), ps(b, digits(B_PRECISION * 100), 100));
			}
			public override int GetHashCode()
			{
				return (pi(h, H_MAX, H_MIN, 1023) << 20) | (pi(w, W_MAX, W_MIN, 1023) << 10) | pi(b, B_MAX, B_MIN, 1023);
			}
			public override bool Equals(object o)
			{
				return (o is HWB) && (o.GetHashCode() == GetHashCode());
			}
			public static bool operator==(HWB a, HWB b)
			{
				return a.GetHashCode() == b.GetHashCode();
			}
			public static bool operator!=(HWB a, HWB b)
			{
				return a.GetHashCode() != b.GetHashCode();
			}
			public static bool operator<(HWB a, HWB b)
			{
				return a.GetHashCode() < b.GetHashCode();
			}
			public static bool operator>(HWB a, HWB b)
			{
				return a.GetHashCode() > b.GetHashCode();
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

			public static double operator-(HWB a, HWB b)
			{
				double ax = a.b * Math.Cos(a.h * Math.PI / 180.0);
				double ay = a.b * Math.Sin(a.h * Math.PI / 180.0);
				double bx = b.b * Math.Cos(b.h * Math.PI / 180.0);
				double by = b.b * Math.Sin(b.h * Math.PI / 180.0);
				return Math.Sqrt(Math.Pow(a.w - b.w, 2) + Math.Pow(ax - bx, 2) + Math.Pow(ay - by, 2));
			}

			public static double[] operator%(HWB a, HWB b)
			{
				double[] rv = new double[3];
				rv[0] = RotSub(a.h, b.h);
				rv[1] = a.w - b.w;
				rv[2] = a.b - b.b;
				return rv;
			}

			public HWB Interpolate(HWB q, double alpha)
			{
				double beta = 1 - alpha;
				HWB rv = new HWB();
				rv.h = RotMul(alpha, h, beta, q.h);
				rv.w = alpha * w + beta * q.w;
				rv.b = alpha * b + beta * q.b;
				return rv;
			}
		}

		public struct XYZ
		{
			public const double X_MIN = 0, X_MAX = 0.95047, Y_MIN = 0, Y_MAX = 1, Z_MIN = 0, Z_MAX = 1.08883, X_PRECISION = 0.00005, Y_PRECISION = 0.00005, Z_PRECISION = 0.00005;
			private static readonly Regex RE = new Regex("xyz\\(\\s*(?<x>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s*,\\s*(?<y>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s*,\\s*(?<z>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s*\\)");
			private static readonly double[,] M = new double[3,3] {
				{ 0.4124564, 0.3575761, 0.1804375 },
				{ 0.2126729, 0.7151522, 0.0721750 },
				{ 0.0193339, 0.1191920, 0.9503041 }
			};
			private static readonly double[,] N = new double[3,3] {
				{  3.2404542, -1.5371385, -0.4985314 },
				{ -0.9692660,  1.8760108,  0.0415560 },
				{  0.0556434, -0.2040259,  1.0572252 }
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
			public XYZ(RGB rgb)
			{
				x = M[0,0] * rgb.linr + M[0,1] * rgb.ling + M[0,2] * rgb.linb;
				y = M[1,0] * rgb.linr + M[1,1] * rgb.ling + M[1,2] * rgb.linb;
				z = M[2,0] * rgb.linr + M[2,1] * rgb.ling + M[2,2] * rgb.linb;
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
				return (pi(x,X_MAX,X_MIN,1023) << 20) | (pi(y,Y_MAX,Y_MIN,1023) << 10) | pi(z,Z_MAX,Z_MIN,1023);
			}
			public override bool Equals(object o)
			{
				return (o is XYZ) && (o.GetHashCode() == GetHashCode());
			}
			public static bool operator==(XYZ a, XYZ b)
			{
				return a.GetHashCode() == b.GetHashCode();
			}
			public static bool operator!=(XYZ a, XYZ b)
			{
				return a.GetHashCode() != b.GetHashCode();
			}
			public static bool operator<(XYZ a, XYZ b)
			{
				return a.GetHashCode() < b.GetHashCode();
			}
			public static bool operator>(XYZ a, XYZ b)
			{
				return a.GetHashCode() > b.GetHashCode();
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "xyz({0},{1},{2})", ps(x, digits(X_PRECISION)), ps(y, digits(Y_PRECISION)), ps(z, digits(Z_PRECISION)));
			}
			public RGB ToRGB()
			{
				RGB rgb = new RGB();
				rgb.linr = N[0,0] * x + N[0,1] * y + N[0,2] * z;
				rgb.ling = N[1,0] * x + N[1,1] * y + N[1,2] * z;
				rgb.linb = N[2,0] * x + N[2,1] * y + N[2,2] * z;
				return rgb;
			}
			private static readonly double[,] D50 = new double[,] {
				{  1.0478112, 0.0228866, -0.0501270 },
				{  0.0295424, 0.9904844, -0.0170491 },
				{ -0.0092345, 0.0150436,  0.7521316 }
			};
			private static readonly double[,] D65 = new double[,] {
				{  0.9555766, -0.0230393, 0.0631636 },
				{ -0.0282895,  1.0099416, 0.0210077 },
				{  0.0122982, -0.0204830, 1.3299098 }
			};
			public XYZ ToD50()
			{
				XYZ d50 = new XYZ();
				d50.x = D50[0,0] * x + D50[0,1] * y + D50[0,2] * z;
				d50.y = D50[1,0] * x + D50[1,1] * y + D50[1,2] * z;
				d50.z = D50[2,0] * x + D50[2,1] * y + D50[2,2] * z;
				return d50;
			}
			public XYZ ToD65()
			{
				XYZ d65 = new XYZ();
				d65.x = D65[0,0] * x + D65[0,1] * y + D65[0,2] * z;
				d65.y = D65[1,0] * x + D65[1,1] * y + D65[1,2] * z;
				d65.z = D65[2,0] * x + D65[2,1] * y + D65[2,2] * z;
				return d65;
			}
			public static double operator-(XYZ a, XYZ b)
			{
				return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
			}

			public static double[] operator%(XYZ a, XYZ b)
			{
				double[] rv = new double[3];
				rv[0] = a.x - b.x;
				rv[1] = a.y - b.y;
				rv[2] = a.z - b.z;
				return rv;
			}

			public XYZ Interpolate(XYZ q, double alpha)
			{
				double beta = 1 - alpha;
				XYZ rv = new XYZ();
				rv.x = alpha * x + beta * q.x;
				rv.y = alpha * y + beta * q.y;
				rv.z = alpha * z + beta * q.z;
				return rv;
			}
		}
		public struct LAB
		{
			public const double L_MIN = 0, L_MAX = 100, A_MIN = -80, A_MAX = 94, B_MIN = -113, B_MAX = 94, L_PRECISION = 0.01, A_PRECISION = 0.01, B_PRECISION = 0.01;
			private static readonly Regex RE = new Regex("lab\\(\\s*(?<l>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s+(?<a>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s+(?<b>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s*\\)");
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
			public LAB(RGB rgb) : this(new XYZ(rgb)) { }
			private const double K = 24389.0 / 27.0;
			private const double E = 216.0 / 24389.0;
			private static double labin(double v)
			{
				if(v > E)
				{
					return Math.Pow(v, 1.0 / 3.0);
				}
				else
				{
					return (K * v + 16.0) / 116.0;
				}
			}
			public LAB(XYZ xyz)
			{
				XYZ d50 = xyz.ToD50();
				XYZ f = new XYZ();
				f.x = labin(d50.x / 0.9642);
				f.y = labin(d50.y);
				f.z = labin(d50.z / 0.8249);
				l = (116.0 * f.y) - 16.0;
				a = 500.0 * (f.x - f.y);
				b = 200.0 * (f.y - f.z);
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "lab({0} {1} {2})", ps(l, digits(L_PRECISION)), ps(a, digits(A_PRECISION)), ps(b, digits(B_PRECISION)));
			}
			public override int GetHashCode()
			{
				return (pi(l, L_MAX, L_MIN, 1023) << 20) | (pi(a, A_MAX, A_MIN, 1023) << 10) | pi(b, B_MAX, B_MIN, 1023);
			}
			public override bool Equals(object o)
			{
				return (o is LAB) && (o.GetHashCode() == GetHashCode());
			}
			public static bool operator==(LAB a, LAB b)
			{
				return a.GetHashCode() == b.GetHashCode();
			}
			public static bool operator!=(LAB a, LAB b)
			{
				return a.GetHashCode() != b.GetHashCode();
			}
			public static bool operator<(LAB a, LAB b)
			{
				return a.GetHashCode() < b.GetHashCode();
			}
			public static bool operator>(LAB a, LAB b)
			{
				return a.GetHashCode() > b.GetHashCode();
			}
			public RGB ToRGB()
			{
				return ToXYZ().ToRGB();
			}
			private static double labout(double v)
			{
				double v3 = Math.Pow(v, 3);
				if(v3 > E)
				{
					return v3;
				}
				else
				{
					return (116.0 * v - 16.0) / K;
				}
			}
			public XYZ ToXYZ()
			{
				XYZ f = new XYZ();
				f.y = (l + 16.0) / 116.0;
				f.x = a / 500.0 + f.y;
				f.z = f.y - b / 200.0;
				XYZ d50 = new XYZ();
				d50.x = labout(f.x) * 0.9642;
				d50.y = (l > (K * E) ? Math.Pow((l + 16.0) / 116.0, 3) : (l / K));
				d50.z = labout(f.z) * 0.8249;
				return d50.ToD65();
			}
			public static double operator-(LAB a, LAB b)
			{
				return Math.Sqrt(Math.Pow(a.l - b.l, 2) + Math.Pow(a.a - b.a, 2) + Math.Pow(a.b - b.b, 2));
			}

			public static double[] operator%(LAB a, LAB b)
			{
				double[] rv = new double[3];
				rv[0] = a.l - b.l;
				rv[1] = a.a - b.a;
				rv[2] = a.b - b.b;
				return rv;
			}

			public LAB Interpolate(LAB q, double alpha)
			{
				double beta = 1 - alpha;
				LAB rv = new LAB();
				rv.l = alpha * l + beta * q.l;
				rv.a = alpha * a + beta * q.a;
				rv.b = alpha * b + beta * q.b;
				return rv;
			}
		}

		public struct LCH
		{
			public const double L_MIN = 0, L_MAX = 100, C_MIN = 0, C_MAX = 132, H_MIN = 0, H_MAX = 360, L_PRECISION = 0.01, C_PRECISION = 0.01, H_PRECISION = 0.01;
			private static readonly Regex RE = new Regex("lch\\(\\s*(?<l>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s+(?<c>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?|\\*)\\s+(?<h>(-|\\+)?(0|[1-9]\\d{0,2})(\\.\\d+)?°?|\\*)\\s*\\)");
			public double l;
			public double c;
			public double h;
			public LCH(double L, double C, double H)
			{
				l = L;
				c = C;
				h = H;
			}
			public LCH(LAB lab)
			{
				l = lab.l;
				c = Math.Sqrt(lab.a * lab.a + lab.b * lab.b);
				h = 180.0 / Math.PI * Math.Atan2(lab.b, lab.a);
				while(h < 0)
				{
					h += 360;
				}
				while(h >= 360)
				{
					h -= 360;
				}
			}
			public LCH(XYZ xyz) : this(new LAB(xyz)) {}
			public LCH(RGB rgb) : this(new LAB(rgb)) {}
			public static bool TryParse(string s, out LCH lch)
			{
				Match m = RE.Match(s);
				if (m.Success)
				{
					lch = new LCH(pn(m.Groups["l"].ToString(), 1), pn(m.Groups["c"].ToString(), 1), pm(pn(m.Groups["h"].ToString(), 360), 360));
					return true;
				}
				lch = new LCH(double.NaN, double.NaN, double.NaN);
				return false;
			}
			public override int GetHashCode()
			{
				return (pi(l, L_MAX, L_MIN, 1023) << 20) | (pi(c, C_MAX, C_MIN, 1023) << 10) | pi(h, H_MAX, H_MIN, 1023);
			}
			public static bool operator==(LCH a, LCH b)
			{
				return a.GetHashCode() == b.GetHashCode();
			}
			public static bool operator!=(LCH a, LCH b)
			{
				return a.GetHashCode() != b.GetHashCode();
			}
			public static bool operator<(LCH a, LCH b)
			{
				return a.GetHashCode() < b.GetHashCode();
			}
			public static bool operator>(LCH a, LCH b)
			{
				return a.GetHashCode() > b.GetHashCode();
			}
			public override bool Equals(object o)
			{
				return (o is LCH) && (o.GetHashCode() == GetHashCode());
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "lch({0} {1} {2})", ps(l, digits(L_PRECISION)), ps(c, digits(C_PRECISION)), ps(h, digits(H_PRECISION)));
			}
			public LAB ToLAB()
			{
				double rad = Math.PI / 180.0 * h;
				return new LAB(l, c * Math.Cos(rad), c * Math.Sin(rad));
			}
			public RGB ToRGB()
			{
				return ToLAB().ToRGB();
			}
			public static double operator-(LCH a, LCH b)
			{
				double ax = a.c * Math.Cos(a.h * Math.PI / 180.0);
				double ay = a.c * Math.Sin(a.h * Math.PI / 180.0);
				double bx = b.c * Math.Cos(b.h * Math.PI / 180.0);
				double by = b.c * Math.Sin(b.h * Math.PI / 180.0);
				return Math.Sqrt(Math.Pow(a.l - b.l, 2) + Math.Pow(ax - bx, 2) + Math.Pow(ay - by, 2));
			}
			public static double[] operator%(LCH a, LCH b)
			{
				double[] rv = new double[3];
				rv[0] = a.l - b.l;
				rv[1] = a.c - b.c;
				rv[2] = RotSub(a.h, b.h);
				return rv;
			}
			public LCH Interpolate(LCH q, double alpha)
			{
				double beta = 1 - alpha;
				LCH rv = new LCH();
				rv.l = alpha * l + beta * q.l;
				rv.c = alpha * c + beta * q.c;
				rv.h = RotMul(alpha, h, beta, q.h);
				return rv;
			}
		}

		private static double RotMul(double alpha, double theta, double beta, double zeta)
		{
			double delta = theta - zeta;
			if(delta < -180)
			{
				theta += 360;
			}
			else if(delta > 180)
			{
				theta -= 360;
			}
			double iota = alpha * theta + beta * zeta;
			while(iota < 0)
			{
				iota += 360;
			}
			while(iota > 360)
			{
				iota -= 360;
			}
			return iota;
		}

		private static double RotSub(double a, double b)
		{
			double cw = a - b;
			if(cw < -180)
			{
				return cw + 360;
			}
			else if(cw > 180)
			{
				return cw - 360;
			}
			else
			{
				return cw;
			}
		}

        public readonly RGB rgb;
		public readonly HSL hsl;
		public readonly HWB hwb;
		public readonly XYZ xyz;
		public readonly LAB lab;
		public readonly LCH lch;
        public readonly string name;
		public readonly object spec;

		public string keyword
		{
			get
			{
				if(string.IsNullOrEmpty(name))
				{
					return string.Empty;
				}
				StringBuilder buf = new StringBuilder();
				for(int i = 0; i < name.Length; ++i)
				{
					char c = name[i];
					if(char.IsLetter(c))
					{
						buf.Append(c);
					}
				}
				return buf.ToString();
			}
		}

        private static double pn(string i, double q = 255.0)
        {
			if(i == "*")
			{
				return double.NaN;
			}
            else if (i.EndsWith("%"))
            {
                return double.Parse(i.TrimEnd('%')) / 100.0;
            }
			else if(i.EndsWith("°"))
			{
				return double.Parse(i.TrimEnd('°')) / 360.0;
            }
			else
			{
                return double.Parse(i) / q;
            }
        }
		private static int digits(double precision)
		{
			return (int)Math.Ceiling(Math.Abs(Math.Log(precision, 10)));
		}
		private static string ps(double d, int digits, double coef = 1, bool plus = false)
		{
			if(double.IsNaN(d))
			{
				return plus ? "±*" : "*";
			}
			else if(double.IsPositiveInfinity(d))
			{
				return "+∞";
			}
			else if(double.IsNegativeInfinity(d))
			{
				return "-∞";
			}
			else if(digits > 0)
			{
				if(plus)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0:\"+\"0." + new string('#', digits) + ";\"-\"0." + new string('#', digits) + "}", d * coef);
				}
				else
				{
					return string.Format(CultureInfo.InvariantCulture, "{0:0." + new string('#', digits) + "}", d * coef);
				}
			}
			else
			{
				if(plus)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0:\"+\"0;\"-\"0}", (int)Math.Round(coef * d));
				}
				else
				{
					return string.Format(CultureInfo.InvariantCulture, "{0}", (int)Math.Round(coef * d));
				}
			}
		}
		private static double pm(double d, double q)
		{
			if(double.IsNaN(d) || double.IsPositiveInfinity(d) || double.IsNegativeInfinity(d))
			{
				return d;
			}
			else
			{
				return d * q;
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

		private Colour(string name_, int r, int g, int b)
        {
            rgb = new RGB(r / 255.0, g / 255.0, b / 255.0);
			hsl = new HSL(rgb);
			hwb = new HWB(rgb);
			xyz = new XYZ(rgb);
			lab = new LAB(xyz);
			lch = new LCH(lab);
			name = name_ ?? Find(rgb);
			spec = rgb;
        }

        public override int GetHashCode()
        {
			return rgb.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Colour)
            {
                Colour x = (Colour)obj;
                return rgb.Equals(x.rgb);
            }
            else
            {
                return false;
            }
        }

        public Colour(object arg, string name_ = null)
        {
			if(arg == null)
			{
				rgb = new RGB();
				hsl = new HSL();
				hwb = new HWB();
				xyz = new XYZ();
				lab = new LAB();
				lch = new LCH();
				name = name_;
				spec = null;
				return;
			}
			else if(arg is RGB)
			{
				rgb = (RGB) arg;
				hsl = new HSL(rgb);
				hwb = new HWB(rgb);
				xyz = new XYZ(rgb);
				lab = new LAB(xyz);
				lch = new LCH(lab);
				name = name_ ?? Find(rgb);
				spec = rgb;
			}
			else if(arg is HSL)
			{
				hsl = (HSL) arg;
				rgb = hsl.ToRGB();
				hwb = new HWB(rgb);
				xyz = new XYZ(rgb);
				lab = new LAB(xyz);
				lch = new LCH(lab);
				name = name_ ?? Find(rgb);
				spec = hsl;
			}
			else if(arg is HWB)
			{
				hwb = (HWB) arg;
				rgb = hwb.ToRGB();
				hsl = new HSL(rgb);
				xyz = new XYZ(rgb);
				lab = new LAB(xyz);
				lch = new LCH(lab);
				name = name_ ?? Find(rgb);
				spec = hwb;
			}
			else if(arg is XYZ)
			{
				xyz = (XYZ) arg;
				rgb = xyz.ToRGB();
				lab = new LAB(xyz);
				lch = new LCH(lab);
				hsl = new HSL(rgb);
				hwb = new HWB(rgb);
				name = name_ ?? Find(rgb);
				spec = xyz;
			}
			else if(arg is LAB)
			{
				lab = (LAB) arg;
				lch = new LCH(lab);
				xyz = lab.ToXYZ();
				rgb = xyz.ToRGB();
				hsl = new HSL(rgb);
				hwb = new HWB(rgb);
				name = name_ ?? Find(rgb);
				spec = lab;
			}
			else if(arg is LCH)
			{
				lch = (LCH) arg;
				lab = lch.ToLAB();
				xyz = lab.ToXYZ();
				rgb = xyz.ToRGB();
				hsl = new HSL(rgb);
				hwb = new HWB(rgb);
				name = name_ ?? Find(rgb);
				spec = lch;
			}
			else
			{
				string input = arg.ToString();
				if(RGB.TryParse(input, out rgb))
				{
					hsl = new HSL(rgb);
					hwb = new HWB(rgb);
					xyz = new XYZ(rgb);
					lab = new LAB(xyz);
					lch = new LCH(lab);
					name = name_ ?? Find(rgb);
					spec = rgb;
				}
				else if(HSL.TryParse(input, out hsl))
				{
					rgb = hsl.ToRGB();
					hwb = new HWB(rgb);
					xyz = new XYZ(rgb);
					lab = new LAB(xyz);
					lch = new LCH(lab);
					name = name_ ?? Find(rgb);
					spec = hsl;
				}
				else if(HWB.TryParse(input, out hwb))
				{
					rgb = hwb.ToRGB();
					hsl = new HSL(rgb);
					xyz = new XYZ(rgb);
					lab = new LAB(xyz);
					lch = new LCH(lab);
					name = Find(rgb);
					spec = hwb;
				}
				else if(XYZ.TryParse(input, out xyz))
				{
					rgb = xyz.ToRGB();
					lab = new LAB(xyz);
					lch = new LCH(lab);
					hsl = new HSL(rgb);
					hwb = new HWB(rgb);
					name = name_ ?? Find(rgb);
					spec = xyz;
				}
				else if(LAB.TryParse(input, out lab))
				{
					lch = new LCH(lab);
					xyz = lab.ToXYZ();
					rgb = xyz.ToRGB();
					hsl = new HSL(rgb);
					hwb = new HWB(rgb);
					name = name_ ?? Find(rgb);
					spec = lab;
				}
				else if(LCH.TryParse(input, out lch))
				{
					lab = lch.ToLAB();
					xyz = lab.ToXYZ();
					rgb = xyz.ToRGB();
					hsl = new HSL(rgb);
					hwb = new HWB(rgb);
					name = name_ ?? Find(rgb);
					spec = lch;
				}
				else
				{
					string mre = input?.Replace("gray", "grey");
					foreach (Colour c in Standard)
					{
						if ((StringComparer.InvariantCultureIgnoreCase.Compare(mre, c.name) == 0) || (StringComparer.InvariantCultureIgnoreCase.Compare(mre, c.keyword) == 0))
						{
							rgb = c.rgb;
							hsl = c.hsl;
							hwb = c.hwb;
							xyz = c.xyz;
							lab = c.lab;
							lch = c.lch;
							name = name_ ?? c.name;
							spec = input ?? string.Empty;
							return;
						}
					}
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Not a valid colour: \"{0}\"", input));
				}
			}
        }

		private static string Find(RGB rgb)
		{
			foreach (Colour c in Standard)
			{
				if (c.rgb.Equals(rgb))
				{
					return c.name;
				}
			}
			return null;
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

		public Colour Interpolate(Colour q, double alpha = 0.5)
		{
			return Interpolate(this, q, alpha);
		}

		public static Colour Interpolate(Colour p, Colour q, double alpha = 0.5)
		{
			return Interpolate(p, q, alpha, p.lab);
		}

        public static Colour Interpolate(Colour p, Colour q, double alpha, object field)
        {
			if(field is LAB)
			{
				return new Colour(p.lab.Interpolate(q.lab, alpha), null);
			}
			else if(field is HWB)
			{
				return new Colour(p.hwb.Interpolate(q.hwb, alpha), null);
			}
			else if(field is LCH)
			{
				return new Colour(p.lch.Interpolate(q.lch, alpha), null);
			}
			else if(field is HSL)
			{
				return new Colour(p.hsl.Interpolate(q.hsl, alpha), null);
			}
			else if(field is XYZ)
			{
				return new Colour(p.xyz.Interpolate(q.xyz, alpha), null);
			}
			else
			{
				return new Colour(p.rgb.Interpolate(q.rgb, alpha), null);
			}
        }

		public Colour Approximate()
		{
			int index = Approximate(Standard, lab);
			return Standard[index];
		}

		public static int Approximate(Colour[] set, object field)
		{
			double D = double.PositiveInfinity;
			int I = 0;
			if(field is LAB)
			{
				LAB lab = (LAB) field;
				for(int i = 0; i < set.Length; ++i)
				{
					double d = lab - set[i].lab;
					if(d < D)
					{
						D = d;
						I = i;
					}
				}
			}
			else if(field is HWB)
			{
				HWB hwb = (HWB) field;
				for(int i = 0; i < set.Length; ++i)
				{
					double d = hwb - set[i].hwb;
					if(d < D)
					{
						D = d;
						I = i;
					}
				}
			}
			else if(field is LCH)
			{
				LCH lch = (LCH) field;
				for(int i = 0; i < set.Length; ++i)
				{
					double d = lch - set[i].lch;
					if(d < D)
					{
						D = d;
						I = i;
					}
				}
			}
			else if(field is HSL)
			{
				HSL hsl = (HSL) field;
				for(int i = 0; i < set.Length; ++i)
				{
					double d = hsl - set[i].hsl;
					if(d < D)
					{
						D = d;
						I = i;
					}
				}
			}
			else if(field is XYZ)
			{
				XYZ xyz = (XYZ) field;
				for(int i = 0; i < set.Length; ++i)
				{
					double d = xyz - set[i].xyz;
					if(d < D)
					{
						D = d;
						I = i;
					}
				}
			}
			else
			{
				RGB rgb = (RGB) field;
				for(int i = 0; i < set.Length; ++i)
				{
					double d = rgb - set[i].rgb;
					if(d < D)
					{
						D = d;
						I = i;
					}
				}
			}
			return I;
		}
//		delta U+394
//		prime U+2B9
		public static double operator-(Colour one, Colour two)
		{
			double ΔLʹ = two.lch.l - one.lch.l;
			double L_ = (one.lch.l + two.lch.l) / 2;
			double C_ = (one.lch.c + two.lch.c) / 2;
			double f = 1 - Math.Sqrt(Math.Pow(C_, 7) / (Math.Pow(C_, 7) + Math.Pow(25, 7)));
			double a1ʹ = one.lab.a + one.lab.a / 2 * f;
			double a2ʹ = two.lab.a + two.lab.a / 2 * f;
			double C1ʹ = Math.Sqrt(Math.Pow(a1ʹ, 2) + Math.Pow(one.lab.b, 2));
			double C2ʹ = Math.Sqrt(Math.Pow(a2ʹ, 2) + Math.Pow(two.lab.b, 2));
			double ΔCʹ = C2ʹ - C1ʹ;
			double C_ʹ = (C1ʹ + C2ʹ) / 2;
			double h1ʹ = Math.Atan2(one.lab.b, a1ʹ) * 180 / Math.PI;
			while(h1ʹ < 0)
			{
				h1ʹ += 360;
			}
			while(h1ʹ >= 360)
			{
				h1ʹ -= 360;
			}
			double h2ʹ = Math.Atan2(two.lab.b, a2ʹ) * 180 / Math.PI;
			while(h2ʹ < 0)
			{
				h2ʹ += 360;
			}
			while(h2ʹ >= 360)
			{
				h2ʹ -= 360;
			}
			f = Math.Abs(h1ʹ - h2ʹ);
			double Δhʹ;
			if(C1ʹ == 0 || C2ʹ == 0)
			{
				Δhʹ = 0;
			}
			else if(f <= 180)
			{
				Δhʹ = h2ʹ - h1ʹ;
			}
			else if(h2ʹ <= h1ʹ)
			{
				Δhʹ = h2ʹ - h1ʹ + 360;
			}
			else
			{
				Δhʹ = h2ʹ - h1ʹ - 360;
			}
			double ΔHʹ = 2 * Math.Sqrt(C1ʹ * C2ʹ) * Math.Sin(Δhʹ * Math.PI / 180 / 2);
			double H_ʹ;
			if(C1ʹ == 0 || C2ʹ == 0)
			{
				H_ʹ = h1ʹ + h2ʹ;
			}
			else if(f <= 180)
			{
				H_ʹ = (h1ʹ + h2ʹ) / 2;
			}
			else if(h1ʹ + h2ʹ < 360)
			{
				H_ʹ = (h1ʹ + h2ʹ + 360) / 2;
			}
			else
			{
				H_ʹ = (h1ʹ + h2ʹ - 360) / 2;
			}
			double T = 1 - 0.17 * Math.Cos((H_ʹ - 30) / 180 * Math.PI) + 0.24 * Math.Cos((2 * H_ʹ) / 180 * Math.PI) + 0.32 * Math.Cos((3 * H_ʹ + 6) / 180 * Math.PI) - 0.20 * Math.Cos((4 * H_ʹ -63) / 180 * Math.PI);
			double SL = 1 + ((0.015 * Math.Pow(L_ - 50, 2)) / Math.Sqrt(20 + Math.Pow(L_ - 50, 2)));
			double SC = 1 + 0.045 * C_ʹ;
			double SH = 1 + 0.015 * C_ʹ * T;
			double RT = -2 * Math.Sqrt(Math.Pow(C_ʹ, 7) / (Math.Pow(C_ʹ, 7) + Math.Pow(25, 7))) * Math.Sin(60 * Math.Exp(-1 * Math.Pow((H_ʹ - 275) / 25, 2)) / 180 * Math.PI);
			double ΔE00 = Math.Sqrt( Math.Pow(ΔLʹ / SL, 2) + Math.Pow(ΔCʹ / SC, 2) + Math.Pow(ΔHʹ / SH, 2) + RT * (ΔCʹ / SC) * (ΔHʹ / SH));
			return ΔE00;
		}

#if COLOUR_INCLUDE_MAIN
	static string pad(string s, int width = 36)
	{
		if(s.Length < width)
		{
			return s + new string(' ', width - s.Length);
		}
		else
		{
			return s;
		}
	}
	static string format(Colour c, object spec, string TAB = "    ", string NL = "")
	{
		RGB? rgbspec = null;
		XYZ? xyzspec = null;
		LAB? labspec = null;
		LCH? lchspec = null;
		if(spec is LCH)
		{
			lchspec = (LCH) spec;
			labspec = lchspec.Value.ToLAB();
			xyzspec = labspec.Value.ToXYZ();
			rgbspec = xyzspec.Value.ToRGB();
		}
		else if(spec is LAB)
		{
			labspec = (LAB) spec;
			lchspec = new LCH(labspec.Value);
			xyzspec = labspec.Value.ToXYZ();
			rgbspec = xyzspec.Value.ToRGB();
		}
		else if(spec is XYZ)
		{
			xyzspec = (XYZ) spec;
			labspec = new LAB(xyzspec.Value);
			lchspec = new LCH(labspec.Value);
			rgbspec = xyzspec.Value.ToRGB();
		}
		else if(spec is RGB)
		{
			rgbspec = (RGB) spec;
			xyzspec = new XYZ(rgbspec.Value);
			labspec = new LAB(xyzspec.Value);
			lchspec = new LCH(labspec.Value);
		}
		StringBuilder buf = new StringBuilder();
		buf.Append(TAB);
		buf.Append(c.rgb.ToHexString());
		buf.Append(NL);
		buf.Append(TAB);
		buf.Append(c.rgb.ToDecimalString());
		buf.Append(NL);
		buf.Append(TAB);
		buf.Append(c.hsl.ToString());
		buf.Append(NL);
		buf.Append(TAB);
		buf.Append(c.hwb.ToString());
		buf.Append(NL);
		buf.Append(TAB);
		if(rgbspec.HasValue)
		{
			buf.Append(pad(c.rgb.ToPercentageString()));
			double[] d = c.rgb % rgbspec.Value;
			double D = c.rgb - rgbspec.Value;
			buf.Append(TAB);
			buf.Append(pad(string.Format(CultureInfo.InvariantCulture,
				"[R{0}%,G{1}%,B{2}%]",
				ps(d[0], digits(RGB.R_PRECISION * 100) + 1, 100, true), ps(d[1], digits(RGB.G_PRECISION * 100) + 1, 100, true), ps(d[2], digits(RGB.B_PRECISION * 100) + 1, 100, true)
			)));
			buf.Append(TAB);
			buf.Append(string.Format(CultureInfo.InvariantCulture,
				"±{0}",
				ps(D, 3)
			));
		}
		else
		{
			buf.Append(c.rgb.ToPercentageString());
		}
		buf.Append(NL);
		buf.Append(TAB);
		if(xyzspec.HasValue)
		{
			buf.Append(pad(c.xyz.ToString()));
			buf.Append(TAB);
			double[] d = c.xyz % xyzspec.Value;
			buf.Append(string.Format(CultureInfo.InvariantCulture,
				"[X{0},Y{1},Z{2}]",
				ps(d[0], digits(XYZ.X_PRECISION) + 1, 1, true), ps(d[1], digits(XYZ.Y_PRECISION) + 1, 1, true), ps(d[2], digits(XYZ.Y_PRECISION) + 1, 1, true)
			));
		}
		else
		{
			buf.Append(c.xyz.ToString());
		}
		buf.Append(NL);
		buf.Append(TAB);
		if(labspec.HasValue)
		{
			buf.Append(pad(c.lab.ToString()));
			double[] d = c.lab % labspec.Value;
			double D = c.lab - labspec.Value;
			buf.Append(TAB);
			buf.Append(pad(string.Format(CultureInfo.InvariantCulture,
				"[L*{0},a*{1},b*{2}]",
				ps(d[0], digits(LAB.L_PRECISION) + 1, 1, true), ps(d[1], digits(LAB.A_PRECISION) + 1, 1, true), ps(d[2], digits(LAB.B_PRECISION) + 1, 1, true)
			)));
			buf.Append(TAB);
			buf.Append(string.Format(CultureInfo.InvariantCulture,
				"±{0}",
				ps(D, 3)
			));
		}
		else
		{
			buf.Append(c.lab.ToString());
		}
		buf.Append(NL);
		buf.Append(TAB);
		if(lchspec.HasValue)
		{
			buf.Append(pad(c.lch.ToString()));
			buf.Append(TAB);
			double[] d = c.lch % lchspec.Value;
			buf.Append(pad(string.Format(CultureInfo.InvariantCulture,
				"[L*{0},C*{1},h°{2}]",
				ps(d[0], digits(LCH.L_PRECISION) + 1, 1, true), ps(d[1], digits(LCH.C_PRECISION) + 1, 1, true), ps(d[2], digits(LCH.H_PRECISION) + 1, 1, true)
			)));
		}
		else
		{
			buf.Append(c.lch.ToString());
		}
		if(!string.IsNullOrWhiteSpace(c.name))
		{
			buf.Append(NL);
			buf.Append(TAB);
			buf.Append(c.keyword);
		}
		else
		{
			buf.Append(NL);
			buf.Append(TAB);
			Colour a = c.Approximate();
			double[] d = a.lch % c.lch;
			double D = a.lab - c.lab;
			buf.Append(pad(string.Format(CultureInfo.InvariantCulture,
				"≅ {0} ({1})",
				a.keyword,
				a.rgb.ToString()
			)));
			buf.Append(TAB);
			buf.Append(pad(string.Format(CultureInfo.InvariantCulture,
				"[L*{0},C*{1},h°{2}]",
				ps(d[0], digits(LCH.L_PRECISION) + 1, 1, true), ps(d[1], digits(LCH.C_PRECISION) + 1, 1, true), ps(d[2], digits(LCH.H_PRECISION) + 1, 1, true)
			)));
			buf.Append(TAB);
			buf.Append(string.Format(CultureInfo.InvariantCulture,
				"±{0}",
				ps(D, 3)
			));
		}
		return buf.ToString();
	}
	static void print(string a, Colour c, object spec, string TAB = "    ", string NL1 = "\n", string NL2 = "\n")
	{
		Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}:{1}{2}", a, NL1, format(c, spec, TAB, NL2)));
	}
	static readonly string[] Unclipped = new string[]{ "/unclip", "/unclipped", "--unclip", "--unclipped", "-u", "/u" };
	static readonly string[] Fixed = new string[]{ "/fix", "/fixed", "--fix", "--fixed", "-f", "/f" };
	static bool Matches(string a, string[] args)
	{
		for(int i = 0; i < args.Length; ++i)
		{
			if(StringComparer.OrdinalIgnoreCase.Compare(a, args[i]) == 0)
			{
				return true;
			}
		}
		return false;
	}
	static Colour rectify(Colour c, bool clip, bool fix)
	{
		if(!c.rgb.IsValid)
		{
			if(fix)
			{
				return new Colour(c.rgb.Fix());
			}
			else if(clip)
			{
				return new Colour(c.rgb.Clip());
			}
		}
		return c;
	}
    static void Main(string[] args)
    {
        Colour? current = null;
        double q = double.NaN;
        char op = '\0';
        string pop = null;
		bool clip = true, fix = false;
        foreach(string a in args)
        {
			if(string.IsNullOrWhiteSpace(a))
			{
				continue;
			}
            else if (a[0] == '+')
            {
                op = '+';
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
			else if(a[0] == '/' || a[0] == '-')
			{
				if(Matches(a, Unclipped))
				{
					clip = false;
				}
				else if(Matches(a, Fixed))
				{
					fix = true;
				}
			}
            else
            {
				try
				{
					Colour cs = new Colour(a, null);
	                Colour c = rectify(cs, clip, fix);
					print(a, c, cs.rgb.IsValid ? null : cs.spec);
	                bool pc = true;
	                if (op == '+')
	                {
						cs = Colour.Interpolate(current.Value, c, q);
	                    current = rectify(cs, clip, fix);
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
						print(pop, current.Value, cs.rgb.IsValid ? null : cs.spec);
	                }
				}
				catch(Exception e)
				{
					Console.Error.WriteLine(e.Message);
				}
            }
        }
	}
#endif
}
