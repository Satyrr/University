using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    class Program
    {
        private static void Compress(string toCompress, string compressed)
        {
            FileStream F = new FileStream(toCompress, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            using (FileStream compressedFile = File.Create(compressed))
            {
                using (GZipStream G = new GZipStream(compressedFile, CompressionMode.Compress))
                {
                    F.CopyTo(G);
                }
            }

            F.Close();
        }

        private static void Uncompress(string compressed, string uncompressed)
        {
            FileInfo fileToDecompress = new FileInfo(compressed);
            using (FileStream compressedFileStream = fileToDecompress.OpenRead())
            {
                using (FileStream decompressedFileStream = File.Create(uncompressed))
                {
                    using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Compress("text.txt", "textcompressed.gz");
            Uncompress("textcompressed.gz", "uncompressed.txt");
            
            Console.ReadKey();
        }
    }
}
