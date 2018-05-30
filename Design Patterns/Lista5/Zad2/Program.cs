using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Zad2
{
    class CaesarStream : Stream, IDisposable
    {
        int _shift;
        Stream _stream;

        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CaesarStream(Stream stream, int shift)
        {
            _shift = shift;
            _stream = stream;
        }

        public void Write(byte[] bytes)
        {
            foreach(byte b in bytes)
            {
                _stream.WriteByte((byte)(b + _shift));
            }
            _stream.Flush();
            
        }

        public byte[] Read()
        {
            List<byte> bytes = new List<byte>();

            int readen = _stream.ReadByte();
            while(readen != -1)
            {
                bytes.Add((byte)(readen+_shift));
                readen = _stream.ReadByte();
            }
            return bytes.ToArray();
        }

        public new void Dispose()
        {
            _stream.Dispose();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (CaesarStream cs = new CaesarStream(File.Create("text.txt"), 10))
            {
                cs.Write(Encoding.ASCII.GetBytes("abcd"));
            }

            using (CaesarStream cs = new CaesarStream(File.OpenRead("text.txt"), -10))
            {
                Console.WriteLine(Encoding.ASCII.GetString(cs.Read()));
            }
            
            Console.Read();
        }
    }
}
