using System.Collections.Generic;

namespace CDForestFull
{
	public class FileParseResult
	{
		public Dictionary<int, string> Hash { get; set; }

		public Dictionary<int, int> Frequency { get; set; }

		public Dictionary<int, List<int>> Distances { get; set; }
	}
}
