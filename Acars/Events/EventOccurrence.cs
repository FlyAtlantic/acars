namespace Acars.Events
{
    public class EventOccurrence
    {
        public int StartId
        { get; private set; }

        public int EndID
        { get; private set; }

        public FlightEvent Event
        { get; private set; }

        public EventOccurrence(int StartId, int EndId, FlightEvent Event)
        {
            this.StartId = StartId;
            this.EndID = EndID;
            this.Event = Event;
        }
    }
}
