using System;
using System.Collections.Generic;
using System.IO;

namespace SelectPython.Core
{
    public class PythonDetector
    {
        public static List<String> Find()
        {
            var drives = DriveInfo.GetDrives();
            var pythonPaths = new List<String>();
            foreach (var d in drives)
            {
                if (d.DriveType == DriveType.Fixed)
                {
                    WalkDirsTree(d.RootDirectory, pythonPaths);
                }
            }
            return pythonPaths;
        }

        private static void WalkDirsTree(DirectoryInfo path, List<String> pythonPaths)
        {
            var subDirs = new DirectoryInfo[] { };
            try
            {
                FindPython(path, pythonPaths);
                subDirs = path.GetDirectories();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("No access to " + path.FullName);
            }

            foreach (var di in subDirs)
            {
                WalkDirsTree(di, pythonPaths);
            }
        }

        private static void FindPython(DirectoryInfo path, List<String> pythonPaths)
        {
            var files = path.GetFiles("python.exe");
            if (files.Length > 0)
            {
                pythonPaths.Add(path.FullName);
            }
        }
    }
}
