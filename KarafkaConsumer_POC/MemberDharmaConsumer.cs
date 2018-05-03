using Dharma.Configurations;
using Dharma.MessageBroker.Kafka;
using MemberProducerSync.Utils;
using Microsoft.Extensions.Configuration;

namespace MemberConsumerSync
{
    public class MemberDharmaConsumer
    {
        public class MessageDharma : Dharma.MessageBroker.Message
        {
            public int Codigo { get; set; }
            public string Nome { get; set; }
        }

        public class ProducerConsumerDharma
        {

            public void Process()
            {
                var options = new MessageBrokerOptions();
                options.Host = ConfigHelper.Configuration.GetValue<string>("brokers");
                options.Port = 9094;
                options.Username = ConfigHelper.Configuration.GetValue<string>("user");
                options.Password = ConfigHelper.Configuration.GetValue<string>("password");
                options.SSL_CERT_KEY = ConfigHelper.Configuration.GetValue<string>("password");
                options.SSL = true;
                options.SASL_SCRAM = true;

                var message = new MessageDharma()
                {
                    Codigo = 1,
                    Nome = "teste"
                };

                var producer = new KafkaPublisher(options, "default");
                producer.Publish(message);

                var consumer = new KafkaSubscriber<MessageDharma>(options, "default", "teste");
                consumer.StartConsume(ProcessMessage);

                while (true)
                {
                }
            }

            private bool ProcessMessage(MessageDharma message)
            {
                return true;
            }
        }
    }
}