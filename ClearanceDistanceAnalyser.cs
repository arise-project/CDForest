using System.Collections.Generic;
using System.Linq;

namespace CDForestFull
{
	public class ClearanceDistanceAnalyser
	{
		private Dictionary<int, HashSet<int>> HDistances { get; } = new Dictionary<int, HashSet<int>>();
		public Dictionary<int, List<int>> Distances { get; } = new Dictionary<int, List<int>>();

		public void Analyse(int clearance, List<string> words)
		{
			HDistances.Clear();
			Distances.Clear();
			int index = 0;
			while (index + clearance < words.Count)
			{
				var range = words.GetRange(index, clearance);

				var first = range.FirstOrDefault();

				if (string.IsNullOrEmpty(first))
				{
					index++;
					continue;
				}

				var f = first.ToLower().GetHashCode();
				if (HDistances.ContainsKey(f))
				{
					foreach (var curr in range.Skip(1))
					{
						var c = curr.ToLower().GetHashCode();
						if (!HDistances[f].Contains(c))
						{
							HDistances[f].Add(c);
							Distances[f].Add(c);
						}
					}
				}
				else
				{
					HDistances.Add(f, new HashSet<int>());
					Distances.Add(f, new List<int>());
				}

				index++;
			}
		}
	}
}
