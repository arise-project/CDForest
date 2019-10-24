using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CDForestFull
{
	public class FileAnalyseWriter
	{
		public string WriteToFile(string fileName, FileParseResult result, byte[] hash, string output, Features features)
		{
			using (Stream stream = new FileStream(output, FileMode.Create))
			{
				using (BinaryWriter bw = new BinaryWriter(stream))
				{
					WriteHeader(bw, result, (int)features);
					WriteHash(bw, result.Hash);
					WriteFrequency(bw, result.Frequency);
					WriteDistances(bw, result.Distances);
					bw.Close();
				}
				stream.Close();
			}

			return output;
		}

		public static void WriteHeader(BinaryWriter bw, FileParseResult result, int features)
		{
			int hashOffset = 6 * 4 + 4 * 10;

			int hashSize = result.Hash.Count * 8 + result.Hash.Sum(h => Encoding.UTF8.GetBytes(h.Value).Length);

			int frequencyOffset = hashOffset + hashSize + 4 * 10;

			int frequencySize = result.Frequency.Count() * 4 + result.Frequency.Count * 8;

			int distancesOffset = frequencyOffset + frequencySize + 4 * 10;

			int distancesSize = result.Distances.Sum(h => h.Value.Count) + result.Distances.Count * 8 + 4;

			bw.Write(hashOffset);
			bw.Write(hashSize);
			bw.Write(frequencyOffset);
			bw.Write(frequencySize);
			bw.Write(distancesOffset);
			bw.Write(distancesSize);
			bw.Write(features);
		}

		public static void WriteHash(BinaryWriter bw, Dictionary<int, string> hash)
		{
			for (int i = 0; i < 10; i++)
				bw.Write(i);

			foreach(var c in hash)
			{
				bw.Write(c.Key);
				bw.Write(Encoding.UTF8.GetBytes(c.Value));
				bw.Write(new byte[] { 0, 0, 0, 0 });
			}
		}

		public static void WriteFrequency(BinaryWriter bw, Dictionary<int, int> frequency)
		{
			for (int i = 10; i < 20; i++)
				bw.Write(i);

			foreach (var c in frequency)
			{
				bw.Write(c.Key);
				bw.Write(c.Value);
				bw.Write(new byte[] { 0, 0, 0, 0 });
			}
		}

		public static void WriteDistances(BinaryWriter bw, Dictionary<int, List<int>> distances)
		{
			for (int i = 20; i < 30; i++)
				bw.Write(i);

			foreach (var c in distances)
			{
				bw.Write(c.Key);
				foreach(var v in c.Value)
					bw.Write(v);
				bw.Write(new byte[] { 0, 0, 0, 0 });
			}
		}
	}
}
