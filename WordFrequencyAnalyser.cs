using System.Collections.Generic;

namespace CDForestFull
{
	public class WordFrequencyAnalyser
	{
		public WordFrequencyAnalyser()
		{
		}

		public Dictionary<int, string> Hash { get; } = new Dictionary<int, string>();

		public Dictionary<int, int> Frequency { get; } = new Dictionary<int, int>();

		public void Analyse(List<string> words)
		{
			Frequency.Clear();

			foreach (var word in words)
			{
				var c = word.ToLower();
				var h = c.GetHashCode();
				if (Frequency.ContainsKey(h))
				{
					Frequency[h]++;
				}
				else
				{
					Frequency.Add(h, 1);
					if (!Hash.ContainsKey(h))
					{
						Hash.Add(h, c);
					}
				}
			}
		}
	}
}
