using Confluent.Kafka;
using System;

namespace Consumidor
{
    class Program
    {
        static void Main(string[] args)
        {
            const string bootstrapServer = "127.0.0.1:9092";
            const string groupId = "Grupo-Cliente-Send-Email";
            const string topic = "cliente";

            ConsumerConfig config = new ConsumerConfig()
            {
                BootstrapServers = bootstrapServer,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            ConsumerBuilder<string, string> consumerBuilder = new ConsumerBuilder<string, string>(config);

            var consumer = consumerBuilder.Build();
            consumer.Subscribe(topic);

            while (true)
            {
                var record = consumer.Consume(12);
                if(record != null)
                Console.WriteLine($"key: {record.Message.Key} \n  Value: {record.Message.Value} \n  Offset: {record.Offset} \n Partition : {record.Partition}");
            }


        }
    }
}
