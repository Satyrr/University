using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zad4
{
    class Program
    {
        static private int freeSeats, totalSeats;
        static private int clientNumber, clientsLeft;
        private static Semaphore custReady, barberReady, seatsAval;

        static private void Cut(string client)
        {
            int res = 0;
            for (int i = 0; i < 100000; i++)
            {
                if (i % 12000 == 0) Console.WriteLine("client {0}: {1}% done", client, i / 1000.0);
                res += i;
            }
            Console.WriteLine("client {0}: 100% done", client);
        }

        static private void ClientProc(object name)
        {
            int doneFlag = 0;

            while(doneFlag != 1)
            {
                seatsAval.WaitOne();

                if(freeSeats > 0)
                {
                    freeSeats--;
                    custReady.Release();
                    seatsAval.Release();
                    barberReady.WaitOne();

                    Cut(name.ToString());
                    doneFlag = 1;
                }
                else
                {
                    seatsAval.Release();
                }
            }
        }

        static private void BarberProc()
        {
            while (clientsLeft > 0)
            {
                custReady.WaitOne();
                seatsAval.WaitOne();
                freeSeats++;
                clientsLeft--;
                barberReady.Release();
                seatsAval.Release();
            }
        }

        static void Main(string[] args)
        {
            clientNumber = clientsLeft = 40;
            totalSeats = clientNumber / 2;
            freeSeats = totalSeats;

            barberReady = new Semaphore(0, 1);
            custReady = new Semaphore(0, totalSeats);
            seatsAval = new Semaphore(1, 1);

            Thread barber = new Thread(new ThreadStart(BarberProc));
            barber.Start();

            Thread[] clients = new Thread[clientNumber];
            for (int i = 1; i <= clientNumber; i++)
            {
                clients[i - 1] = new Thread(new ParameterizedThreadStart(ClientProc));
                clients[i - 1].Start("client nr " + i.ToString());
            }

            barber.Join();
            for (int i = 0; i < clientNumber; i++)
                clients[i].Join();

            Console.Read();
        }
    }
}
