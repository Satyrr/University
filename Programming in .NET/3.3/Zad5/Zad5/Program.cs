using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zad5
{
    class NetworkRequests
    {
        public FtpWebResponse FtpRequest(string uri)
        {
            FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(uri);
            fwr.Method = WebRequestMethods.Ftp.ListDirectory;

            FtpWebResponse response = (FtpWebResponse)fwr.GetResponse();

            return response;
        }
        public HttpWebResponse HttpRequest(string uri)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(uri);
            hwr.Method = WebRequestMethods.Http.Get;

            HttpWebResponse response = (HttpWebResponse)hwr.GetResponse();

            return response;
        }
    }
    class Program
    {
        private static void printResponse(Stream str, int max)
        {
            int c, printed = 0;
            while ((c = str.ReadByte()) != -1 && printed < max)
            {
                Console.Write((char)c);
                printed++;
            }
                
        }


        static void Main(string[] args)
        {
            NetworkRequests nr = new NetworkRequests();

            FtpWebResponse lsResponse = nr.FtpRequest("ftp://ftp.icm.edu.pl/");

            Console.WriteLine("lsResponse: " + lsResponse.StatusDescription);
            Stream s = lsResponse.GetResponseStream();
            printResponse(s, 300);

            HttpWebResponse getResponse = nr.HttpRequest("http://www.ii.uni.wroc.pl/~wzychla/");
            Console.WriteLine("\ngetResponse: " + getResponse.StatusDescription);
            s = getResponse.GetResponseStream();
            printResponse(s, 300);



            Console.ReadLine();
        }
    }
}
