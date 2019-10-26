using CDSearchFile;
using System.Collections.Generic;
using System.Linq;

namespace CDSearchApp
{
    public class FileRatingAnalyser
    {
        public FileRating Analyse(string fileName, FileParseResult input, IEnumerable<string> words)
        {
            FileRating result = new FileRating
            {
                FileName = fileName,
                Occurances = words.Where(s => input.Hash.ContainsValue(s)).ToList(),
                Missing = words.Where(s => !input.Hash.ContainsValue(s)).ToList(),
                MinDistances = new int[words.Count(s => input.Hash.ContainsValue(s))],
                Rating = 0
            };

            if (result.Occurances.Count == 0)
            {
                return null;
            }

            var hases = words.Select(w => input.Hash.Any(h => string.Equals(h.Value, w)) ? input.Hash.First(h => string.Equals(h.Value, w)).Key : 0);
            var distances = input.Distances.Where(d => hases.Any(h => h == d.Key));

            int index = 0;
            foreach (var hash in hases)
            {
                if(hash == 0)
                {
                    continue;
                }

                int rating = 0;
                foreach (var distance in distances)
                {
                    if (distance.Value.Contains(hash))
                    {
                        rating++;
                    }
                }
                result.MinDistances[index++] = rating;
            }

            result.Rating = result.Occurances.Count + result.MinDistances.Sum();

            return result;
        }
    }
}
