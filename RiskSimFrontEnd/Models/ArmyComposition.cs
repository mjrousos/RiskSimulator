using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text;

namespace RiskSimFrontEnd.Models
{
    // TODO : Make RiskSimLib a PCL and use this type from there
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
}
