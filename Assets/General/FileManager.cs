using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace BenScr.UnityStack
{
    public static class FileManager
    {
        public enum MemoryUnit : ulong
        {
            Bit = 1,
            Byte = 1,
            KiloByte = 1024,
            MegaByte = 1024 * 1024,
            GigaByte = 1024 * 1024 * 1024,
        }

        public enum Extension
        {
            txt, json, dat, bin
        }

        public static string GetUnityPersistentDataFolder()
        {
            return Application.persistentDataPath;
        }
        public static string GetUnityDataFolder()
        {
            return Application.dataPath;
        }

        public static string GetLocalAppDataFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public static string FromLocalAppDataFolder(params string[] paths) => Path.Combine(GetLocalAppDataFolder(), Path.Combine(paths));
        public static string CombinePathWithExtension(Extension extension, params string[] paths) => Path.ChangeExtension(Path.Combine(paths), extension.ToString());
        public static string RemoveLastPathSegment(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            string trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string parent = Path.GetDirectoryName(trimmed) ?? string.Empty;

            if (string.IsNullOrEmpty(parent) && Path.IsPathRooted(trimmed))
                return Path.GetPathRoot(trimmed) ?? string.Empty;

            return parent;
        }

        public static double GetFileSize(string path, MemoryUnit memoryUnit)
        {
            if (File.Exists(path))
            {
                long fileSizeInBytes = new FileInfo(path).Length;
                return fileSizeInBytes / (double)memoryUnit;
            }
            else
            {
                throw new FileNotFoundException($"File at path ({path}) doesn't Exist");
            }
        }

        public static void DeleteAllFromDirectory(string directoryPath, bool onlyFiles)
        {
            if (Directory.GetDirectories(directoryPath).Length > 0)
                DeleteAllDirectories(directoryPath, onlyFiles);

            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
        public static void DeleteAllDirectories(string path, bool onlyFiles)
        {
            try
            {
                string[] directories = Directory.GetDirectories(path);

                foreach (string directory in directories)
                {
                    DeleteAllFromDirectory(directory, false);

                    if (!onlyFiles)
                        Directory.Delete(directory);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static string[] GetDirectoryFiles(string dirPath, bool recursive = false) => recursive ? GetDirectoryFilesRecursive(dirPath) : Directory.GetFiles(dirPath);
        private static string[] GetDirectoryFilesRecursive(string dirPath)
        {
            List<string> files = new List<string>();

            foreach (var dir in Directory.GetDirectories(dirPath))
            {
                files.AddRange(GetDirectoryFiles(dir));
            }

            return files.ToArray();
        }
    }
}
