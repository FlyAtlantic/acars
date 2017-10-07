using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acars.FlightData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Acars.Events.FlightEvent;
using System.Diagnostics;

namespace Acars.FlightData.Tests
{
    [TestClass()]
    public class FlightDatabaseTests
    {
        [TestMethod()]
        public void GetFlightTest()
        {
            // should return null if no flight found for user
            Assert.IsNull(FlightDatabase.GetFlight(""));
        }

        [TestMethod()]
        public void Timings()
        {
            // SetUp
            Telemetry[] T = new Telemetry[] { new Telemetry(), new Telemetry() };
            List<Telemetry> result = new List<Telemetry>();
            int Duration = 5;
            int i = 0;
            int eventStart = -1;
            int eventEnd = -1;

            Telemetry t = T[i];
            FlightEventConditionActiveDelegate ConditionActive = (_t) => { return (Guid.NewGuid() != Guid.NewGuid()); };

            int iterations = 1 * 3600 * 1;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (--iterations > 0)
            {
                // check for event start
                if (eventStart < 0 && ConditionActive(t))
                {
                    eventStart = i;
                    eventEnd = i;
                }

                // check for event finished
                if (eventEnd != -1 && !ConditionActive(t))
                {
                    // if the condition is no longer active, then it was the last index the last one active
                    eventEnd = i - 1;

                    // validate duration, and proceed
                    if ((T[eventEnd].Timestamp - T[eventStart].Timestamp).TotalSeconds > Duration)
                    {
                        int[] I = { eventStart, eventEnd };
                        result.Add(t);
                        eventStart = -1;
                        eventEnd = -1;
                    }
                }

                //i++;
            }

            sw.Stop();

            Assert.Fail(sw.ElapsedMilliseconds.ToString());
        }
    }
}