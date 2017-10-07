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

        /// <summary>
        /// Human readable description of the event trigger
        /// </summary>
        public string Description
        { get; private set; }

        /// <summary>
        /// Percentage to discount from flight score
        /// </summary>
        public int Discount
        { get; private set; }

        /// <summary>
        /// Max applicable discount to apply to flight score
        /// </summary>
        public int MaxDiscount
        { get; private set; }

        private FlightEventConditionActiveDelegate ConditionActive;

        public delegate bool FlightEventConditionActiveDelegate(Telemetry t);

        public FlightEvent(string Code, int Duration, string Message, int Discount, int MaxDiscount, FlightEventConditionActiveDelegate ConditionActive)
        {
            this.Code = Code;
            this.Duration = Duration;
            this.Description = Description;
            this.Discount = Discount;
            this.MaxDiscount = MaxDiscount;
            this.ConditionActive = ConditionActive;
        }

        /// <summary>
        /// Gets all indeces of start 
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public EventOccurrence[] GetOccurrences(Telemetry[] T, out int AcculumatedDiscount)
        {
            List<EventOccurrence> result = new List<EventOccurrence>();
            int i = 0;

            int eventStart = -1;
            int eventEnd = -1;
            AcculumatedDiscount = 0;

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
                        result.Add(new EventOccurrence(eventStart, eventEnd, this));

                        // calculate score discount
                        AcculumatedDiscount += Discount;
                        if (AcculumatedDiscount > MaxDiscount)
                            AcculumatedDiscount = MaxDiscount;

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
