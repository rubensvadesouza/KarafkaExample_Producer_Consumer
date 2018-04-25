using KarafkaConsumer_POC.Domain.Events;
using MemberProducerSync.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace MemberStressTest
{
    internal class MemberProducerStressTest
    {
        private List<MemberModel> Members { get; set; }

        private static void Main(string[] args)
        {
        }

        //private static List<MemberModel> GetMembers()
        //{
        //    var members = new List<MemberModel>();

        //    var m1 = new MemberModel()
        //    {
        //        ID = "1",
        //        Source = 0,
        //        Code = MemberEvents.Create,
        //        RequestId = ObjectId.GenerateNewId().ToString(),
        //        RequestDate = DateTime.Now,
        //        FullName = "Joao da Silva Penha",
        //        Age = 25,
        //        CellNumber = "15997791860",
        //        DateOfBirth = DateTime.Now.AddYears(-25),
        //        Date = DateTime.Now,
        //        Version = 1
        //    };

        //    var m2 = new MemberModel()
        //    {
        //        ID = "2",
        //        Source = 0,
        //        Code = MemberEvents.Create,
        //        RequestId = ObjectId.GenerateNewId().ToString(),
        //        RequestDate = DateTime.Now,
        //        FullName = "Ricardo Alves Souza",
        //        Age = 25,
        //        CellNumber = "15997791860",
        //        DateOfBirth = DateTime.Now.AddYears(-25),
        //        Date = DateTime.Now,
        //        Version = 1
        //    };
        //}
    }
}