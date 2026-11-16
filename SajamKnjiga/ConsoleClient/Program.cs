using System;
using Core.Models;
using ConsoleClient.Controllers;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
          

            ConsoleController controller = new ConsoleController();
            controller.Start();
        }
    }
}