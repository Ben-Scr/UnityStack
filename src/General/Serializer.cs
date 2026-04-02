using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Json;
using System;
using System.IO;
using System.IO.Compression;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


namespace BenScr.UnityStack
{
    /*
     * Used for saving and loading data in JSON and binary formats
     */

    public static class Json
    {
        public static void Serialize<T>(string path, T obj)
        {
            EnsureDir(path);

            string json = JsonSerialization.ToJson(obj);
            File.WriteAllText(path, json);
        }
        public static T Deserialize<T>(string path, T defaulValue = default)
        {
            try
            {
                string json = File.ReadAllText(path);
                return JsonSerialization.FromJson<T>(json);
            }
            catch(Exception ex)
            {
                Logger.Message(ex.Message);
                return defaulValue;
            }
        }
        public static T DeserializeFromJson<T>(string json, T defaulValue = default)
        {
            try
            {
                return JsonSerialization.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                Logger.Message(ex.Message);
                return defaulValue;
            }
        }

        private static void EnsureDir(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }

    public static class Binary
    {
        public static void Serialize<T>(string path, T[] data) where T : unmanaged
        {
            string? dirPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            if (data == null || data.Length == 0)
            {
                using var emptyFs = new FileStream(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None);
                return;
            }

            ReadOnlySpan<T> span = data;
            ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(span);

            using var fs = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                options: FileOptions.SequentialScan);

            fs.Write(bytes);
        }

        public static T[] Deserialize<T>(string path) where T : unmanaged
        {
            if (!File.Exists(path))
                return Array.Empty<T>();

            try
            {
                byte[] fileBytes = File.ReadAllBytes(path);
                if (fileBytes.Length == 0)
                    return Array.Empty<T>();

                int elementSize = Unsafe.SizeOf<T>();

                if (fileBytes.Length % elementSize != 0)
                {
                    int usableBytes = fileBytes.Length / elementSize * elementSize;
                    if (usableBytes == 0)
                        return Array.Empty<T>();

                    fileBytes = fileBytes.AsSpan(0, usableBytes).ToArray();
                }

                int count = fileBytes.Length / elementSize;
                T[] result = new T[count];

                Span<byte> byteSpan = fileBytes;
                Span<T> resultSpan = result;

                MemoryMarshal.Cast<byte, T>(byteSpan).CopyTo(resultSpan);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Message(ex.Message);
                return Array.Empty<T>();
            }
        }
    }
}
