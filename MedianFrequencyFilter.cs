using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDForestFull
{
	public static class MedianFrequencyFilter
	{
		/// as median finding algorithms.
		/// Pivot is selected ranodmly if random number generator is supplied else its selected as last element in the list.
		/// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 171
		/// </summary>
		private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
		{
			if (rnd != null)
				list.Swap(end, rnd.Next(start, end + 1));

			var pivot = list[end];
			var lastLow = start - 1;
			for (var i = start; i < end; i++)
			{
				if (list[i].CompareTo(pivot) <= 0)
					list.Swap(i, ++lastLow);
			}
			list.Swap(end, ++lastLow);
			return lastLow;
		}

		/// <summary>
		/// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
		/// Note: specified list would be mutated in the process.
		/// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
		/// </summary>
		public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T>
		{
			return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
		}
		private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
		{
			while (true)
			{
				var pivotIndex = list.Partition(start, end, rnd);
				if (pivotIndex == n)
					return list[pivotIndex];

				if (n < pivotIndex)
					end = pivotIndex - 1;
				else
					start = pivotIndex + 1;
			}
		}

		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			if (i == j)   //This check is not required but Partition function may make many calls so its for perf reason
				return;
			var temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}

		/// <summary>
		/// Note: specified list would be mutated in the process.
		/// </summary>
		public static T Median<T>(this IList<T> list) where T : IComparable<T>
		{
			return list.NthOrderStatistic((list.Count - 1) / 2);
		}

		public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
		{
			var list = sequence.Select(getValue).ToList();
			var mid = (list.Count - 1) / 2;
			return list.NthOrderStatistic(mid);
		}

		public static FileParseResult FilterMedian(this FileParseResult input, int treshold)
		{
			if(input == null)
			{
				return null;
			}

			if(input.Hash.Count < treshold)
			{
				return input;
			}

			List<KeyValuePair<int, int>> sortedFrequency = input.Frequency.ToList();

			sortedFrequency.Sort(
				delegate (KeyValuePair<int, int> pair1,
				KeyValuePair<int, int> pair2)
				{
					return pair1.Value.CompareTo(pair2.Value);
				}
			);


			int median = sortedFrequency.Select(f => f.Value).ToList().Median();

			FileParseResult output = new FileParseResult { Hash = input.Hash, Frequency = new Dictionary<int, int>(), Distances = input.Distances };

			for (int i = 0; i < sortedFrequency.Count; i++)
			{
				if(sortedFrequency[i].Value < median)
				{
					output.Frequency.Add(sortedFrequency[i].Key, sortedFrequency[i].Value);
				}
			}

			foreach (int i in output.Frequency.Keys)
			{
				if (!output.Distances.ContainsKey(output.Frequency[i]))
				{
					output.Distances.Remove(output.Frequency[i]);
				}
				else
				{
					foreach (int j in output.Frequency.Keys)
					{
						if (!output.Distances[output.Frequency[i]].Contains(output.Frequency[j]) || i == j)
						{
							output.Distances[output.Frequency[i]].Remove(output.Frequency[j]);
						}
					}
				}
			}

			return output;
		}
	}
}
