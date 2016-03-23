using System;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Creates .NET resource files.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

namespace mkres
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			if((0 == args.Length) || ("/?" == args[0]))
			{
				Console.WriteLine("Creates .NET resource files.\nsyntax: mkres filename.(resources|resX) resources...\nsupported resources:\n\t/icon resName fileName\n\t/image resName fileName\n\t/cursor resName fileName\n\t/string resName \"string\"\n\t/filestring resName fileName\n\t/filebytes resName fileName\n\t/imagelist resName fileName fileName fileName...");
				return;
			}
			IResourceWriter rw;
			switch(Path.GetExtension(args[0]).ToLower())
			{
				case ".resources":
					try
					{
						rw = new ResourceWriter(args[0]);
					}
					catch(Exception e)
					{
						Console.WriteLine(String.Format("error: {0}: {1}", args[0], e.Message));
						return;
					}
					break;
				case ".resx":
					try
					{
						rw = new ResXResourceWriter(args[0]);
					}
					catch(Exception e)
					{
						Console.WriteLine(String.Format("error: {0}: {1}", args[0], e.Message));
						return;
					}
					break;
				default:
					Console.WriteLine(String.Format("error: {0}: unknown filename extension; must be .resources or .resX", args[0]));
					return;
			}
			for(int i = 1; i < args.Length;)
			{
				switch(args[i].ToLower())
				{
					default:
						Console.WriteLine(String.Format("error: {0}: invalid argument; see mkres /? for syntax help", args[i]));
						return;
					case "/icon":
						try
						{
							rw.AddResource(args[i+1], new Icon(args[i + 2]));
						}
						catch(Exception e)
						{
							Console.WriteLine(String.Format("error: {0}: {1}", args[i + 2], e.Message));
							return;
						}
						i += 3;
						break;
					case "/image":
						try
						{
							rw.AddResource(args[i + 1], Image.FromFile(args[i + 2]));
						}
						catch(Exception e)
						{
							Console.WriteLine(String.Format("error: {0}: {1}", args[i + 2], e.Message));
							return;
						}
						i += 3;
						break;
					case "/cursor":
						try
						{
							rw.AddResource(args[i + 1], new Cursor(args[i + 2]));
						}
						catch(Exception e)
						{
							Console.WriteLine(String.Format("error: {0}: {1}", args[i + 2], e.Message));
							return;
						}
						i += 3;
						break;
					case "/string":
						rw.AddResource(args[i + 1], args[i + 2]);
						i += 3;
						break;
					case "/filestring":
						try
						{
							StreamReader sr = new StreamReader(args[i + 2]);
							rw.AddResource(args[i + 1], sr.ReadToEnd());
							sr.Close();
						}
						catch(Exception e)
						{
							Console.WriteLine(String.Format("error: {0}: {1}", args[i + 2], e.Message));
							return;
						}
						i += 3;
						break;
					case "/filebytes":
						try
						{
							FileStream fs = new FileStream(args[i + 2], FileMode.Open);
							byte[] arr = new byte[fs.Length];
							fs.Read(arr, 0, arr.Length);
							rw.AddResource(args[i + 1], arr);
							fs.Close();
						}
						catch(Exception e)
						{
							Console.WriteLine(String.Format("error: {0}: {1}", args[i + 2], e.Message));
							return;
						}
						i += 3;
						break;
					case "/imagelist":
						using(ImageList il = new ImageList())
						{
							il.ColorDepth = ColorDepth.Depth32Bit;
							string resname = args[i + 1];
							bool haveSetSize = false;
							try
							{
								for(i+=2; (i < args.Length) && ('/' != args[i][0]); i++)
								{
									Image img;
									Icon ico;
									switch(Path.GetExtension(args[i]).ToLower())
									{
										default:
											img = Image.FromFile(args[i]);
											if(!haveSetSize)
											{
												il.ImageSize = img.Size;
												haveSetSize = true;
											}
											else if(img.Size != il.ImageSize)
											{
												Console.WriteLine("warning: cannot store differently sized images in an ImageList.\n{0} stored in ImageList {1} will be truncated from {2} to {3} pixels\n", args[i], resname, img.Size, il.ImageSize);
											}
											il.Images.Add(img);
											break;
										case ".ico":
											ico = new Icon(args[i]);
											if(!haveSetSize)
											{
												il.ImageSize = ico.Size;
												haveSetSize = true;
											}
											else if(ico.Size != il.ImageSize)
											{
												Console.WriteLine("warning: cannot store differently sized images in an ImageList.\n{0} stored in ImageList {1} will be truncated from {2} to {3} pixels\n", args[i], resname, ico.Size, il.ImageSize);
											}
											il.Images.Add(ico);
											break;
									}
								}
							}
							catch(Exception e)
							{
								Console.WriteLine(String.Format("error: {0}: {1}", args[i], e.Message));
								return;
							}
							try
							{
								rw.AddResource(resname, il.ImageStream);
							}
							catch(Exception e)
							{
								Console.WriteLine(String.Format("error: {0}: {1}", resname, e.Message));
							}
						}
						break;
				}
			}
			try
			{
				rw.Close();
			}
			catch(Exception e)
			{
				Console.WriteLine(String.Format("error: {0}: {1}", args[0], e.Message));
			}
		}
	}
}
