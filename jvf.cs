using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Validates JFIF (JPEG) file integrity.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

class jvf {

static bool ValidateJFIF(Stream fs) {
	byte[] buf = new byte[2];
	bool eoi = false;
	bool loop = true;
	long offset = 0;
	while(loop) {
		int n = fs.Read(buf, 0, 2);
		if(n < 2) {
			loop = false;
		} else {
			int marker = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buf, 0)) & 0xFFFF;
			bool sized = true;
			bool scan = false;
			string name = "unknown";
			switch(marker) {
			default:
				if(marker >= 0xFFD0 && marker <= 0xFFD7) {
					name = string.Format(CultureInfo.InvariantCulture, "RST{0}*", marker - 0xFFD0);
					scan = true;
					sized = false;
				} else if(marker >= 0xFFE0 && marker <= 0xFFEF) {
					name = string.Format(CultureInfo.InvariantCulture, "APP{0}", marker - 0xFFE0);
				} else if(marker >= 0xFFF0 && marker <= 0xFFFD) {
					name = string.Format(CultureInfo.InvariantCulture, "JPG{0}", marker - 0xFFF0);
				} else if(marker >= 0xFF02 && marker <= 0xFFBF) {
					name = "RES";
				} else {
					Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "at {0:X8}: bad marker: 0x{1:X4}", offset, marker));
					return false;
				}
				break;
			case 0xFF01:
				name = "TEM*";
				sized = false;
				break;
			case 0xFFC0:
				name = "SOF0";
				break;
			case 0xFFC1:
				name = "SOF1";
				break;
			case 0xFFC2:
				name = "SOF2";
				break;
			case 0xFFC3:
				name = "SOF3";
				break;
			case 0xFFC4:
				name = "DHT";
				break;
			case 0xFFC5:
				name = "SOF5";
				break;
			case 0xFFC6:
				name = "SOF6";
				break;
			case 0xFFC7:
				name = "SOF7";
				break;
			case 0xFFC8:
				name = "JPG";
				break;
			case 0xFFC9:
				name = "SOF9";
				break;
			case 0xFFCA:
				name = "SOF10";
				break;
			case 0xFFCB:
				name = "SOF11";
				break;
			case 0xFFCC:
				name = "DAC";
				break;
			case 0xFFCD:
				name = "SOF13";
				break;
			case 0xFFCE:
				name = "SOF14";
				break;
			case 0xFFCF:
				name = "SOF15";
				break;
			case 0xFFD8:
				name = "SOI*";
				sized = false;
				break;
			case 0xFFD9:
				name = "EOI*";
				eoi = true;
				sized = false;
				break;
			case 0xFFDA:
				name = "SOS";
				scan = true;
				break;
			case 0xFFDB:
				name = "DQT";
				break;
			case 0xFFDC:
				name = "DNL";
				break;
			case 0xFFDD:
				name = "DRI";
				break;
			case 0xFFDE:
				name = "DHP";
				break;
			case 0xFFDF:
				name = "EXP";
				break;
			case 0xFFFE:
				name = "COM";
				break;
			}
			if(sized) {
				n = fs.Read(buf, 0, 2);
				if(n < 2) {
					loop = false;
				} else {
					int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buf, 0)) & 0xFFFF;
					Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "at {0:X8}: segment marker: 0x{1:X4} ({2}), length: {3} bytes", offset, marker, name, length));
					if(length > 2) {
						fs.Seek(length - 2, SeekOrigin.Current);
					}
					offset += 2 + length;
				}
			} else {
				Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "at {0:X8}: marker: 0x{1:X4} ({2})", offset, marker, name));
				offset += 2;
			}
			while(scan) {
				int b = fs.ReadByte();
				if(b == 0xFF) {
					b = fs.ReadByte();
					if(b > 0) {
						Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "at {0:X8}: end of scan reached", offset));
						fs.Seek(-2, SeekOrigin.Current);
						scan = false;
					} else if(b == 0) {
						offset += 2;
					}
				} else if (b >= 0) {
					++offset;
				}
				if(b < 0) {
					loop = false;
					scan = false;
				}
			}
		}
	}
	if(!eoi) {
		Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "at {0:X8}: unexpected end of file reached", offset));
	}
	return eoi;
}

static bool ValidateJFIF(string file) {
	using(FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read)) {
		return ValidateJFIF(fs);
	}
}

static bool ValidateJFIF(string a, bool dir) {
	if(!Path.GetFileName(a).StartsWith(".")) {
		if(dir) {
			bool notok = false;
			foreach(string f in Directory.GetFiles(a, "*.jpg")) {
				bool ok = ValidateJFIF(f, false);
				notok = notok || !ok;
			}
			foreach(string f in Directory.GetFiles(a, "*.jpeg")) {
				bool ok = ValidateJFIF(f, false);
				notok = notok || !ok;
			}
			foreach(string d in Directory.GetDirectories(a)) {
				bool ok = ValidateJFIF(d, true);
				notok = notok || !ok;
			}
			return !notok;
		} else {
			bool ok = ValidateJFIF(a);
			string msg = string.Format(CultureInfo.InvariantCulture, "{0}: {1}ok", a, ok ? string.Empty : "not ");
			TextWriter tw = ok ? Console.Out : Console.Error;
			tw.WriteLine(msg);
			return ok;
		}
	} else {
		return true;
	}
}

static bool matches(string a, params string[] m) {
	foreach(string n in m) {
		if(StringComparer.OrdinalIgnoreCase.Compare(a,n) == 0) {
			return true;
		}
	}
	return false;
}

public static int Main(string[] args) {
	bool notok = false;
	bool didit = false;
	bool syntax = false;
	foreach(string a in args) {
		if(matches(a, "--help", "/help", "-h", "/?")) {
			syntax = true;
		} else {
			bool ok = ValidateJFIF(Path.GetFullPath(a), Directory.Exists(a));
			notok = notok || !ok;
			didit = true;
		}
	}
	if(syntax || !didit) {
		Console.WriteLine("Validates JFIF (JPEG) files:");
		Console.WriteLine("intact file names printed to stdout, invalid ones to stderr.");
		Console.WriteLine("Exit codes:");
		Console.WriteLine(" 0 = all files OK");
		Console.WriteLine(" 1 = at least one file is NOT OK");
		Console.WriteLine("Syntax: jvf jpg|dir...");
	}
	return notok ? 1 : 0;
}

}
