﻿using CQRS.MongoDB;
using KarafkaConsumer_POC.Domain.Aggregates;
using KarafkaConsumer_POC.Domain.Events;

namespace KarafkaConsumer_POC.Domain.Commands
{
    public class UpdateMemberCommand : UpdateCommand<MemberAggregate, IPersonalInfoEvent>
    {
        public UpdateMemberCommand(MongoProvider provider) : base(provider) { }
    }
}
