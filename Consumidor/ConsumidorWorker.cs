using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Consumidor
{
    public class ConsumidorWorker
    {

        private readonly ConsumerConfig config;
        private readonly ConsumerBuilder<string, string> consumerBuilder;
        private string topic;
        private IConsumer<string, string> consumer;
        private readonly List<string> dataReaded;
        public string Status = "0";

        public ConsumidorWorker(
            string bootstrapServer,
            string groupId,
            string topic)
        {

            config = new ConsumerConfig()
            {
                BootstrapServers = bootstrapServer,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            consumerBuilder = new ConsumerBuilder<string, string>(config);

            this.topic = topic;

            consumer = consumerBuilder.Build();

            dataReaded = new List<string>();
        }

        public async void startConsuming()
        {
            if (Status == "0")
            {
                Status = "1";

                consumer.Subscribe(topic);

                Task.Run(() =>
                {
                    while (Status == "1")
                    {
                        var record = consumer.Consume(12);
                        if (record != null)
                            dataReaded.Add($"key: {record.Message.Key} \n  Value: {record.Message.Value} \n  Offset: {record.Offset} \n Partition : {record.Partition}");

                    }
                }
               );


            }

        }

        public void logAllDataReaded()
        {
            lock (dataReaded)
            {
                dataReaded.ForEach(str => Console.WriteLine(str));
            }
        }

        public void stopConsuming()
        {
            Status = "0";

            lock (consumer) 
            {
                consumer.Unsubscribe();
            }
        }
    }
}
