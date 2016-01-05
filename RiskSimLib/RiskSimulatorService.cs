using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace RiskSimLib
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RiskSimulatorService : IRiskSimulator //, ICalculator
    {
        private Random numGen = new Random();

        public string GetServiceName()
        {
            return "Risk Combat Simulator Service";
        }

        public AttackResult SimulateAttack(ArmyComposition attackingForce, ArmyComposition defendingForce, uint trials)
        {
            var outcomes = new ConcurrentDictionary<short, uint>();

            // Random isn't thread safe so don't parallelize
         //   Parallel.For(0, trials, (i) =>
         for (int i = 0; i < trials; i++)
            {
                outcomes.AddOrUpdate(RunTrial(attackingForce, defendingForce), 1, (s, u) => u + 1);
            }
         //);

            var ret = new AttackResult()
            {
                Trials = trials,
                AttackingArmy = attackingForce,
                DefendingArmy = defendingForce,
            };

            var wins = 0u;

            foreach(var outcome in outcomes)
            {
                ret.AllOutcomeChances.Add(outcome.Key, ((double)outcome.Value) / trials);
                if (outcome.Key > 0)
                {
                    wins += outcome.Value;
                }
            }

            ret.SuccessChance = ((double)wins) / trials;

            return ret;
        }

        private short RunTrial(ArmyComposition attackingForce, ArmyComposition defendingForce)
        {
            var atkRemaining = attackingForce.Size;
            var defRemaining = defendingForce.Size;

            while (atkRemaining > 0 && defRemaining > 0)
            {
                var atkRolls = new List<byte>(3);
                var defRolls = new List<byte>(2);

                for (int i = 0; i < Math.Min((short)3, atkRemaining); i++)
                {
                    atkRolls.Add((byte)numGen.Next(6));
                }

                for (int i = 0; i < Math.Min((short)2, defRemaining); i++)
                {
                    defRolls.Add((byte)numGen.Next(6));
                }

                var numToCompare = Math.Min(atkRolls.Count, defRolls.Count);

                var modAtkRolls = atkRolls.OrderByDescending(b => b).Take(numToCompare).Select(b => (sbyte)b).ToArray();
                var modDefRolls = defRolls.OrderByDescending(b => b).Take(numToCompare).Select(b => (sbyte)b).ToArray();

                modAtkRolls[0] += attackingForce.HighestModifier;
                modDefRolls[0] += defendingForce.HighestModifier;

                modAtkRolls[numToCompare - 1] += attackingForce.LowestModifier;
                modDefRolls[numToCompare - 1] += defendingForce.LowestModifier;

                for(int i = 0; i < numToCompare; i++)
                {
                    if (modAtkRolls[i] + attackingForce.AllModifier > modDefRolls[i] + defendingForce.AllModifier)
                    {
                        defRemaining--;
                    }
                    else
                    {
                        atkRemaining--;
                    }
                }
            }

            return (short)(atkRemaining - defRemaining);
        }

#if Calc
        public double Add(double n1, double n2)
        {
            double result = n1 + n2;
            Console.WriteLine("Received Add({0},{1})", n1, n2);
            // Code added to write output to the console window.
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            double result = n1 - n2;
            Console.WriteLine("Received Subtract({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            double result = n1 * n2;
            Console.WriteLine("Received Multiply({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Divide(double n1, double n2)
        {
            double result = n1 / n2;
            Console.WriteLine("Received Divide({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }
#endif // Calc
    }

    public class CalculatorService //: ICalculator
    {
        public double Add(double n1, double n2)
        {
            double result = n1 + n2;
            Console.WriteLine("Received Add({0},{1})", n1, n2);
            // Code added to write output to the console window.
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            double result = n1 - n2;
            Console.WriteLine("Received Subtract({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            double result = n1 * n2;
            Console.WriteLine("Received Multiply({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Divide(double n1, double n2)
        {
            double result = n1 / n2;
            Console.WriteLine("Received Divide({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }
    }
}
