﻿using System;
using AttackOrDefenseServer.Servers;

namespace AttackOrDefenseServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 6688);
            server.Start();
            Console.ReadKey();
        }
    }
}
