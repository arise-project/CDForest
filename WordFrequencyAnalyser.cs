using System.Collections.Generic;

namespace CDForest
{
	public class WordFrequencyAnalyser
	{
		public WordFrequencyAnalyser()
		{
		}

		public Dictionary<int, string> Hash { get; } = new Dictionary<int, string>();

		public Dictionary<string, int> Frequency { get; } = new Dictionary<string, int>();

		public void Analyse(List<string> words)
		{
			Frequency.Clear();

			foreach(var word in words)
			{
				var c = word.ToLower();
				if(Frequency.ContainsKey(c))
				{
					Frequency[c]++;
				}
				else
				{
					Frequency.Add(c, 1);
					Hash.Add(c.GetHashCode(), c);
				}
			}
		}
	}
}
