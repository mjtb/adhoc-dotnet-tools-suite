// © 2016 Michael Trenholm-Boyle. Part of Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite.
// This file is used by git_receive_pack.cs, git_upload_archive.cs, and git_upload_pack.cs.
// Delegates the call to the appropriate installed binary, unescaping Bash shell escapes and converting them to DOS format.
using System;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace MJTB
{
    public class GitWrap
    {
        private static string unbash(string s)
        {
            if(string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            StringBuilder buf = new StringBuilder();
            char quot = '\0';
            for(int i = 0; i < s.Length; ++i)
            {
                char c = s[i];
                if (c == '\'' || c == '\"')
                {
                    if (quot == c)
                    {
                        quot = '\0';
                    }
                    else if (quot != '\0')
                    {
                        buf.Append(c);
                    }
                    else
                    {
                        quot = c;
                    }
                }
                else if (c == '\\')
                {
                    char cc = (i + 1 < s.Length) ? s[i + 1] : '\0';
                    if(quot == '\"')
                    {
                        if (cc != '$')
                        {
                            buf.Append('\\');
                        }
                        buf.Append(cc);
                    }
                    else if(quot == '\'')
                    {
                        buf.Append(c);
                    }
                    else
                    {
                        switch(cc)
                        {
                            default:
                                buf.Append(cc);
                                break;
                            case 'a':
                                buf.Append('\a');
                                break;
                            case 'b':
                                buf.Append('\b');
                                break;
                            case 'e':
                                buf.Append((char)27);
                                break;
                            case 'f':
                                buf.Append('\f');
                                break;
                            case 'n':
                                buf.Append('\n');
                                break;
                            case 'r':
                                buf.Append('\r');
                                break;
                            case 't':
                                buf.Append('\t');
                                break;
                            case 'v':
                                buf.Append('\v');
                                break;
                            case 'u':
                                buf.Append((char)int.Parse(s.Substring(i + 2, 4), NumberStyles.AllowHexSpecifier));
                                i += 4;
                                break;
                            case 'x':
                                buf.Append((char)int.Parse(s.Substring(i + 2, 2), NumberStyles.AllowHexSpecifier));
                                i += 2;
                                break;
                        }
                        ++i;
                    }
                }
                else if(c == '$' && quot != '\'')
                {
                    string varnam = null;
                    if((i + 1) < s.Length && s[i + 1] == '{')
                    {
                        int j = s.IndexOf('}', i + 2);
                        if(j > i)
                        {
                            varnam = s.Substring(i + 2, j - i - 2);
                            i = j;
                        }
                    }
                    else
                    {
                        int j;
                        for(j = i + 1; j < s.Length; ++j)
                        {
                            char cc = s[j];
                            if(char.IsLetterOrDigit(cc) || c == '_')
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                        varnam = s.Substring(i + 1, j - i - 1);
                        i = j - 1;
                    }
                    if(string.IsNullOrWhiteSpace(varnam))
                    {
                        buf.Append('$');
                    }
                    else
                    {
                        string varval = Environment.GetEnvironmentVariable(varnam);
                        if(!string.IsNullOrEmpty(varval))
                        {
                            buf.Append(varval);
                        }
                    }
                }
                else
                {
                    buf.Append(c);
                }
            }
            return buf.ToString();
        }
        private static string[] unbash(string[] args)
        {
            string[] rv = new string[args.Length];
            for(int i = 0; i < rv.Length; ++i)
            {
                rv[i] = unbash(args[i]);
            }
            return rv;
        }
        private static bool dosneedsescape(string s)
        {
            if(string.IsNullOrEmpty(s))
            {
                return true;
            }
            for(int i = 0; i < s.Length; ++i)
            {
                char c = s[i];
                if(char.IsWhiteSpace(c) || char.IsControl(c) || c == '\"' || c == '%' || c == '&' || c == '>' || c == '>' || c == '^')
                {
                    return true;
                }
            }
            return false;
        }
        private static string dosjoin(string[] args)
        {
            StringBuilder buf = new StringBuilder();
            foreach(string s in args)
            {
                if(buf.Length > 0)
                {
                    buf.Append(' ');
                }
                if(string.IsNullOrEmpty(s))
                {
                    buf.Append("\"\"");
                }
                else if(dosneedsescape(s))
                {
                    buf.Append('\"');
                    for(int i = 0; i < s.Length; ++i)
                    {
                        char c = s[i];
                        if(c == '\"')
                        {
                            buf.Append(c);
                        }
                        buf.Append(c);
                    }
                    buf.Append('\"');
                }
                else
                {
                    buf.Append(s);
                }
            }
            return buf.ToString();
        }
        public static int Execute(string cmd, string[] args)
        {
            try
            {
                string programFilesFolder = Environment.GetEnvironmentVariable("ProgramFiles");
                string gitFolder = Path.Combine(programFilesFolder, "Git");
                string cmdFileName = cmd.ToLowerInvariant().EndsWith(".exe") ? cmd : (cmd + ".exe");
                string cmdPath = Path.Combine(Path.Combine(gitFolder, "mingw64", "bin"), cmdFileName);
                string cmdArgs = dosjoin(unbash(args));
                ProcessStartInfo si = new ProcessStartInfo(cmdPath, cmdArgs);
                si.WorkingDirectory = gitFolder;
                si.UseShellExecute = false;
                Process p = Process.Start(si);
                p.WaitForExit();
                return p.ExitCode;
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return 1;
            }
        }
    }
}