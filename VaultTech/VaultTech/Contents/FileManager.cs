/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 18/February/2014
 * Date Moddified :- 27/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace VaultTech.Contents
{
    public static class FileManager
    {
        public static readonly string ContentFolder = "./Content/";
        public static readonly string SourceArchiveFileName = "Content.data";
        public static readonly string SourceArchiveFilePath = ContentFolder + SourceArchiveFileName;
        public static readonly string DecompressedContentFolder = "DecompressedContent/";

        static MemoryStream ArchiveFileMemoryStream;
        static FileStream ArchiveStream;
        static ZipArchive ArchiveFile;

        /// <summary>
        /// Compresses everything inside the content folder and name the file Content.data.
        /// </summary>
        /// <param name="Overwrite">Whether or whether not to overwrite exiting Content.data file.</param>
        public static void CompressContent(bool Overwrite)
        {
            if (Overwrite)
            {
                if (File.Exists(SourceArchiveFilePath))
                    File.Delete(SourceArchiveFilePath);

                if (Directory.Exists(ContentFolder))
                {
                    ZipFile.CreateFromDirectory(ContentFolder, SourceArchiveFileName);
                    File.Move(SourceArchiveFileName, SourceArchiveFilePath);
                }
            }
            else
            {
                if (!File.Exists(SourceArchiveFilePath) && Directory.Exists(ContentFolder))
                {
                    ZipFile.CreateFromDirectory(ContentFolder, SourceArchiveFileName);
                    File.Move(SourceArchiveFileName, SourceArchiveFilePath);
                }
            }
        }

        public static Stream GetFileStreamFromArchive(string FilePathInArchive)
        {
            ArchiveStream = new FileStream(ContentFolder + SourceArchiveFileName, FileMode.Open);
            ArchiveFile = new ZipArchive(ArchiveStream);

            foreach (ZipArchiveEntry Entry in ArchiveFile.Entries)
                if (Entry.FullName == FilePathInArchive)
                    return Entry.Open();

            return null;
        }

        /// <summary>
        /// Read a specific file from an archive without decompressing it into the hard drive.
        /// </summary>
        /// <param name="FilePath">File path inside the archive.</param>
        public static Stream GetFileStreamFromArchive(string ArchiveFileName, string FilePathInArchive)
        {
            ArchiveStream = new FileStream(ContentFolder + ArchiveFileName, FileMode.Open);
            ArchiveFile = new ZipArchive(ArchiveStream);

            foreach (ZipArchiveEntry Entry in ArchiveFile.Entries)
                if (Entry.FullName == FilePathInArchive)
                    return Entry.Open();

            return null;
        }

        /// <summary>
        /// Takes an archive within an archive stream.
        /// </summary>
        /// <param name="ArchiveStream"></param>
        /// <param name="FilePath"></param>
        /// <returns>Returns a file from archive within an archive.</returns>
        public static Stream GetFileStreamFromArchive(Stream ArchiveStream, string FilePath)
        {
            ArchiveFile = new ZipArchive(ArchiveStream);

            foreach (ZipArchiveEntry Entry in ArchiveFile.Entries)
                if (Entry.FullName == FilePath)
                    return Entry.Open();

            return null;
        }

        public static MemoryStream GetFileMemoryStreamFromArchive(string FilePathInArchive)
        {
            ArchiveStream = new FileStream(ContentFolder + SourceArchiveFileName, FileMode.Open);
            ArchiveFile = new ZipArchive(ArchiveStream);

            foreach (ZipArchiveEntry Entry in ArchiveFile.Entries)
                if (Entry.FullName == FilePathInArchive)
                {
                    ArchiveFileMemoryStream = new MemoryStream();
                    Entry.Open().CopyTo(ArchiveFileMemoryStream);

                    return ArchiveFileMemoryStream;
                }

            return null;
        }

        public static MemoryStream GetFileMemoryStreamFromArchive(string ArchiveFileName, string FilePathInArchive)
        {
            ArchiveStream = new FileStream(ContentFolder + ArchiveFileName, FileMode.Open);
            ArchiveFile = new ZipArchive(ArchiveStream);

            foreach (ZipArchiveEntry Entry in ArchiveFile.Entries)
                if (Entry.FullName == FilePathInArchive)
                {
                    ArchiveFileMemoryStream = new MemoryStream();
                    Entry.Open().CopyTo(ArchiveFileMemoryStream);

                    return ArchiveFileMemoryStream;
                }

            return null;
        }

        /// <summary>
        /// Takes an archive within an archive stream.
        /// </summary>
        /// <param name="ArchiveStream">An archive within an archive stream.</param>
        /// <param name="FilePathInArchive"></param>
        /// <returns>Returns a file from archive within an archive.</returns>
        public static MemoryStream GetFileMemoryStreamFromArchive(Stream ArchiveStream, string FilePathInArchive)
        {
            ArchiveFile = new ZipArchive(ArchiveStream);

            foreach (ZipArchiveEntry Entry in ArchiveFile.Entries)
                if (Entry.FullName == FilePathInArchive)
                {
                    ArchiveFileMemoryStream = new MemoryStream();
                    Entry.Open().CopyTo(ArchiveFileMemoryStream);

                    return ArchiveFileMemoryStream;
                }

            return null;
        }

        public static void Dispose()
        {
            if (ArchiveStream != null)
            {
                ArchiveStream.Dispose();
                ArchiveStream = null;
            }

            if (ArchiveFile != null)
            {
                ArchiveFile.Dispose();
                ArchiveFile = null;
            }

            if (ArchiveFileMemoryStream != null)
            {
                ArchiveFileMemoryStream.Dispose();
                ArchiveFileMemoryStream = null;
            }
        }

        /// <summary>
        /// Decompress all the files in the archive into a specified directory.
        /// </summary>
        /// <param name="SourceArchiveFile">Archive that will be decompressed.</param>
        /// <param name="OutputDirectory">The directory where the archive will be decompressed in.</param>
        /// <param name="Overwrite">If the archive was already decompressed and exist, then it will overwrite the old version of the decompressed files.</param>
        public static void DecompressAll(string SourceArchiveFile, string OutputDirectory, bool Overwrite)
        {
            if (Overwrite && Directory.Exists(ContentFolder + OutputDirectory))
                Directory.Delete(ContentFolder + OutputDirectory, true);

            try
            {
                Directory.CreateDirectory(ContentFolder + OutputDirectory);
                ZipFile.ExtractToDirectory(ContentFolder + SourceArchiveFile, ContentFolder + OutputDirectory);
            }
            catch { }
        }

        /// <summary>
        /// If Content.data exist then it decompress the desired file inside the content folder.
        /// </summary>
        /// <param name="FilePath">File name to decompress from Content.data</param>
        /// <param name="Overwrite">If true and if desire file to decompress already decompressed and exist, then it will overwrite the old version of the file.</param>
        public static void DecompressFile(string FilePath, bool Overwrite)
        {
            if (Overwrite)
                if (File.Exists(DecompressedContentFolder + FilePath))
                    File.Delete(DecompressedContentFolder + FilePath);

            if (!File.Exists(DecompressedContentFolder + FilePath))
                if (File.Exists(SourceArchiveFilePath))
                    using (ZipArchive zip = ZipFile.Open(SourceArchiveFilePath, ZipArchiveMode.Read))
                        foreach (ZipArchiveEntry entry in zip.Entries)
                            if (entry.FullName.Replace('\\', '/') == FilePath)
                            {
                                string FilteredDirectory = entry.FullName;
                                FilteredDirectory = FilteredDirectory.Replace('\\', '/');
                                FilteredDirectory = FilteredDirectory.Substring(0, FilteredDirectory.LastIndexOf('/'));
                                FilteredDirectory = DecompressedContentFolder + FilteredDirectory;

                                if (!Directory.Exists(FilteredDirectory))
                                    Directory.CreateDirectory(FilteredDirectory);

                                entry.ExtractToFile(DecompressedContentFolder + entry.FullName);
                                break;
                            }
        }

        /// <summary>
        /// If Content.data exist then it decompress the desired directory inside the content folder.
        /// </summary>
        /// <param name="DirectoryPath">Directory name to decompress from Content.data</param>
        /// <param name="Overwrite">If true and if desire directory to decompress already decompressed and exist, then it will overwrite the old version of the file.</param>
        public static void DecompressDirectory(string DirectoryPath, bool Overwrite)
        {
            if (Overwrite)
                if (Directory.Exists(DecompressedContentFolder + DirectoryPath))
                    Directory.Delete(DecompressedContentFolder + DirectoryPath, true);

            if (!File.Exists(DecompressedContentFolder + DirectoryPath))
                if (File.Exists(SourceArchiveFilePath))
                    using (ZipArchive zip = ZipFile.Open(SourceArchiveFilePath, ZipArchiveMode.Read))
                        foreach (ZipArchiveEntry entry in zip.Entries)
                            if (entry.FullName.Replace('\\', '/').Contains(DirectoryPath))
                            {
                                string FilteredDirectory = entry.FullName;
                                FilteredDirectory = FilteredDirectory.Replace('\\', '/');
                                FilteredDirectory = FilteredDirectory.Substring(0, FilteredDirectory.LastIndexOf('/'));
                                FilteredDirectory = DecompressedContentFolder + FilteredDirectory;

                                if (!Directory.Exists(FilteredDirectory))
                                    Directory.CreateDirectory(FilteredDirectory);

                                if (entry.FullName.Contains('.'))
                                    entry.ExtractToFile(DecompressedContentFolder + entry.FullName);
                            }
        }

        /// <summary>
        /// Deletes the decompressed content directory and everything inside it.
        /// </summary>
        public static void DeleteDecompressedDirectory()
        {
            Directory.Delete(DecompressedContentFolder, true);
        }
    }
}