using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiskSimConsoleClient.RiskSimulators;

namespace RiskSimConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var riskClient = new RiskSimulatorClient();
            using (var calcClient = new CalculatorClient())
            {
                Console.WriteLine(riskClient.EstimateSuccessChance(new ArmyComposition() { Size = 10 }, new ArmyComposition() { Size = 10 }));

                Console.WriteLine($"5 x 11 == {calcClient.Multiply(5, 11)}");
            }
            riskClient.Close();
        }
    }
}
