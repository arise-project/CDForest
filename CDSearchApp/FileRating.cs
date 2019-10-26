using System.Collections.Generic;

namespace CDSearchApp
{
    public class FileRating
    {
        public string FileName { get; set; }

        public int Rating { get; set; }

        public List<string> Occurances { get; set; }

        public List<string> Missing { get; set; }

        public int[] MinDistances { get; set; }
    }
}
