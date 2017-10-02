using Acars.FlightData;
using System.Collections.Generic;

namespace Acars
{
    public abstract class FlightEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract string Code
        { get; }

        /// <summary>
        /// The duration in seconds the event must last to be triggered
        /// </summary>
        public abstract int Duration
        { get; }

        /// <summary>
        /// Analyses Telemetry t and a returns true if the event condition is active
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public abstract bool ConditionActive(Telemetry t);

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
