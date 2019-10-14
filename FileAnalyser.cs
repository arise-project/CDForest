using System;
using TikaOnDotNet.TextExtraction;

namespace CDForestFull
{
	public class FileAnalyser
	{
		private readonly string _path;

		private readonly WordSplitter _wordSplitter = new WordSplitter();

		private readonly WordFrequencyAnalyser _wordFrequencyAnalyser = new WordFrequencyAnalyser();

		private readonly ClearanceDistanceAnalyser _clearanceDistanceAnalyser = new ClearanceDistanceAnalyser();

		private string _text;

		public FileAnalyser(string path)
		{
			_path = path;
		}

		public FileParseResult Parse(int clearance)
		{
			TextExtractionResult textExtractionResult;
			try
			{
				textExtractionResult = new TextExtractor().Extract(_path);
			}
			catch(Exception ex)
			{
				return null;
			}

			_text = textExtractionResult.Text;
			var words = _wordSplitter.Split(_text);
			_wordFrequencyAnalyser.Analyse(words);

			_clearanceDistanceAnalyser.Analyse(clearance, words);

			return new FileParseResult { Hash = _wordFrequencyAnalyser.Hash, Frequency = _wordFrequencyAnalyser.Frequency, Distances = _clearanceDistanceAnalyser.Distances };
		}
	}
}
