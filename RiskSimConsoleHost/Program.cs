using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace RiskSimConsoleHost
{
    class Program
    {
        const string BaseAddress = "http://localhost:8000/Simulators";
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(RiskSimLib.RiskSimulatorService), new Uri(BaseAddress));

            try
            {
                host.AddServiceEndpoint(typeof(RiskSimLib.IRiskSimulator), new WSHttpBinding(), "Risk");
                host.AddServiceEndpoint(typeof(RiskSimLib.ICalculator), new WSHttpBinding(), "Calc");

                var smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                host.Description.Behaviors.Add(smb);

                var sdb = host.Description.Behaviors[typeof(ServiceDebugBehavior)] as ServiceDebugBehavior;
                sdb.IncludeExceptionDetailInFaults = true;

                host.Open();

                Console.WriteLine("Service running...");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();

                host.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine($"An unexpected error occured:{Environment.NewLine}{ce.ToString()}");
                host.Abort();
            }
        }
    }
}
