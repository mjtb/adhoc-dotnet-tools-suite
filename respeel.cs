using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Extracts bitmap and icon Win32 resources from executables and dynamic link library files.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

namespace respeel
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
	}
	[StructLayout(LayoutKind.Sequential, Pack=2)]
	struct BitmapFileHeader
	{
		public short bfType;
		public int bfSize;
		public short bfReserved1;
		public short bfReserved2;
		public int bfOffBits;
	}
	struct NewHeader
	{
		public short Reserved;
		public short ResType;
		public short ResCount;
	}
	struct IconResDir
	{
		public byte Width;
		public byte Height;
		public byte ColorCount;
		public byte reserved;
	}
	struct ResDir
	{
		public IconResDir Icon;
		public short Planes;
		public short BitCount;
		public int BytesInRes;
		public int Offset;
	}
	class MainClass
	{
		[Flags]
		enum LoadLibraryFlags
		{
			LOAD_LIBRARY_AS_DATAFILE = 2,
		}
		delegate bool ResNameEnumProc(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam);
		delegate bool ResTypeEnumProc(IntPtr hModule, IntPtr lpszType, IntPtr lParam);
		[DllImport("kernel32", SetLastError=true)]
		static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);
		[DllImport("kernel32", SetLastError=true)]
		static extern IntPtr LoadResource(IntPtr hModule, IntPtr hRsrc);
		[DllImport("kernel32", SetLastError=true)]
		static extern IntPtr LockResource(IntPtr hGlobal);
		[DllImport("kernel32", SetLastError=true)]
		static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, LoadLibraryFlags dwFlags);
		[DllImport("kernel32", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool FreeLibrary(IntPtr hModule);
		[DllImport("kernel32", SetLastError=true)]
		static extern int SizeofResource(IntPtr hModule, IntPtr hRsrc);
		[DllImport("kernel32", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool EnumResourceNames(IntPtr hModule, IntPtr lpszType, ResNameEnumProc lpEnumFunc, IntPtr lParam);
		[DllImport("kernel32", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool EnumResourceTypes(IntPtr hModule, ResTypeEnumProc lpEnumFunc, IntPtr lParam);
		class ResourceTypes
		{
			public static IntPtr RT_BITMAP
			{
				get
				{
					return new IntPtr(2);
				}
			}
			public static IntPtr RT_ICON
			{
				get
				{
					return new IntPtr(3);
				}
			}
		}
		static string baseName;
		static bool IS_INTRESOURCE(IntPtr lpszName)
		{
			return (0 == (((int) lpszName) & 0x7FFF0000));
		}
		static string GetOutputFileName(IntPtr lpszName, string extension)
		{
			if(IS_INTRESOURCE(lpszName))
			{
				return string.Format("{0}_{1}{2}", MainClass.baseName, (int) lpszName, extension);
			}
			else
			{
				return string.Format("{0}_{1}{2}", MainClass.baseName, Marshal.PtrToStringAuto(lpszName), extension);
			}
		}
		static int GetBitmapColors(ref BitmapInfoHeader bmi)
		{
			if(bmi.ClrUsed > 0)
			{
				return bmi.ClrUsed;
			}
			else
			{
				return (bmi.BitCount < 16) ? (1 << bmi.BitCount) : 0;
			}
		}
		static byte[] StructureToArray(object obj)
		{
			byte[] arr = new byte[Marshal.SizeOf(obj.GetType())];
			GCHandle gch = GCHandle.Alloc(obj, GCHandleType.Pinned);
			try
			{
				Marshal.Copy(gch.AddrOfPinnedObject(), arr, 0, arr.Length);
				return arr;
			}
			finally
			{
				gch.Free();
			}
		}
		static bool EnumResourceIcons(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam)
		{
			IntPtr hrsrc = FindResource(hModule, lpszName, lpszType);
			if(IntPtr.Zero == hrsrc)
			{
				return true;
			}
			int resSize = SizeofResource(hModule, hrsrc);
			IntPtr hglobal = LoadResource(hModule, hrsrc);
			if(IntPtr.Zero == hglobal)
			{
				return true;
			}
			byte[] resData = new byte[resSize];
			IntPtr ptr = LockResource(hglobal);
			Marshal.Copy(ptr, resData, 0, resData.Length);
			BitmapInfoHeader bmi = (BitmapInfoHeader) Marshal.PtrToStructure(ptr, typeof(BitmapInfoHeader));
			NewHeader nh = new NewHeader();
			nh.ResType = 1;
			nh.ResCount = 1;
			ResDir rd = new ResDir();
			rd.Icon.Width = (byte) bmi.Width;
			rd.Icon.Height = (byte) bmi.Height;
			rd.Icon.ColorCount = (byte) GetBitmapColors(ref bmi);
			rd.Planes = bmi.Planes;
			rd.BitCount = bmi.BitCount;
			rd.BytesInRes = resSize;
			rd.Offset = 6 + 16;
			byte[] nhData = StructureToArray(nh);
			byte[] rdData = StructureToArray(rd);
			string fileName = GetOutputFileName(lpszName, ".ico");
			BinaryWriter bw = new BinaryWriter(new FileStream(fileName, FileMode.Create));
			bw.Write(nhData, 0, nhData.Length);
			bw.Write(rdData, 0, rdData.Length);
			bw.Write(resData, 0, resData.Length);
			bw.Close();
			Console.WriteLine("created: {0}", fileName);
			return true;
		}
		static bool EnumResourceBitmaps(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam)
		{
			IntPtr hrsrc = FindResource(hModule, lpszName, lpszType);
			if(IntPtr.Zero == hrsrc)
			{
				return true;
			}
			int resSize = SizeofResource(hModule, hrsrc);
			IntPtr hglobal = LoadResource(hModule, hrsrc);
			if(IntPtr.Zero == hglobal)
			{
				return true;
			}
			BitmapFileHeader  bfh = new BitmapFileHeader();
			bfh.bfType = 0x4D42;
			byte[] resData = new byte[resSize];
			IntPtr ptr = LockResource(hglobal);
			Marshal.Copy(ptr, resData, 0, resData.Length);
			BitmapInfoHeader bmi = (BitmapInfoHeader) Marshal.PtrToStructure(ptr, typeof(BitmapInfoHeader));
			bfh.bfSize = Marshal.SizeOf(typeof(BitmapFileHeader)) + resSize;
			bfh.bfOffBits = Marshal.SizeOf(typeof(BitmapFileHeader)) + bmi.Size + 4 * GetBitmapColors(ref bmi);
			byte[] bfhData = StructureToArray(bfh);
			string fileName = GetOutputFileName(lpszName, ".bmp");
			BinaryWriter bw = new BinaryWriter(new FileStream(fileName, FileMode.Create));
			bw.Write(bfhData);
			bw.Write(resData);
			bw.Close();
			Console.WriteLine(string.Format("created: {0}", fileName));
			return true;
		}
		static bool CheckMagic(IntPtr ptr, int resSize, byte[] magic)
		{
			if(resSize < magic.Length)
			{
				return false;
			}
			for(int i = 0; i < magic.Length; i++)
			{
				if(Marshal.ReadByte(ptr, i) != magic[i])
				{
					return false;
				}
			}
			return true;
		}
		static bool CheckMagic(IntPtr ptr, int ofs, int resSize, byte[] magic)
		{
			return CheckMagic(new IntPtr(((int)ptr)+ofs), resSize - ofs, magic);
		}
		static bool CheckMagic(IntPtr ptr, int resSize, string sig)
		{
			return CheckMagic(ptr, resSize, System.Text.Encoding.ASCII.GetBytes(sig));
		}
		static bool CheckMagic(IntPtr ptr, int ofs, int resSize, string sig)
		{
			return CheckMagic(ptr, ofs, resSize, System.Text.Encoding.ASCII.GetBytes(sig));
		}
		static bool EnumResourceBinaryData(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam)
		{
			IntPtr hrsrc = FindResource(hModule, lpszName, lpszType);
			if(IntPtr.Zero == hrsrc)
			{
				return true;
			}
			int resSize = SizeofResource(hModule, hrsrc);
			if(resSize < 80)
			{
				return true;
			}
			IntPtr hglobal = LoadResource(hModule, hrsrc);
			if(IntPtr.Zero == hglobal)
			{
				return true;
			}
			IntPtr ptr = LockResource(hglobal);
			string ext = null;
			if(CheckMagic(ptr, resSize, new byte[]{0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a}))
			{
				ext = ".png";
			}
			else if(CheckMagic(ptr, resSize, "GIF89a") || CheckMagic(ptr, resSize, "GIF87a"))
			{
				ext = ".gif";
			}
			else if(CheckMagic(ptr, resSize, new byte[]{0xFF, 0xD8, 0xFF}) && (CheckMagic(ptr, 6, resSize, "Exif") || CheckMagic(ptr, 6, resSize, "JFIF")))
			{
				ext = ".jpg";
			}
			else if(CheckMagic(ptr, resSize, "%PDF"))
			{
				ext = ".pdf";
			}
			else if(CheckMagic(ptr, resSize, "PK"))
			{
				ext = ".zip";
			}
			else if(CheckMagic(ptr, resSize, "BM"))
			{
				ext = ".bmp";
			}
			else
			{
				Console.Error.WriteLine("magic check failed: type {0} name {1}", (int)lpszType, (int)lpszName);
			}
			if(ext != null)
			{
				string fileName = GetOutputFileName(lpszName, ext);
				byte[] resData = new byte[resSize];
				Marshal.Copy(ptr, resData, 0, resData.Length);
				FileStream fs = new FileStream(fileName, FileMode.Create);
				fs.Write(resData, 0, resData.Length);
				fs.Close();
				Console.WriteLine(string.Format("created: {0}", fileName));
			}
			return true;
		}
		static bool EnumResourceTypes(IntPtr hModule, IntPtr lpszType, IntPtr lParam)
		{
			if(lpszType == ResourceTypes.RT_BITMAP)
			{
				EnumResourceNames(hModule, ResourceTypes.RT_BITMAP, new ResNameEnumProc(EnumResourceBitmaps), IntPtr.Zero);
			}
			else if(lpszType == ResourceTypes.RT_ICON)
			{
				EnumResourceNames(hModule, ResourceTypes.RT_ICON, new ResNameEnumProc(EnumResourceIcons), IntPtr.Zero);
			}
			else
			{
				Console.Error.WriteLine(string.Format("Enumerating resources of type {0}", (int)lpszType));
				EnumResourceNames(hModule, lpszType, new ResNameEnumProc(EnumResourceBinaryData), IntPtr.Zero);
			}
			return true;
		}
		static void Extract(string fileName)
		{
			MainClass.baseName = Path.GetFileNameWithoutExtension(fileName);
			IntPtr hModule = LoadLibraryEx(fileName, IntPtr.Zero, LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
			if(IntPtr.Zero == hModule)
			{
				Console.Error.WriteLine(string.Format("error: cannot open {0}", fileName));
			}
			try
			{
				EnumResourceTypes(hModule, new ResTypeEnumProc(EnumResourceTypes), IntPtr.Zero);
			}
			finally
			{
				FreeLibrary(hModule);
			}
		}
		public static void Main(string[] args)
		{
			if(args.Length < 1)
			{
				Console.WriteLine("Extracts bitmap, icon and png resources from Win32 executables (*.exe and *.dll files).\nsyntax: respeel filename.ext\nFiles are stored as filename_resname.ext (.bmp, .ico, or .png)");
				return;
			}
			for(int i = 0; i < args.Length; i++)
			{
				try
				{
					Extract(args[i]);
				}
				catch(System.IO.FileNotFoundException)
				{
					Console.WriteLine("error: file not found: " + args[i]);
				}
			}
		}
	}
}
