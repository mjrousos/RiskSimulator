using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text;

namespace RiskSim.Models
{
    // TODO : Make RiskSimLib a PCL and use this type from there
    [DataContract]
    public class ArmyComposition
    {
        [DataMember]
        [Display(Name = "Army Size")]
        public short Size { get; set; }

        [DataMember]
        [Display(Name = "+/- to Highest Die")]
        public sbyte HighestModifier { get; set; }

        [DataMember]
        [Display(Name = "+/- to Lowest Die")]
        public sbyte LowestModifier { get; set; }

        [DataMember]
        [Display(Name = "+/- to All Dice")]
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
}
