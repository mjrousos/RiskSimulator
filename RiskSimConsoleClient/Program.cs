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

            try
            {
                var quit = false;

                while (!quit)
                {
                    ArmyComposition atk = new ArmyComposition();
                    ArmyComposition def = new ArmyComposition();
                    Console.Write("Number of attackers:\t");
                    atk.Size = short.Parse(Console.ReadLine());
                    Console.Write("Number of defenders:\t");
                    def.Size = short.Parse(Console.ReadLine());
                    Console.Write("Success chance:     \t");
                    var result = riskClient.SimulateAttack(atk, def, 10000);
                    Console.WriteLine(result.SuccessChance.ToString("P2"));
                    Console.WriteLine();
                }
                riskClient.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Caught unexpected exception: " + ex.ToString());
                riskClient.Abort();
            }
        }
    }
}
