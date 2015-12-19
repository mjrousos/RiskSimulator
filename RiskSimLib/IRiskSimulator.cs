using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RiskSimLib
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IRiskSimulator
    {
        [OperationContract]
        double EstimateSuccessChance(ArmyComposition attackingForce, ArmyComposition defendingForce);

        [OperationContract]
        string GetServiceName();
    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        double Divide(double n1, double n2);
    }

    [DataContract]
    public class ArmyComposition
    {
        [DataMember]
        short Size { get; set; }

        [DataMember]
        sbyte HighestModifier { get; set; }

        [DataMember]
        sbyte LowestModifier { get; set; }

        [DataMember]
        sbyte AllModifier { get; set; }

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
