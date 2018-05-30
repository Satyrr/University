using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad4
{
    class Program
    {
        static void Main(string[] args)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo("./");
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


            long filesLength = fileList.Select(f => f.Length).Aggregate((x, y) => x + y);

            Console.WriteLine(filesLength);

            Console.ReadLine();
        }
    }
}
