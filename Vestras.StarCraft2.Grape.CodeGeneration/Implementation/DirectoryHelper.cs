using System;
using System.IO;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class DirectoryHelper {
        public static void MakeFolderWritable(string folder) {
            if (IsFolderReadOnly(Path.GetDirectoryName(folder)) == true) {
                DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(folder));
                directory.Attributes = directory.Attributes & ~FileAttributes.ReadOnly;
            }
        }

        public static bool IsFolderReadOnly(string folder) {
            DirectoryInfo directory = new DirectoryInfo(folder);
            return ((directory.Attributes & FileAttributes.ReadOnly) > 0);
        }

        public static bool IsFileReadOnly(string file) {
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.IsReadOnly == true) {
                return fileInfo.IsReadOnly;
            }

            try {
                using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None)) {
                    try {
                        stream.ReadByte();
                        return false;
                    } catch (IOException) {
                        return true;
                    } finally {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            } catch (IOException) {
                return true;
            }
        }

        public static void CreatePath(string path) {
            if (Directory.Exists(Path.GetDirectoryName(path)) == false) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

        public static void CreateFolderPath(string path) {
            if (Directory.Exists(path) == false) {
                Directory.CreateDirectory(path);
            }
        }

        public static void DeleteDirectory(string path) {
            if (Directory.Exists(path) == true) {
                string[] files = Directory.GetFiles(path);
                string[] dirs = Directory.GetDirectories(path);

                Array.ForEach(files, file => {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                });

                Array.ForEach(dirs, dir => DeleteDirectory(dir));

                Directory.Delete(path, false);
            }
        }
    }
}
