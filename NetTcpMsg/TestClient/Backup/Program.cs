using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using NTCPMSG.Client;
using NTCPMSG.Event;
namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int nIsContinue = 0;
            do 
            { 
                Console.Write("Test SingleConnectionCable choose 0, Test SingelCOnnection choose 1. [0]");
                int option;

                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    option = 0;
                }

                if (option == 1)
                {
                    TestSingleConnection.Test(args);
                }
                else
                {
                    TestSingleConnectionCable.Test(args);
                }

                               
                //Is Continue
                Console.Write("Is Continue? 1 or 0.[0]");
                 if (!int.TryParse(Console.ReadLine(), out nIsContinue))
                {
                    nIsContinue = 0;
                }

            } while (nIsContinue != 0);


        }
    }
}
