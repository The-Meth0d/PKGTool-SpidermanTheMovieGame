using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace PKGTool
{
	public class ExtractPKG
	{
		public string PKG_NAME;

		public long FILEDATA_START_OFFSET;

		public BinaryReader br;

		public FileStream fs;

		public uint TotalFolders;

		public uint TotalFiles;

		public uint TotalSize;

		public List<GameCatalog> GameCatalogs = new List<GameCatalog>();

		public List<GameFile> GameFiles = new List<GameFile>();

		static ExtractPKG()
		{
		}

		public ExtractPKG(string pkgFile)
		{
			this.PKG_NAME = Path.GetFileNameWithoutExtension(pkgFile);
			this.fs = new FileStream(pkgFile, FileMode.Open);
			this.br = new BinaryReader(this.fs);
			this.ReadHeader();
			this.ReadGameCatalog();
			this.ReadGameFiles();
			this.ExtractAllFiles();
			Console.WriteLine();
			this.br.Close();
			this.fs.Close();
		}

		public void ExtractAllFiles()
		{
			foreach (GameCatalog gameCatalog in this.GameCatalogs)
			{
				Console.WriteLine(string.Concat(new object[] { "  Extracting ", gameCatalog.TotalFiles, " files from catalog: ", gameCatalog.Name }));
				foreach (uint fileID in gameCatalog.FileIDs)
				{
					GameFile fileById = this.GetFileById(fileID);
					string str = string.Concat(this.PKG_NAME, "\\", gameCatalog.Name, "\\");
					Directory.CreateDirectory(str);
					File.WriteAllBytes(string.Concat(str, fileById.Name), this.GetFileData((int)fileById.Size, fileById.Offset));
				}
			}
		}

		public GameFile GetFileById(uint fileId)
		{
			GameFile gameFile = this.GameFiles.Find((GameFile Item) => Item.Id == fileId);
			return gameFile;
		}

		public byte[] GetFileData(int size, uint offset)
		{
			this.br.BaseStream.Position = (long)((uint)this.FILEDATA_START_OFFSET + offset);
			byte[] numArray = new byte[size];
			this.br.Read(numArray, 0, size);
			return numArray;
		}

		public void ReadGameCatalog()
		{
			for (uint i = 0; i < this.TotalFolders; i++)
			{
				string str = this.ReadName(this.br.ReadBytes(64));
				uint num = this.br.ReadUInt32();
				List<uint> nums = new List<uint>();
				for (uint j = 0; j < num; j++)
				{
					nums.Add(this.br.ReadUInt32());
				}
				this.GameCatalogs.Add(new GameCatalog(str, num, nums));
			}
		}

		public void ReadGameFiles()
		{
			for (uint i = 0; i < this.TotalFiles; i++)
			{
				uint num = i;
				string str = this.ReadName(this.br.ReadBytes(40));
				uint num1 = this.br.ReadUInt32();
				uint num2 = this.br.ReadUInt32();
				this.GameFiles.Add(new GameFile(num, str, num1, num2));
				num++;
			}
			this.FILEDATA_START_OFFSET = this.br.BaseStream.Position;
		}

		public void ReadHeader()
		{
			this.TotalFolders = this.br.ReadUInt32();
			this.TotalFiles = this.br.ReadUInt32();
			this.TotalSize = this.br.ReadUInt32();
		}

		public string ReadName(byte[] bytes)
		{
			string empty = string.Empty;
			empty = Encoding.UTF8.GetString(bytes);
			if (bytes[0] == 0)
			{
				empty = empty.Substring(1);
			}
			int num = empty.IndexOf('\0');
			if (num > 0)
			{
				empty = empty.Substring(0, num);
			}
			return empty;
		}
	}
}