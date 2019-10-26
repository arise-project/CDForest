using CDSearchFile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace CDForestFull
{
    class Program
    {
        public class CDForest
        {
            public List<CDInfo> Disks { get; set; }
        }

        public class CDInfo
        {
            public string Name { get; set; }

            public uint Sernum { get; set; }

            public List<CDDirectoryInfo> Directories { get; set; }
        }

        public class CDFileInfo
        {
            public CDFileInfo()
            {

            }

            public CDFileInfo(FileInfo file)
            {
                Created = file.CreationTimeUtc;
                Extension = file.Extension;
                Modified = file.LastWriteTimeUtc;
                Name = file.Name;
                Path = file.FullName;
                Size = file.Length;
            }

            public string Name { get; set; }

            public string Extension { get; set; }

            public string Path { get; set; }

            public DateTime Created { get; set; }

            public DateTime? Modified { get; set; }

            public long Size { get; set; }

            public string SearchFile { get; set; }

            public string Hash { get; set; }
        }

        public class CDDirectoryInfo
        {
            public CDDirectoryInfo()
            {

            }

            public CDDirectoryInfo(DirectoryInfo dir)
            {
                Created = dir.CreationTimeUtc;
                Modified = dir.LastWriteTimeUtc;
                Name = dir.Name;
                Path = dir.FullName;
                Files = new List<CDFileInfo>();
                Directories = new List<CDDirectoryInfo>();
                RelatedDiscNames = new List<string>();
                RelatedDiscs = new List<int>();
            }

            public string Name { get; set; }

            public string Path { get; set; }

            public DateTime Created { get; set; }

            public DateTime? Modified { get; set; }

            public long Size { get; set; }

            public List<CDFileInfo> Files { get; set; }

            public List<CDDirectoryInfo> Directories { get; set; }

            public List<int> RelatedDiscs { get; set; }

            public List<string> RelatedDiscNames { get; set; }
        }

        [Flags]
        public enum FileSystemFeature : uint
        {
            /// <summary>
            /// The file system preserves the case of file names when it places a name on disk.
            /// </summary>
            CasePreservedNames = 2,

            /// <summary>
            /// The file system supports case-sensitive file names.
            /// </summary>
            CaseSensitiveSearch = 1,

            /// <summary>
            /// The specified volume is a direct access (DAX) volume. This flag was introduced in Windows 10, version 1607.
            /// </summary>
            DaxVolume = 0x20000000,

            /// <summary>
            /// The file system supports file-based compression.
            /// </summary>
            FileCompression = 0x10,

            /// <summary>
            /// The file system supports named streams.
            /// </summary>
            NamedStreams = 0x40000,

            /// <summary>
            /// The file system preserves and enforces access control lists (ACL).
            /// </summary>
            PersistentACLS = 8,

            /// <summary>
            /// The specified volume is read-only.
            /// </summary>
            ReadOnlyVolume = 0x80000,

            /// <summary>
            /// The volume supports a single sequential write.
            /// </summary>
            SequentialWriteOnce = 0x100000,

            /// <summary>
            /// The file system supports the Encrypted File System (EFS).
            /// </summary>
            SupportsEncryption = 0x20000,

            /// <summary>
            /// The specified volume supports extended attributes. An extended attribute is a piece of
            /// application-specific metadata that an application can associate with a file and is not part
            /// of the file's data.
            /// </summary>
            SupportsExtendedAttributes = 0x00800000,

            /// <summary>
            /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
            /// </summary>
            SupportsHardLinks = 0x00400000,

            /// <summary>
            /// The file system supports object identifiers.
            /// </summary>
            SupportsObjectIDs = 0x10000,

            /// <summary>
            /// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
            /// </summary>
            SupportsOpenByFileId = 0x01000000,

            /// <summary>
            /// The file system supports re-parse points.
            /// </summary>
            SupportsReparsePoints = 0x80,

            /// <summary>
            /// The file system supports sparse files.
            /// </summary>
            SupportsSparseFiles = 0x40,

            /// <summary>
            /// The volume supports transactions.
            /// </summary>
            SupportsTransactions = 0x200000,

            /// <summary>
            /// The specified volume supports update sequence number (USN) journals. For more information,
            /// see Change Journal Records.
            /// </summary>
            SupportsUsnJournal = 0x02000000,

            /// <summary>
            /// The file system supports Unicode in file names as they appear on disk.
            /// </summary>
            UnicodeOnDisk = 4,

            /// <summary>
            /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
            /// </summary>
            VolumeIsCompressed = 0x8000,

            /// <summary>
            /// The file system supports disk quotas.
            /// </summary>
            VolumeQuotas = 0x20
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool GetVolumeInformation(
                                                            string rootPathName,
                                                            StringBuilder volumeNameBuffer,
                                                            int volumeNameSize,
                                                            out uint volumeSerialNumber,
                                                            out uint maximumComponentLength,
                                                            out FileSystemFeature fileSystemFlags,
                                                            StringBuilder fileSystemNameBuffer,
                                                            int nFileSystemNameSize);


        static void Main(string[] args)
        {
            CDForest forest = new CDForest();
            string fileName = "forest.json";
            if (args.Length == 1 && File.Exists(args[0]))
            {
                fileName = args[0];
            }

            if (File.Exists(fileName))
            {
                forest = JsonConvert.DeserializeObject<CDForest>(File.ReadAllText(fileName));
            }

            forest.Disks = forest.Disks ?? new List<CDInfo>();

            DriveInfo[] theCollectionOfDrives = DriveInfo.GetDrives();

            var filter = new ApacheTikaFilter();

            foreach (DriveInfo curDrive in theCollectionOfDrives)
            {
                if (curDrive.DriveType == DriveType.CDRom)
                {
                    if (curDrive.IsReady)
                    {
                        var diskName = curDrive.Name;

                        StringBuilder volname = new StringBuilder(261);
                        StringBuilder fsname = new StringBuilder(261);
                        if (!GetVolumeInformation(
                            curDrive.RootDirectory.FullName,
                            volname,
                            volname.Capacity,
                            out uint sernum,
                            out uint maxlen,
                            out FileSystemFeature flags,
                            fsname,
                            fsname.Capacity))
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                        string volnamestr = volname.ToString();
                        string fsnamestr = fsname.ToString();

                        CDInfo disk = new CDInfo
                        {
                            Name = curDrive.VolumeLabel,
                            Sernum = sernum,
                            Directories = new List<CDDirectoryInfo>()
                        };

                        var existing = forest.Disks.FirstOrDefault(d => d.Sernum == sernum);
                        if (existing != null)
                        {
                            forest.Disks.Remove(existing);
                        }

                        forest.Disks.Add(disk);

                        Stack<DirectoryInfo> directories = new Stack<DirectoryInfo>();

                        Dictionary<string, CDDirectoryInfo> cache = new Dictionary<string, CDDirectoryInfo>();

                        foreach (var dir in curDrive.RootDirectory.GetDirectories())
                        {
                            directories.Push(dir);
                            CDDirectoryInfo directoryItem = new CDDirectoryInfo(dir);
                            disk.Directories.Add(directoryItem);
                            cache.Add(dir.FullName, directoryItem);
                        }

                        while (directories.Count > 0)
                        {
                            var dir = directories.Pop();
                            CDDirectoryInfo directoryItem;
                            if (cache.ContainsKey(dir.FullName))
                            {
                                directoryItem = cache[dir.FullName];
                            }
                            else
                            {
                                directoryItem = new CDDirectoryInfo(dir);
                                cache.Add(dir.FullName, directoryItem);
                            }

                            FileInfo[] files = dir.GetFiles("*", SearchOption.TopDirectoryOnly);

                            long size = 0;
                            foreach (var file in files)
                            {
                                CDFileInfo fileItem = new CDFileInfo(file);
                                size += file.Length;
                                directoryItem.Files.Add(fileItem);

                                if (filter.IsTextFile(fileItem.Path))
                                {
                                    byte[] hash;
                                    using (var md5 = MD5.Create())
                                    {
                                        using (var stream = File.OpenRead(fileItem.Path))
                                        {
                                            hash = md5.ComputeHash(stream);
                                        }
                                    }
                                    string outputHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                                    string output = Path.GetFileName(fileItem.Path) + ".search." + outputHash;

                                    fileItem.Hash = outputHash;

                                    if (!File.Exists(output))
                                    {
                                        var result = new FileAnalyser(fileItem.Path).Parse(20).FilterMedian(500);

                                        if (result != null)
                                            fileItem.SearchFile = new FileAnalyseWriter().WriteToFile(
                                                fileItem.Path,
                                                result,
                                                hash,
                                                output,
                                                Features.Distances | Features.Frequency | Features.Hash | Features.MedianFilter);
                                    }
                                }
                            }

                            directoryItem.Size = size;

                            if (cache.ContainsKey(dir.Parent?.FullName))
                            {
                                cache[dir.Parent?.FullName].Directories.Add(directoryItem);
                            }

                            foreach (var subDir in dir.GetDirectories())
                            {
                                directories.Push(subDir);
                            }
                        }
                    }
                }
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(forest, Formatting.Indented));
        }
    }
}
