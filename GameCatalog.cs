using System;
using System.Collections.Generic;

namespace PKGTool
{
	public class GameCatalog
	{
		public string Name;

		public uint TotalFiles;

		public List<uint> FileIDs;

		static GameCatalog()
		{
		}

		public GameCatalog(string name, uint totalFiles, List<uint> fileIDs)
		{
			this.Name = name;
			this.TotalFiles = totalFiles;
			this.FileIDs = fileIDs;
		}
	}
}