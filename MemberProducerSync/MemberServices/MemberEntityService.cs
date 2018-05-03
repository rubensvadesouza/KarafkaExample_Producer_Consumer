using MemberProducerSync.EF;
using MemberProducerSync.EF.Models;
using MemberProducerSync.Model;
using MemberProducerSync.Producer.Base;
using MemberProducerSync.Producers;
using MemberProducerSync.Repository;
using MemberProducerSync.Utils;
using System;
using System.Threading.Tasks;

namespace MemberProducerSync.MemberService
{
    public class MemberEntityService
    {
        private MemberEntityRepository _repo;

        private MemberProducer _sync;

        public MemberEntityService(MemberContext context)
        {
            _repo = new MemberEntityRepository(context);
            _sync = new MemberProducer(new ConfluentProducer());
        }

        public void InsertOrUpdate(MemberModel model)
        {
            model.Date = DateTime.Now;
            var sucess = InsertMember(model);
            if (sucess)
            {
                _sync.Send(model);
            }
        }

        private bool InsertMember(MemberModel model)
        {
            model.Code = MemberEvents.Create;
            _sync.Send(model);
            //try
            //{
            //    var e = GetById(model.ID);
            //    bool isNew = e == null;

            //    e = Map(model, e);

            //    if (isNew)
            //    {
            //        model.Code = MemberEvents.Create;
            //        _repo.Add(e);
            //    }
            //    else
            //    {
            //        model.Code = MemberEvents.Update;
            //        _repo.Update(e);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

            return true;
        }

        public MemberEntity GetById(string id)
        {
            return _repo.GetSingle(id);
        }

        public Task<MemberEntity> GetByIdAsync(string id)
        {
            return _repo.GetSingleAsync(id);
        }

        private MemberEntity Map(MemberModel member, MemberEntity entity = null)
        {
            var e = entity ?? new MemberEntity();

            e.ID = member.ID;
            e.FullName = member.FullName;
            e.DateOfBirth = member.DateOfBirth;
            e.CellNumber = member.CellNumber;
            e.Age = member.Age;
            e.Version = member.Version;

            return e;
        }
    }
}