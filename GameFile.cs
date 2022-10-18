using System;

namespace PKGTool
{
	public class GameFile
	{
		public uint Id;

		public string Name;

		public uint Size;

		public uint Offset;

		public GameFile(uint id, string name, uint size, uint offset)
		{
			this.Id = id;
			this.Name = name;
			this.Size = size;
			this.Offset = offset;
		}
	}
}