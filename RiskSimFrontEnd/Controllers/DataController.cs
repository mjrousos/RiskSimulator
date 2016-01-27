﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RiskSim.Models;
using RiskSimulatorService;
using System.Net;

namespace RiskSimFrontEnd.Controllers
{
    [Route("[controller]/[action]")]
    public class DataController : Controller
    {
        const int DefaultTrials = 10000;

        // GET: /Data/SimulateAttack
        [HttpPost]
        public async Task<IActionResult> SimulateAttack([FromBody] AttackResult armyCompositions)
        {
            RiskSimulatorClient client = null;
            AttackResult result = null;
            try
            {
                var Trials = armyCompositions.Trials;
                if (Trials <= 0) Trials = DefaultTrials;
                client = new RiskSimulatorClient(RiskSimulatorClient.EndpointConfiguration.BasicHttpBinding_IRiskSimulator);
                await client.OpenAsync();
                result = await client.SimulateAttackAsync(armyCompositions.AttackingArmy, armyCompositions.DefendingArmy, Trials);
                await client.CloseAsync();
            }
            catch (Exception)
            {
                if (client?.State == System.ServiceModel.CommunicationState.Opened)
                {
                    client.Abort();
                }
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
            return Json(new {
                SuccessChance = result.SuccessChance,
                ChartData = new
                {
                    labels = result.LikelyOutcomeChances.Keys.Select(s => s.ToString()).ToArray(),
                    datasets = new object[]
                    {
                        new
                        {
                            fillColor = "rgba(120, 210, 120, 0.7)",
                            strokeColor = "rgba(120, 210, 120, 0.9)",
                            highlightFill = "rgba(120, 210, 120, 0.9)",
                            highlightStroke = "rgba(120, 210, 120, 1)",
                            data = result.LikelyOutcomeChances.Values.ToArray()
                        }
                    }
                },
                options = new
                {
                    animation = true,
                    scaleOverride = true,
                    scaleStartValue= 0,
                    scaleStepWidth = 0.01,
                    scaleSteps = (int)((result.LikelyOutcomeChances.Values.Max() / 0.01) + 1)
                }
            });
        }
    }
}