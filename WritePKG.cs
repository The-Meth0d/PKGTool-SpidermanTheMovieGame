using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PKGTool
{
	public class WritePKG
	{
		public string PKG_FOLDER;

		public string PKG_NAME;

		public uint TotalFolders;

		public uint TotalFiles;

		public uint TotalFileData;

		public List<string> AllFiles = new List<string>();

		public List<GameCatalog> GameCatalogs = new List<GameCatalog>();

		public List<GameFile> GameFiles = new List<GameFile>();

		public FileStream fs;

		public BinaryWriter bw;

		static WritePKG()
		{
		}

		public WritePKG(string pkgFolder)
		{
			this.PKG_FOLDER = pkgFolder;
			this.PKG_NAME = (new DirectoryInfo(pkgFolder)).Name;
			Console.WriteLine("  Loading files from directory...");
			string[] directories = Directory.GetDirectories(pkgFolder, "*.*", SearchOption.AllDirectories);
			uint num = 0;
			uint num1 = 0;
			uint num2 = 0;
			string[] strArrays = directories;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				string[] files = Directory.GetFiles(str);
				if (files.Length != 0)
				{
					List<uint> nums = new List<uint>();
					string[] strArrays1 = files;
					for (int j = 0; j < (int)strArrays1.Length; j++)
					{
						FileInfo fileInfo = new FileInfo(strArrays1[j]);
						uint num3 = num1;
						string name = fileInfo.Name;
						uint length = (uint)fileInfo.Length;
						uint num4 = num2;
						nums.Add(num3);
						this.GameFiles.Add(new GameFile(num3, name, length, num4));
						this.AllFiles.Add(fileInfo.FullName);
						num1++;
						num2 += length;
					}
					string str1 = string.Concat(str.Replace(string.Concat(this.PKG_FOLDER, "\\"), ""), "\\");
					this.GameCatalogs.Add(new GameCatalog(str1, (uint)nums.Count, nums));
					num++;
				}
			}
			this.TotalFolders = num;
			this.TotalFiles = num1;
			this.TotalFileData = num2;
			this.fs = new FileStream(string.Concat(this.PKG_NAME, ".pkg"), FileMode.Create);
			this.bw = new BinaryWriter(this.fs);
			this.WriteHeader();
			this.WriteCatalogBlock();
			this.WriteFileInfoBlock();
			this.WriteDataBlock();
			Console.WriteLine();
			this.bw.Close();
			this.fs.Close();
		}

		private static byte[] StringToFixedByteArray(string str, int length)
		{
			if (char.IsNumber(str[0]))
			{
				str = str.Insert(0, "\0");
			}
			byte[] bytes = Encoding.UTF8.GetBytes(str.PadRight(length, '\0'));
			return bytes;
		}

		public void WriteCatalogBlock()
		{
			Console.WriteLine("  Writing PKG catalogs information...");
			foreach (GameCatalog gameCatalog in this.GameCatalogs)
			{
				this.bw.Write(WritePKG.StringToFixedByteArray(gameCatalog.Name, 64));
				this.bw.Write(gameCatalog.TotalFiles);
				foreach (uint fileID in gameCatalog.FileIDs)
				{
					this.bw.Write(fileID);
				}
			}
		}

		public void WriteDataBlock()
		{
			Console.WriteLine("  Writing file data to PKG... be patient, this may take a while!");
			foreach (string allFile in this.AllFiles)
			{
				this.bw.Write(File.ReadAllBytes(allFile));
			}
		}

		public void WriteFileInfoBlock()
		{
			Console.WriteLine("  Writing PKG files information...");
			foreach (GameFile gameFile in this.GameFiles)
			{
				this.bw.Write(WritePKG.StringToFixedByteArray(gameFile.Name, 40));
				this.bw.Write(gameFile.Size);
				this.bw.Write(gameFile.Offset);
			}
		}

		public void WriteHeader()
		{
			Console.WriteLine("  Writing PKG header information...");
			this.bw.Write(this.TotalFolders);
			this.bw.Write(this.TotalFiles);
			this.bw.Write(this.TotalFileData);
		}
	}
}