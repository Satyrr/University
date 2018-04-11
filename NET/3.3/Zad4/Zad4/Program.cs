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
        static private Object mutex, custReady, barberReady, seatsAval;

        static private void Cut(string client)
        {
            int res = 0;
            for (int i = 0; i < 100000; i++)
            {
                if (i % 12000 == 0) Console.WriteLine("client {0}: {1} done", client, i / 100000.0);
                res += i;
            }
            Console.WriteLine("client {0}: 100% done", client);
        }

        static private void ClientProc(object name)
        {
            int doneFlag = 0;

            lock (mutex)
            {
                while (doneFlag != 1)
                {
                    if (freeSeats > 0)
                    {
                        freeSeats--;
                        //notify barber about ready customer
                        Monitor.Pulse(custReady);

                        //wait for cut
                        Monitor.Wait(barberReady);

                        Cut(name.ToString());
                        doneFlag = 1;
                    }
                    else
                    {
                        Monitor.Wait(seatsAval);
                    }
                }
                
            }
        }

        static private void BarberProc()
        {
            while(clientsLeft > 0)
            {
                lock(mutex)
                {
                    if(freeSeats == totalSeats) Monitor.Wait(custReady);
                    freeSeats++;
                    clientsLeft--;
                    Monitor.Pulse(barberReady);
                    if (freeSeats > 0) Monitor.Pulse(seatsAval);
                }
            }
        }

        static void Main(string[] args)
        {
            clientNumber = clientsLeft = 40;
            totalSeats = clientNumber / 2;
            freeSeats = totalSeats;

            mutex = new object();
            barberReady = new object();
            custReady = new object();
            seatsAval = new object();

            Thread barber = new Thread(new ThreadStart(BarberProc));
            barber.Start();

            Thread[] clients = new Thread[clientNumber];
            for(int i = 1; i <= clientNumber; i++)
            {
                clients[i - 1] = new Thread(new ParameterizedThreadStart(ClientProc));
                clients[i - 1].Start("client nr " + i.ToString());
            }

            barber.Join();
            for (int i = 0; i < clientNumber; i++)
                clients[i].Join();

        }
    }
}
