using System;
using System.IO;

namespace PKGTool
{
	internal class Program
	{
		public ExtractPKG Extractor;

		public WritePKG Writer;

		public Program()
		{
		}

		public void Logo()
		{
			this.Write("");
			this.Write("SPIDERMAN: THE MOVIE GAME (PC) - PKG Tool");
			this.Write("Written by Leo aka \"Meth0d\" - https://meth0d.org");
			this.Write("---------------------------------------------------");
			this.Write("This tool is provided without any kind of warranty.");
			this.Write("Remember to always backup your game files.");
			this.Write("---------------------------------------------------");
			this.Write("");
		}

		private static void Main(string[] args)
		{
			Console.Title = "Meth0d's PKG Tool - 1.0";
			Program program = new Program();
			program.Logo();
			if (args.Length == 0)
			{
				program.ToolUsage();
			}
			else if (!File.Exists(args[0]))
			{
				program.REPACK(args[0]);
			}
			else if (Path.GetExtension(args[0]) != ".pkg")
			{
				Console.ForegroundColor = ConsoleColor.Red;
				program.Write("Invalid file format, not a PKG file... Enter to close.");
				Console.ReadLine();
				Environment.Exit(0);
			}
			else
			{
				program.UNPACK(args[0]);
			}
			Console.ReadLine();
			Environment.Exit(0);
		}

		public void REPACK(string pkgFolderPath)
		{
			this.Writer = new WritePKG(pkgFolderPath);
			Console.ForegroundColor = ConsoleColor.Green;
			this.Write(string.Concat("All files were successfully repacked to ", this.Writer.PKG_NAME, ".pkg"));
		}

		public void ToolUsage()
		{
			Console.ForegroundColor = ConsoleColor.White;
			this.Write("[ TOOL USAGE ] - This is a drag-and-drop software.");
			this.Write("To unpack: drop a PKG FILE to the tool executable.");
			this.Write("To repack: drop a FOLDER to the tool executable.");
			this.Write("");
			this.Write("Enter to close.");
		}

		public void UNPACK(string pkgFile)
		{
			this.Extractor = new ExtractPKG(pkgFile);
			Console.ForegroundColor = ConsoleColor.Green;
			this.Write("All files were successfully extracted.");
		}

		public void Write(string text = "")
		{
			Console.WriteLine(string.Concat("  ", text));
		}
	}
}