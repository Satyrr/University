using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace Zad2
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordApp = new Word.Application();
            wordApp.Visible = true;
            Word.Document wordDoc = wordApp.Documents.Add();
            wordDoc.Content.Text = "Programowanie pod Windows";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            wordDoc.SaveAs2(path + @"\filename.doc");
        }

    }
}
