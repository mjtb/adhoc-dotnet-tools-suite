using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Creates an icon file from 32-bpp .png files.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

namespace mkres
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
		}
		public void Write(BinaryWriter bw)
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
		}
	}
	struct NewHeader
	{
		public const short ICON_RESTYPE = 1;
		public const short CURSOR_RESTYPE = 2;
		public short Reserved;
		public short ResType;
		public short ResCount;
		public NewHeader(int rescount, short restype)
		{
			this.Reserved = 0;
			this.ResType = restype;
			this.ResCount = (short)rescount;
		}
		public void Write(BinaryWriter bw)
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
		public void Write(BinaryWriter bw)
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
			this.BytesInRes =  40 + ((((((width + 7) >> 3) + 3) >> 2) << 2) + (width * 4)) * height;
			this.Offset = 0;
		}
		public ResDir(int width, int height, short xHotspot, short yHotspot)
		{
			this.Icon = new IconResDir(width, height);
			this.Planes = xHotspot;
			this.BitCount = yHotspot;
			this.BytesInRes =  40 + ((((((width + 7) >> 3) + 3) >> 2) << 2) + (width * 4)) * height;
			this.Offset = 0;
		}
		public void Write(BinaryWriter bw)
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
		static unsafe byte[] GetBitmapBits(Bitmap bm)
		{
			BitmapData bd = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			byte* bi  = ((byte*) ((void*) bd.Scan0));
			byte[] bo = new byte[bd.Stride * bd.Height];
			for(int y = 0; y < bd.Height; y++)
			{
				int i = (bd.Height - y - 1) * bd.Stride;
				int o = y * bd.Stride;
				for(int x = bd.Stride - 1; x >= 0; x--)
				{
					bo[o + x] = bi[i + x];
				}
			}
			bm.UnlockBits(bd);
			return bo;
		}
		public static void Main(string[] args)
		{
			if(args.Length < 2)
			{
				Console.WriteLine("Creates an icon or cursor file from 32-bpp .png files.\nsyntax: mkicon output.(ico|.cur) ((--hotspot=x,y)? file.png)...");
				return;
			}
			short xHotspot = 0;
			short yHotspot = 0;
			List<Tuple<short,short,Bitmap>> inputs = new List<Tuple<short,short,Bitmap>>();
			for(int i = 1; i < args.Length; i++)
			{
				if(args[i].StartsWith("--hotspot=", StringComparison.InvariantCultureIgnoreCase))
				{
					int comma = args[i].IndexOf(',');
					xHotspot = short.Parse(args[i].Substring(10,comma-10));
					yHotspot = short.Parse(args[i].Substring(comma+1));
				}
				else
				{
					try
					{
						inputs.Add(new Tuple<short,short,Bitmap>(xHotspot,yHotspot,((Bitmap) Bitmap.FromFile(args[i]))));
					}
					catch(System.IO.FileNotFoundException)
					{
						Console.WriteLine("error: file not found: " + args[i]);
						return;
					}
				}
			}
			bool cur = args[0].EndsWith(".cur", StringComparison.InvariantCultureIgnoreCase);
			using(BinaryWriter bw = new BinaryWriter(new System.IO.FileStream(args[0], System.IO.FileMode.Create, System.IO.FileAccess.Write)))
			{
				int length = inputs.Count;
				NewHeader nh = new NewHeader(length, cur ? NewHeader.CURSOR_RESTYPE : NewHeader.ICON_RESTYPE);
				ResDir[] rd = new ResDir[length];
				BitmapInfoHeader[] bmih = new BitmapInfoHeader[length];
				byte[][] bits = new byte[length][];
				byte[][] masks = new byte[length][];
				for(int i = 0; i < length; i++)
				{
					Bitmap bm = inputs[i].Item3;
					int stride = 4 * bm.Width;
					int maskStride = ((((bm.Width + 7) >> 3) + 3) >> 2) << 2;
					if(cur)
					{
						rd[i] = new ResDir(bm.Width, bm.Height, inputs[i].Item1, inputs[i].Item2);
					}
					else
					{
						rd[i] = new ResDir(bm.Width, bm.Height);
					}
					bmih[i] = new BitmapInfoHeader(bm.Width, bm.Height);
					bits[i] = GetBitmapBits(bm);
					masks[i] = new byte[maskStride * bm.Height];
					for(int y = bm.Height - 1; y >= 0; y--)
					{
						int ofs = y * stride;
						int maskOfs = y * maskStride;
						for(int x = bm.Width - 1; x >= 0; x--, ofs += 4)
						{
							if(bits[i][ofs + 3] < 128)
							{
								bits[i][ofs + 3] = 0;
								bits[i][ofs + 2] = 0;
								bits[i][ofs + 1] = 0;
								bits[i][ofs] = 0;
								int x_ = bm.Width - 1 - x;
								masks[i][maskOfs + (x_ >> 3)] |= ((byte) (0x80 >> (x_ & 7)));
							}
						}
					}
				}
				rd[0].Offset = 6 + 16 * length;
				for(int i = 1; i < length; i++)
				{
					rd[i].Offset = rd[i - 1].Offset + rd[i - 1].BytesInRes;
				}
				nh.Write(bw);
				for(int i = 0; i < length; i++)
				{
					rd[i].Write(bw);
				}
				for(int i = 0; i < length; i++)
				{
					bmih[i].Write(bw);
					bw.Write(bits[i]);
					bw.Write(masks[i]);
				}
			}
		}
	}
}
