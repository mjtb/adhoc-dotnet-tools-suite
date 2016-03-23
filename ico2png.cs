using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Creates an series of PNG files from a *.ico file.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

namespace ico2png
{
	struct BitmapInfoHeader
	{
		public int Size;
		public int Width;
		public int Height;
		public short Planes;
		public short BitCount;
		public int Compression;
		public int SizeImage;
		public int XPelsPerMeter;
		public int YPelsPerMeter;
		public int ClrUsed;
		public int ClrImportant;
		public int[] Colors;
		public void Read(System.IO.BinaryReader br)
		{
			this.Size = br.ReadInt32();
			this.Width = br.ReadInt32();
			this.Height = br.ReadInt32();
			this.Planes = br.ReadInt16();
			this.BitCount = br.ReadInt16();
			this.Compression = br.ReadInt32();
			this.SizeImage = br.ReadInt32();
			this.XPelsPerMeter = br.ReadInt32();
			this.YPelsPerMeter = br.ReadInt32();
			this.ClrUsed = br.ReadInt32();
			this.ClrImportant = br.ReadInt32();
			if((this.ClrUsed > 0) || (this.BitCount < 16))
			{
				int colors = 0;
				if(this.BitCount < 16)
				{
					colors = 1 << this.BitCount;
				}
				else
				{
					colors = this.ClrUsed;
				}
				this.Colors = new int[colors];
				byte[] buf = new byte[4 * colors];
				br.Read(buf, 0, buf.Length);
				System.Buffer.BlockCopy(buf, 0, this.Colors, 0, buf.Length);
			}
			else
			{
				this.Colors = new int[0];
			}
		}
		public BitmapInfoHeader(int width, int height)
		{
			this.Size = 40;
			this.Width = width;
			this.Height = 2 * height;
			this.Planes = 1;
			this.BitCount = 32;
			this.Compression = 0;
			this.SizeImage = 0; //width * 4 * height;
			this.XPelsPerMeter = 0;
			this.YPelsPerMeter = 0;
			this.ClrUsed = 0;
			this.ClrImportant = 0;
			this.Colors = new int[0];
		}
		public void Write(System.IO.BinaryWriter bw)
		{
			bw.Write(this.Size);
			bw.Write(this.Width);
			bw.Write(this.Height);
			bw.Write(this.Planes);
			bw.Write(this.BitCount);
			bw.Write(this.Compression);
			bw.Write(this.SizeImage);
			bw.Write(this.XPelsPerMeter);
			bw.Write(this.YPelsPerMeter);
			bw.Write(this.ClrUsed);
			bw.Write(this.ClrImportant);
			if((this.ClrUsed > 0) || (this.BitCount < 16))
			{
				int colors = 0;
				if(this.BitCount < 16)
				{
					colors = 1 << this.BitCount;
				}
				else
				{
					colors = this.ClrUsed;
				}
				if(colors > this.Colors.Length)
				{
					throw new ApplicationException(string.Format("BitmapInfoHeader.Colors does not specify enough colors; expected {0}, found {1}.", colors, this.Colors.Length));
				}
				byte[] buf = new byte[4 * colors];
				System.Buffer.BlockCopy(this.Colors, 0, buf, 0, buf.Length);
				bw.Write(buf, 0, buf.Length);
			}
		}
	}
	struct NewHeader
	{
		public short Reserved;
		public short ResType;
		public short ResCount;
		public NewHeader(int rescount)
		{
			this.Reserved = 0;
			this.ResType = 1;
			this.ResCount = ((short) rescount);
		}
		public void Read(System.IO.BinaryReader br)
		{
			this.Reserved = br.ReadInt16();
			this.ResType = br.ReadInt16();
			this.ResCount = br.ReadInt16();
		}
		public void Write(System.IO.BinaryWriter bw)
		{
			bw.Write(this.Reserved);
			bw.Write(this.ResType);
			bw.Write(this.ResCount);
		}
	}
	struct IconResDir
	{
		public byte Width;
		public byte Height;
		public byte ColorCount;
		public byte reserved;
		public IconResDir(int width, int height)
		{
			this.Width = ((byte) width);
			this.Height = ((byte) height);
			this.ColorCount = 0;
			this.reserved = 0;
		}
		public void Read(System.IO.BinaryReader br)
		{
			this.Width = br.ReadByte();
			this.Height = br.ReadByte();
			this.ColorCount = br.ReadByte();
			this.reserved = br.ReadByte();
		}
		public void Write(System.IO.BinaryWriter bw)
		{
			bw.Write(this.Width);
			bw.Write(this.Height);
			bw.Write(this.ColorCount);
			bw.Write(this.reserved);
		}
	}
	struct ResDir
	{
		public IconResDir Icon;
		public short Planes;
		public short BitCount;
		public int BytesInRes;
		public int Offset;
		public ResDir(int width, int height)
		{
			this.Icon = new IconResDir(width, height);
			this.Planes = 1;
			this.BitCount = 32;
			this.BytesInRes = 40 + ((((((width + 7) >> 3) + 3) >> 2) << 2) + (width * 4)) * height;
			this.Offset = 0;
		}
		public void Read(System.IO.BinaryReader br)
		{
			this.Icon.Read(br);
			this.Planes = br.ReadInt16();
			this.BitCount = br.ReadInt16();
			this.BytesInRes = br.ReadInt32();
			this.Offset = br.ReadInt32();
		}
		public void Write(System.IO.BinaryWriter bw)
		{
			this.Icon.Write(bw);
			bw.Write(this.Planes);
			bw.Write(this.BitCount);
			bw.Write(this.BytesInRes);
			bw.Write(this.Offset);
		}
	}
	class MainClass
	{
		public static void Expand1bpp(byte[] bits, int ofs, int[] colors, int width, int height, int stride, int[] cmap)
		{
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					cmap[y * width + x] = colors[(((bits[ofs + y * stride + (x >> 3)] & (0x80 >> (x & 7))) != 0) ? 1 : 0)];
				}
			}
		}
		public static void Expand4bpp(byte[] bits, int ofs, int[] colors, int width, int height, int stride, int[] cmap)
		{
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int i = bits[ofs + y * stride + (x >> 1)];
					if(0 == (x & 1))
					{
						i >>= 4;
					}
					i &= 15;
					cmap[y * width + x] = colors[i];
				}
			}
		}
		public static void Expand8bpp(byte[] bits, int ofs, int[] colors, int width, int height, int stride, int[] cmap)
		{
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					cmap[y * width + x] = colors[bits[ofs + y * stride + x]];
				}
			}
		}
		public static void Expand16bpp555(byte[] bits, int ofs, int width, int height, int stride, int[] cmap)
		{
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int c = BitConverter.ToUInt16(bits, ofs + y * stride + x * 2);
					cmap[y * width + x] = (((c << 2) & 0xf8) | ((c << 5) & 0xf800) | ((c << 8) & 0xf80000));
				}
			}
		}
		public static void Expand24bpp(byte[] bits, int ofs, int width, int height, int stride, int[] cmap)
		{
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int i = ofs + y * stride + x * 3;
					cmap[y * width + x] = (bits[i] | (bits[i + 1] << 8) | (bits[i + 2] << 16));
				}
			}
		}
		public static void Expand32bpp(byte[] bits, int ofs, int width, int height, int stride, int[] cmap)
		{
			System.Buffer.BlockCopy(bits, ofs, cmap, 0, width * 4 * height);
		}
		public static void Mask(byte[] bits, int ofs, int width, int height, int[] cmap)
		{
			int stride = ((width + 31) >> 5) << 2;
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int i = y * width + x;
					if(0 == (bits[ofs + y * stride + (x >> 3)] & (0x80 >> (x & 7))))
					{
						cmap[i] |= -16777216;
					}
					else
					{
						cmap[i] = 0;
					}
				}
			}
		}
		public static System.Drawing.Bitmap Decode(byte[] bits, int offset, int count, out int width, out int height, out int bitcount)
		{
			System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.MemoryStream(bits, offset, count));
			BitmapInfoHeader bmih = new BitmapInfoHeader();
			bmih.Read(br);
			width = bmih.Width;
			height = bmih.Height / 2;
			bitcount = bmih.BitCount;
			int[] cmap = new int[width * 4 * height];
			int stride = ((width * bmih.BitCount + 31) >> 5) << 2;
			switch(bmih.BitCount)
			{
				case 1:
					Expand1bpp(bits, offset + bmih.Size + 4 * bmih.Colors.Length, bmih.Colors, width, height, stride, cmap);
					break;
				case 4:
					Expand4bpp(bits, offset + bmih.Size + 4 * bmih.Colors.Length, bmih.Colors, width, height, stride, cmap);
					break;
				case 8:
					Expand8bpp(bits, offset + bmih.Size + 4 * bmih.Colors.Length, bmih.Colors, width, height, stride, cmap);
					break;
				case 16:
					Expand16bpp555(bits, offset + bmih.Size + 4 * bmih.Colors.Length, width, height, stride, cmap);
					break;
				case 24:
					Expand24bpp(bits, offset + bmih.Size, width, height, stride, cmap);
					break;
				case 32:
					Expand32bpp(bits, offset + bmih.Size, width, height, stride, cmap);
					break;
			}
			if(bitcount < 32)
			{
				Mask(bits, offset + bmih.Size + 4 * bmih.Colors.Length + stride * height, width, height, cmap);
			}
			byte[] cmapbits = new byte[4 * width * height];
			FlipTo(cmap, width, height, cmapbits);
			System.Drawing.Bitmap bm = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Drawing.Imaging.BitmapData bd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Runtime.InteropServices.Marshal.Copy(cmapbits, 0, bd.Scan0, cmapbits.Length);
			bm.UnlockBits(bd);
			return bm;
		}
		private static void FlipTo(int[] cmap, int width, int height, byte[] cmapbits)
		{
			for(int y = 0; y < height; y++)
			{
				System.Buffer.BlockCopy(cmap, y * width * 4, cmapbits, (height - y - 1) * width * 4, width * 4);
			}
		}
		[STAThread]
		public static void Main(string[] args)
		{
			if((args.Length == 0) || (args[0] == "/?") || (args[0] == "-?"))
			{
				Console.WriteLine("Extracts PNG images from an .ico file.");
				Console.WriteLine("syntax: png2ico files...");
				Console.WriteLine();
				Console.WriteLine("Can specify multiple files or wildcards.");
				Console.WriteLine();
				return;
			}
			for(int i = 0; i < args.Length; i++)
			{
				string dirname = ".", pathpart = args[i];
				if(args[i].IndexOf('\\') != -1)
				{
					dirname = Path.GetDirectoryName(args[i]);
					pathpart = Path.GetFileName(args[i]);
				}
				bool found = false;
				foreach(string icofile in Directory.GetFiles(dirname, pathpart))
				{
					found = true;
					string fname = icofile;//Path.Combine(dirname, icofile);
					Console.WriteLine(string.Format("Converting {0}:", fname));
					try
					{
						Convert(fname);
					}
					catch(Exception e)
					{
						Console.Error.WriteLine(e.ToString());
					}
				}
				if(!found)
				{
					Console.Error.WriteLine(string.Format("{0}: file not found", args[i]));
				}
				Console.WriteLine();
			}
		}
		public static void Convert(string icofile)
		{
			System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.FileStream(icofile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read));
			NewHeader nh = new NewHeader();
			nh.Read(br);
			ResDir[] rd = new ResDir[nh.ResCount];
			for(int i = 0; i < nh.ResCount; i++)
			{
				rd[i].Read(br);
			}
			byte[] bits = new byte[0];
			for(int i = 0; i < nh.ResCount; i++)
			{
				br.BaseStream.Seek(rd[i].Offset, System.IO.SeekOrigin.Begin);
				if(bits.Length < rd[i].BytesInRes)
				{
					bits = new byte[rd[i].BytesInRes];
				}
				br.Read(bits, 0, rd[i].BytesInRes);
				int bitcount, width, height;
				System.Drawing.Bitmap bm = Decode(bits, 0, rd[i].BytesInRes, out width, out height, out bitcount);
				string fileName = string.Format("{0}-{1}x{2}-{3}bpp.png", System.IO.Path.GetFileNameWithoutExtension(icofile), width, height, bitcount);
				bm.Save(fileName);
				bm.Dispose();
				Console.WriteLine(string.Format("  Saved {0}", fileName));
			}
			br.Close();
		}
	}
}
