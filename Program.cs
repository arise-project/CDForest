using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp4
{
    class Program
    {
        public class CDForest
        {
            public List<CDInfo> Disks { get; set; }
        }

        public class CDInfo
        {
            public int DiskId { get; set; }

            public string Name { get; set; }

            public List<CDDirectoryInfo> Directories { get; set; }
        }

        public class CDFileInfo
        {
            public string Name { get; set; }

            public string Extension { get; set; }

            public string Path { get; set; }

            public DateTime Created { get; set; }

            public DateTime? Modified { get; set; }

            public long Size { get; set; }
        }

        public class CDDirectoryInfo
        {
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

        static void Main(string[] args)
        {
            CDForest f = new CDForest();
            string fileName = "forest.json";
            if(args.Length == 1 && File.Exists(args[0]))
            {
                fileName = args[0];
                
            }

            if(File.Exists(fileName))
            {
                f = JsonSerializer.Deserialize<CDForest>(File.ReadAllText(fileName));
            }

            f.Disks = f.Disks ?? new List<CDInfo>();

            DriveInfo[] theCollectionOfDrives = DriveInfo.GetDrives();
            foreach (DriveInfo curDrive in theCollectionOfDrives)
            {
                if (curDrive.DriveType == DriveType.CDRom)
                {
                    if (curDrive.IsReady)
                    {
                        var diskName = curDrive.Name;

                        CDInfo disk = new CDInfo { Name = curDrive.Name, Directories = new List<CDDirectoryInfo>() };

                        f.Disks.Add(disk);

                        Stack<DirectoryInfo> directories = new Stack<DirectoryInfo>();

                        Dictionary<string, CDDirectoryInfo> cache = new Dictionary<string, CDDirectoryInfo>();

                        foreach (var dir in curDrive.RootDirectory.GetDirectories())
                        {
                            directories.Push(dir);
                            CDDirectoryInfo directoryItem = new CDDirectoryInfo { Created = dir.CreationTimeUtc, Modified = dir.LastWriteTimeUtc, Name = dir.Name, Path = dir.FullName, Files = new List<CDFileInfo>(), Directories = new List<CDDirectoryInfo>(), RelatedDiscNames = new List<string>(), RelatedDiscs = new List<int>() };
                            disk.Directories.Add(directoryItem);
                        }

                        while (directories.Count > 0)
                        {
                            var dir = directories.Pop();
                            CDDirectoryInfo directoryItem = cache.ContainsKey(dir.FullName) ? 
                                cache[dir.FullName] : 
                                new CDDirectoryInfo { Created = dir.CreationTimeUtc, Modified = dir.LastWriteTimeUtc, Name = dir.Name, Path = dir.FullName, Files = new List<CDFileInfo>(), Directories = new List<CDDirectoryInfo>(), RelatedDiscNames = new List<string>(), RelatedDiscs = new List<int>() };
                            
                            cache.Add(dir.FullName, directoryItem);
                            FileInfo[] files = curDrive.RootDirectory.GetFiles("*", SearchOption.TopDirectoryOnly);

                            long size = 0;
                            foreach (var file in files)
                            {
                                CDFileInfo fileItem = new CDFileInfo { Created = file.CreationTimeUtc, Extension = file.Extension, Modified = file.LastWriteTimeUtc, Name = file.Name, Path = file.FullName, Size = file.Length };
                                size += file.Length;
                                directoryItem.Files.Add(fileItem);
                            }

                            directoryItem.Size = size;

                            if(cache.ContainsKey(dir.Parent?.FullName))
                            {
                                cache[dir.Parent?.FullName].Directories.Add(directoryItem);
                            }

                            foreach(var subDir in dir.GetDirectories())
                            {
                                directories.Push(subDir);
                            }
                        }
                    }
                }
            }

            File.WriteAllText(fileName, JsonSerializer.Serialize<CDForest>(f));
        }
    }
}
