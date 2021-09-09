using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neigh.Datamodel;

namespace neigh.Models
{
    public class ResultEditModel : IDisposable
    {
        public int HorseId { get; set; }
        public int ShowId { get; set; }
        public int ClassId { get; set; }
        public TClass Class { get; set; }
        public THorse Horse { get; set; }
        public List<TResult_Show_Judge> Results_Show_Judges { get; set; }
        public int? OverallPlacing { get; set; }
        public int? HorsesInClass { get; set; }
        public String ClassNumber { get; set; }

        public class TClass
        {
            public String Name { get; set; }
        }
        public class THorse
        {
            public String Nickname { get; set; }
        }
        public class TResult_Show_Judge
        {
            public int? Placing { get; set; }
            public int Show_JudgeId { get; set; }
            public TShow_Judge Show_Judges { get; set; }
        }
        public class TShow_Judge
        {
            public String Name { get; set; }
        }

        public ResultEditModel(Result r)
        {
            HorseId = r.HorseId;
            ClassId = r.ClassId;
            ShowId = r.ShowId;
            Results_Show_Judges = new List<TResult_Show_Judge>();
            Class = new TClass()
            {
                Name = r.Class.Name
            };
            Horse = new THorse()
            {
                Nickname = r.Horse.Nickname
            };
            OverallPlacing = r.OverallPlacing;
            HorsesInClass = r.HorsesInClass;
            ClassNumber = r.ClassNumber;

            foreach (Results_Show_Judges result in r.Results_Show_Judges)
            {
                Results_Show_Judges.Add(new TResult_Show_Judge()
                {
                    Placing = result.Placing,
                    Show_JudgeId = result.Show_JudgeId,
                    Show_Judges = new TShow_Judge()
                    {
                        Name = result.Show_Judges.Name
                    }
                });
            }
        }

        public void Dispose()
        {
            Results_Show_Judges.Clear();
        }
    }
}