using System.Collections.Generic;
using System.Text;

namespace CDForest
{
	public class WordSplitter
	{
		public List<string> SplitString(string text, int[] lengths)
		{
			List<string> output = new List<string>();

			List<string> words = Split(text);

			int i = 0;
			int lineNum = 0;
			string s = string.Empty;
			while (i < words.Count)
			{
				if (s.Length + words[i].Length < lengths[lineNum])
				{
					s += words[i];
					i++;
					if (lineNum < lengths.Length - 1)
						lineNum++;
				}
				else
				{
					output.Add(s);
					s = string.Empty;
				}

			}

			s.Remove(s.Length - 1, 1);// deletes last extra space.

			return output;
		}


		public List<string> Split(string text)
		{
			List<string> result = new List<string>();
			StringBuilder sb = new StringBuilder();

			foreach (var letter in text)
			{
				if (letter != ' ' && letter != '\t' && letter != '\n')
				{
					sb.Append(letter);
				}
				else
				{
					if (sb.Length > 0)
					{
						var sr = sb.ToString();
						if (!string.IsNullOrWhiteSpace(sr))
						{
							result.Add(sb.ToString());
						}
							
					}

					var r = letter.ToString();
					if (!string.IsNullOrWhiteSpace(r))
					{
						result.Add(letter.ToString());
					}
						
					sb = new StringBuilder();
				}
			}

			return result;
		}
	}
}
