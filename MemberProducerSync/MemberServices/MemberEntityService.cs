using MemberProducerSync.EF;
using MemberProducerSync.EF.Models;
using MemberProducerSync.Model;
using MemberProducerSync.Repository;
using MemberProducerSync.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MemberProducerSync.MemberService
{
    public class MemberEntityService
    {
        private MemberEntityRepository _repo;


        public MemberEntityService(MemberContext context)
        {
            _repo = new MemberEntityRepository(context);
        }

        public void InsertOrUpdate(MemberModel model)
        {
            model.Date = DateTime.Now;
            var sucess = InsertMember(model);
            if (sucess)
            {
                HttpHelper.SendEventMember(model);
            }
        }

        private bool InsertMember(MemberModel model)
        {
            try
            {
                var e = GetById(model.ID);
                bool isNew = e == null;

                e = Map(model);

                if (isNew)
                {
                    model.Code = MemberEvents.Create;
                    _repo.Add(e);
                }
                else
                {
                    model.Code = MemberEvents.Update;
                    _repo.Update(e);
                }

                _repo.Commit();
            }
            catch
            {
                return false;
            }

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

            return e;
        }


    }
}
