using System;
using System.IO;
using System.Windows;
using System.Reflection;

[assembly: AssemblyCompany("Michael Trenholm-Boyle")]
[assembly: AssemblyCopyright("© 2016 Michael Trenholm-Boyle")]
[assembly: AssemblyTitle("Copies text from the clipboard to a file.")]
[assembly: AssemblyProduct(@"Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite")]
[assembly: AssemblyVersion("0.1.*")]

namespace paste
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string fout = null;
            string ftyp = null;
            TextDataFormat text = TextDataFormat.UnicodeText;
            if(args.Length > 0)
            {
                switch(args[0].ToLowerInvariant())
                {
                    default:
                        fout = args[0];
                        break;
                    case "-":
                    case "--":
                    case "stdout":
                        break;
                    case "--query":
                    case "-q":
                    case "/q":
                        QueryFormats();
                        Environment.Exit(0);
                        return;
                    case "--help":
                    case "-h":
                    case "/h":
                    case "-?":
                    case "/?":
                        PrintSyntax();
                        Environment.Exit(0);
                        return;
                }
                if(args.Length > 1)
                {
                    ftyp = args[1];
                    switch(ftyp.ToLowerInvariant())
                    {
                        default:
                            break;
                        case "html":
                            text = TextDataFormat.Html;
                            break;
                        case "rtf":
                            text = TextDataFormat.Rtf;
                            break;
                        case "csv":
                            text = TextDataFormat.CommaSeparatedValue;
                            break;
                    }
                }
            }
            if(Clipboard.ContainsText(text))
            {
                if (string.IsNullOrWhiteSpace(fout))
                {
                    Console.WriteLine(Clipboard.GetText(text));
                }
                else
                {
                    File.WriteAllText(fout, Clipboard.GetText(text));
                }
                Environment.Exit(0);
                return;
            }
            else if (Clipboard.ContainsFileDropList())
            {
                if(string.IsNullOrWhiteSpace(fout))
                {
                    foreach (string x in Clipboard.GetFileDropList())
                    {
                        Console.WriteLine(File.ReadAllText(x));
                    }
                }
                else if(Directory.Exists(fout))
                {
                    foreach (string x in Clipboard.GetFileDropList())
                    {
                        string y = Path.Combine(fout, Path.GetFileName(x));
                        if(Directory.Exists(x))
                        {
                            CopyFilesRecursively(x, y);
                        }
                        else
                        {
                            File.Copy(x, y, true);
                            Console.WriteLine(x);
                        }
                    }

                }
                else
                {
                    using (Stream o = new FileStream(fout, FileMode.Create))
                    {
                        byte[] buf = new byte[4096];
                        foreach (string x in Clipboard.GetFileDropList())
                        {
                            using (Stream i = new FileStream(x, FileMode.Open, FileAccess.Read))
                            {
                                while (true)
                                {
                                    int j = i.Read(buf, 0, buf.Length);
                                    if (j > 0)
                                    {
                                        o.Write(buf, 0, j);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            Console.WriteLine(x);
                        }
                    }
                }
                Environment.Exit(0);
                return;
            }
            else
            {
                Console.Error.WriteLine("Error: No supported format on clipboard");
                QueryFormats();
                Environment.Exit(1);
                return;
            }
        }

        static void PrintSyntax()
        {
            Console.Error.WriteLine("Saves clipboard data to a file (default is stdout).");
            Console.Error.WriteLine("Syntax: paste (file)?");
        }

        static void QueryFormats()
        {
            IDataObject data = Clipboard.GetDataObject();
            string[] formats = data?.GetFormats();
            if(formats == null)
            {
                Console.WriteLine("Clipboard is empty");
            }
            else
            {
                Console.WriteLine("Available formats in clipboard:");
                foreach (string x in formats)
                {
                    Console.WriteLine("\t" + x);
                }
                if(Clipboard.ContainsFileDropList())
                {
                    Console.WriteLine("Available files in drop list:");
                    foreach(string x in Clipboard.GetFileDropList())
                    {
                        Console.WriteLine("\t" + x);
                    }
                }
            }
        }

        static void CopyFilesRecursively(string srcdir, string dstdir)
        {
            foreach(string x in Directory.EnumerateDirectories(srcdir))
            {
                string y = Path.Combine(dstdir, Path.GetFileName(x));
                if (!Directory.Exists(y))
                {
                    Directory.CreateDirectory(y);
                }
                CopyFilesRecursively(x, y);
            }
            foreach(string x in Directory.EnumerateFiles(srcdir))
            {
                string y = Path.Combine(dstdir, Path.GetFileName(x));
                File.Copy(x, y, true);
                Console.WriteLine(x);
            }
        }
    }
}
