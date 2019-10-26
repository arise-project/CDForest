using System.IO;

namespace CDSearchFile
{
    public class FileAnalyseChecker
    {
        int hashOffset;
        int hashSize;
        int frequencyOffset;
        int frequencySize;
        int distancesOffset;
        int distancesSize;
        int features;

        public bool Check(string fileName)
        {
            using (var reader = new BinaryReader(File.OpenRead(fileName)))
            {
                return CheckHeader(reader) && CheckHash(reader) && CheckFrequency(reader) && CheckDistances(reader);
            }
        }

        private bool CheckHeader(BinaryReader reader)
        {

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            hashOffset = reader.ReadInt32();

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            hashSize = reader.ReadInt32();

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            frequencyOffset = reader.ReadInt32();

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            frequencySize = reader.ReadInt32();

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            distancesOffset = reader.ReadInt32();

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            distancesSize = reader.ReadInt32();

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            if (hashOffset > frequencyOffset || frequencyOffset > distancesOffset)
            {
                return false;
            }

            if (hashSize < 40 || frequencySize < 40 || distancesSize < 40)
            {
                return false;
            }

            if (!reader.BaseStream.CanRead)
            {
                return false;
            }

            features = reader.ReadInt32();

            return true;
        }

        private bool CheckLength(BinaryReader reader, int size)
        {
            if (!SkipBeginMarker(reader))
            {
                return false;
            }

            int length = reader.ReadBytes(size).Length;
            if (length < size)
            {
                return false;
            }

            return true;
        }

        private bool SkipBeginMarker(BinaryReader reader)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!reader.BaseStream.CanRead || reader.ReadInt32() != i)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckHash(BinaryReader reader)
        {
            return CheckLength(reader, hashSize);
        }

        private bool CheckFrequency(BinaryReader reader)
        {
            return CheckLength(reader, frequencySize);
        }

        private bool CheckDistances(BinaryReader reader)
        {
            return CheckLength(reader, distancesSize);
        }
    }
}
