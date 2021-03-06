﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RiskSimLib
{
    [ServiceContract]
    public interface IRiskSimulator
    {
        [OperationContract]
        AttackResult SimulateAttack(ArmyComposition attackingForce, ArmyComposition defendingForce, uint trials);
        
        [OperationContract]
        string GetServiceName();
    }

    //[ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    //public interface ICalculator
    //{
    //    [OperationContract]
    //    double Add(double n1, double n2);
    //    [OperationContract]
    //    double Subtract(double n1, double n2);
    //    [OperationContract]
    //    double Multiply(double n1, double n2);
    //    [OperationContract]
    //    double Divide(double n1, double n2);
    //}

    [DataContract]
    public class ArmyComposition
    {
        [DataMember]
        public short Size { get; set; }

        [DataMember]
        public sbyte HighestModifier { get; set; }

        [DataMember]
        public sbyte LowestModifier { get; set; }

        [DataMember]
        public sbyte AllModifier { get; set; }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append($"{Size} troops");
            if (HighestModifier != 0) ret.Append($" {HighestModifier.ToString("+#;-#")}H");
            if (LowestModifier != 0) ret.Append($" {HighestModifier.ToString("+#;-#")}L");
            if (AllModifier != 0) ret.Append($" {HighestModifier.ToString("+#;-#")}A");

            return ret.ToString();
        }
    }

    [DataContract]
    public class AttackResult
    {
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
