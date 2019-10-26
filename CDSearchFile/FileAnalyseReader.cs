using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CDSearchFile
{
    public class FileAnalyseReader
    {
        int hashOffset;
        int hashSize;
        int frequencyOffset;
        int frequencySize;
        int distancesOffset;
        int distancesSize;
        int features;

        public FileParseResult Read(string fileName)
        {
            FileParseResult result = new FileParseResult();

            using (var reader = new BinaryReader(File.OpenRead(fileName)))
            {
                RaeadHeader(reader);
                result.Hash = ReadHash(reader, hashSize);
                result.Frequency = ReadFrequency(reader, frequencySize);
                result.Distances = ReadDistances(reader, distancesSize);
            }
            return result;
        }

        private Dictionary<int, List<int>> ReadDistances(BinaryReader reader, int size)
        {
            var distnaces = new Dictionary<int, List<int>>();
            SkipBeginMarker(reader);

            while (size > 0)
            {
                var h = reader.ReadInt32();
                size -= 4;

                int zeros = 0;
                List<int> d = new List<int>();
                while (zeros < 1)
                {
                    int b = reader.ReadInt32();
                    if (b != 0)
                    {
                        d.Add(b);
                    }
                    else
                    {
                        zeros++;
                    }
                    size -= 4;
                }

                distnaces.Add(h, d);
            }

            return distnaces;
        }

        private Dictionary<int, int> ReadFrequency(BinaryReader reader, int size)
        {
            var frequency = new Dictionary<int, int>();
            SkipBeginMarker(reader);

            while (size > 0)
            {
                var h = reader.ReadInt32();
                var c = reader.ReadInt32();
                frequency.Add(h, c);
                int cs1 = reader.ReadInt32();
                size -= 12;
            }

            return frequency;
        }

        private Dictionary<int, string> ReadHash(BinaryReader reader, int size)
        {
            var hash = new Dictionary<int, string>();
            SkipBeginMarker(reader);

            while (size > 0)
            {
                int h = reader.ReadInt32();
                size -= 4;

                string str = ReadString(reader, ref size);

                hash.Add(h, str);
            }

            return hash;
        }

        private string ReadString(BinaryReader reader, ref int size)
        {
            int zeros = 0;
            List<byte> str = new List<byte>();
            while (zeros < 4)
            {
                byte b = reader.ReadByte();
                if (b != 0)
                {
                    str.Add(b);
                }
                else
                {
                    zeros++;
                }
                size--;
            }

            return Encoding.UTF8.GetString(str.ToArray());
        }

        private void RaeadHeader(BinaryReader reader)
        {

            hashOffset = reader.ReadInt32();
            hashSize = reader.ReadInt32();
            frequencyOffset = reader.ReadInt32();
            frequencySize = reader.ReadInt32();
            distancesOffset = reader.ReadInt32();
            distancesSize = reader.ReadInt32();
            features = reader.ReadInt32();
        }

        private void SkipBeginMarker(BinaryReader reader)
        {
            for (int i = 0; i < 10; i++)
            {
                reader.ReadInt32();
            }
        }
    }
}
