using Acars.FlightData;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Acars.Events
{
    public class FlightEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public string Code
        { get; private set; }

        /// <summary>
        /// The duration in seconds the event must last to be triggered
        /// </summary>
        public int Duration
        { get; private set; }

        private FlightEventConditionActiveDelegate ConditionActive;

        public delegate bool FlightEventConditionActiveDelegate(Telemetry t);

        public FlightEvent(string Code, int Duration, FlightEventConditionActiveDelegate ConditionActive)
        {
            this.Code = Code;
            this.Duration = Duration;
            this.ConditionActive = ConditionActive;
        }

        /// <summary>
        /// Gets all indeces of start 
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int[][] GetOccurences(Telemetry[] T)
        {
            List<int[]> result = new List<int[]>();
            int i = 0;

            int eventStart = -1;
            int eventEnd = -1;

            foreach (Telemetry t in T)
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
                        result.Add(I);
                        eventStart = -1;
                        eventEnd = -1;
                    }
                }

                i++;
            }

            return result.ToArray();
        }
    }
}
