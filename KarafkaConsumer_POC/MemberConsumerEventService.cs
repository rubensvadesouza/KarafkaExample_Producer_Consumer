using KarafkaConsumer_POC.Contracts.Messages;
using KarafkaConsumer_POC.Domain.Events;
using KarafkaConsumer_POC.Domain.Handlers;
using KarafkaConsumer_POC.Model;
using MemberConsumerSync.Utils;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MemberConsumerSync
{
    public class MemberConsumerEventService
    {
        private MemberCreatedEventHandler _createdHandler;
        private MemberUpdatedEventHandler _updatedHandler;

        public MemberConsumerEventService(MemberCreatedEventHandler created, MemberUpdatedEventHandler updated)
        {
            _createdHandler = created;
            _updatedHandler = updated;
        }

        public async void ProcessMessage(string message)
        {
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(message);

            switch (baseMessage.Code)
            {
                case MemberEvents.Create:
                    await ProcessAddMessage(message);
                    break;

                case MemberEvents.Update:
                    await ProcessUpdateMessage(message);
                    break;

                default:
                    break;
            }
        }

        private async Task ProcessAddMessage(string message)
        {
            var msg = JsonConvert.DeserializeObject<AddMemberMessage>(message);
            var ret = await _createdHandler.HandleMember(msg);

            if (ret)
            {
                var model = new MemberModel();
                model.ID = msg.LegacyID;
                model.FullName = msg.FullName;
                model.Age = msg.Age;
                model.CellNumber = msg.CellNumber;
                model.DateOfBirth = msg.DateOfBirth;
                model.RequestDate = msg.RequestDate;
                model.RequestId = msg.RequestId;
                model.Date = msg.RequestDate;
                model.Version = msg.Version;
                model.Code = string.Empty;

                if(msg.Source == 0)
                {
                    HttpHelper.SendLegacyMessage(model);
                }
                else
                {
                    HttpHelper.SendNextMessage(model);
                }
            }
        }

        private async Task ProcessUpdateMessage(string message)
        {
            var msg = JsonConvert.DeserializeObject<MemberUpdatedMessage>(message);
            var ret = await _updatedHandler.HandleMember(msg);

            if (ret)
            {
                var model = new MemberModel();
                model.ID = msg.LegacyID;
                model.FullName = msg.FullName;
                model.Age = msg.Age;
                model.CellNumber = msg.CellNumber;
                model.DateOfBirth = msg.DateOfBirth;
                model.RequestDate = msg.RequestDate;
                model.Date = msg.RequestDate;
                model.RequestId = msg.RequestId;
                model.Code = string.Empty;

                if (msg.Source == 0)
                {
                    HttpHelper.SendLegacyMessage(model);
                }
                else
                {
                    HttpHelper.SendNextMessage(model);
                }
            }
        }
    }
}