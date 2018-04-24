using System;
using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Events;
using MemberProducerSync.Model;
using MemberProducerSync.Producer.Base;
using MemberProducerSync.Producers.Base;
using Newtonsoft.Json;

namespace MemberProducerSync.Producers
{
    public class MemberProducer
    {

        protected IProducer _producer;

        public MemberProducer()
        {
            _producer = new ConfluentProducer();
        }

        public MemberProducer(IProducer producer)
        {
            _producer = producer;
        }

        public void Send(MemberModel model)
        {
            switch (model.Code)
            {
                case MemberEvents.Create:
                    SendInsert(model);
                    break;

                case MemberEvents.Update:
                    SendUpdate(model);
                    break;

                default:
                    break;
            }
        }

        public void SendUpdate(MemberModel model)
        {
            var message = new MemberUpdatedMessage();

            message.FullName = model.FullName;
            message.Age = model.Age;
            message.CellNumber = model.CellNumber;
            message.DateOfBirth = model.DateOfBirth;
            message.LegacyID = model.LegacyID;
            message.RequestId = model.RequestId;
            message.RequestDate = DateTime.Now;

            _producer.Send(1, JsonConvert.SerializeObject(model));
        }

        public void SendInsert(MemberModel model)
        {
            var message = new AddMemberMessage();

            message.FullName = model.FullName;
            message.Age = model.Age;
            message.CellNumber = model.CellNumber;
            message.DateOfBirth = model.DateOfBirth;
            message.LegacyID = model.LegacyID;
            message.RequestId = model.RequestId;
            message.RequestDate = DateTime.Now;

            _producer.Send(1, JsonConvert.SerializeObject(model));
        }

    }
}
