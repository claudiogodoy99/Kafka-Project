using System;
using System.Collections.Generic;

namespace Consumidor
{
    class Program
    {

        private static List<StackConsumer> stackConsumers;
        const string bootstrapServer = "127.0.0.1:9092";

        static void Main(string[] args)
        {
            stackConsumers = new List<StackConsumer>();

            showMenu();

            var optMenu = Console.ReadLine();

            try
            {
                while (optMenu != "0")
                {
                    Console.Clear();

                    switch (optMenu)
                    {
                        case "1":
                            createNewConsumer();
                            
                            break;
                        case "2":
                            startConsumer();
                            break;
                        case "3":
                            listConsumers();
                            
                            break;
                        case "4":
                            stopConsumer();
                            break;
                        case "5":
                            showDataRecieved();
                            break;
                        default:

                            break;
                    }
                    Console.Clear();
                    showMenu();
                    optMenu = Console.ReadLine();
                }
            }
            catch 
            {
                Console.Clear();
                Console.WriteLine("Something happend!");
            }
            finally{
                stackConsumers?.Clear();
                GC.SuppressFinalize(stackConsumers);
            }

        }

        private static void showMenu()
        {
            Console.WriteLine(@"
             =================Consumer Pool===================
             0 - Exit;
             1 - create new Consumer;
             2 - start consumer;
             3 - List Consumers;
             4 - stop a consumer;
             5 - show data recieved;
            ");
        }



        private async static void createNewConsumer()
        {
            Console.WriteLine("id: ");
            var id = Console.ReadLine();

            Console.WriteLine("groupId: ");
            var groupId = Console.ReadLine();

            Console.WriteLine("topic: ");
            var topic = Console.ReadLine();

            stackConsumers.Add(new StackConsumer
            {
                id = id,
                consumerWorker = new ConsumidorWorker(bootstrapServer, groupId, topic)
            });
        }

        private static async void startConsumer()
        { 
            findConsumerById((cons) => cons.startConsuming());
        }

        private static void listConsumers()
        {
            stackConsumers.ForEach(item =>
            {
                Console.WriteLine($"ID: {item.id} | STATUS: {item.consumerWorker.Status}");
            });

            Console.Read(); 
        }

        private static void stopConsumer()
        {
            findConsumerById((cons) => cons.stopConsuming());
        }

        private static void showDataRecieved()
        {
            findConsumerById((cons) => cons.logAllDataReaded());
        }

        private static void findConsumerById(Action<ConsumidorWorker> action) 
        {
            Console.WriteLine("ID: ");
            string id = Console.ReadLine();

            var worker = stackConsumers.Find(st => st.id == id);
            

            if (worker != null)
            {
                action.Invoke(worker.consumerWorker);
            }
            else 
            {
                Console.WriteLine("ID NOT FOUND");
            }

            Console.ReadKey();
        }


    }

  
}
