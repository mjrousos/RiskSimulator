using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RiskSim.Models
{
    // TODO : Make RiskSimLib a PCL and use this type from there
    [DataContract]
    public class AttackResult
    {
        public AttackResult()
        {
            AttackingArmy = new ArmyComposition();
            DefendingArmy = new ArmyComposition();  
        }

        [DataMember]
        public ArmyComposition AttackingArmy { get; set; }

        [DataMember]
        public ArmyComposition DefendingArmy { get; set; }

        [DataMember]
        public uint Trials { get; set; }

        [DataMember]
        public double SuccessChance { get; set; }

        [DataMember]
        public Dictionary<short, double> AllOutcomeChances { get; } = new Dictionary<short, double>();

        [IgnoreDataMember]
        public IDictionary<short, double> LikelyOutcomeChances
        {
            get
            {
                var greatestChance = AllOutcomeChances.Max(kvp => kvp.Value);
                var mostLikelyOutcome = AllOutcomeChances.Where(kvp => kvp.Value == greatestChance).FirstOrDefault().Key;
                return AllOutcomeChances.Where(kvp => Math.Abs(kvp.Key - mostLikelyOutcome) <= 3)
                                        .OrderBy(kvp => kvp.Key)
                                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }
    }
}
