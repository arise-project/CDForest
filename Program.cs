using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp4
{
    class Program
    {
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
            Console.WriteLine("Hello World!");

            DriveInfo[] theCollectionOfDrives = DriveInfo.GetDrives();
            foreach (DriveInfo curDrive in theCollectionOfDrives)
            {
                if (curDrive.DriveType == DriveType.CDRom)
                {
                    if (curDrive.IsReady)
                    {
                        var diskName = curDrive.Name;

                        CDInfo disk = new CDInfo { Name = curDrive.Name };

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
        }
    }
}
