using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiskSimConsoleClient.RiskSimulatorService;

namespace RiskSimConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpointSuffix = "Binding_IRiskSimulator";
            RiskSimulatorClient riskClient = null;

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
                    Console.Write("Binding (BasicHttp, WebHttp, WsHttp): ");
                    var endpointName = Console.ReadLine() + endpointSuffix;
                    riskClient = new RiskSimulatorClient(endpointName);
                    riskClient.ClientCredentials.Windows.ClientCredential.UserName = "Foo";
                    riskClient.ClientCredentials.Windows.ClientCredential.Password = "FooPasswork";
                    Console.Write("Success chance:     \t");
                    var result = riskClient.SimulateAttack(atk, def, 10000);
                    Console.WriteLine(result.SuccessChance.ToString("P2"));
                    Console.WriteLine();
                    riskClient.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Caught unexpected exception: " + ex.ToString());
                if (riskClient?.State == System.ServiceModel.CommunicationState.Opened)
                {
                    riskClient.Abort();
                }
            }
        }
    }
}
