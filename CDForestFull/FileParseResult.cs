using System.Collections.Generic;

namespace CDForest
{
	public class FileParseResult
	{
		public Dictionary<int, string> Hash { get; set; }

		public Dictionary<string, int> Frequency { get; set; }

		public Dictionary<int, List<int>> Distances { get; set; }
	}
}
