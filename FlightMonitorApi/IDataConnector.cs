namespace FlightMonitorApi
{
    public interface IDataConnector
    {
        bool LookupFlightPlan();

        bool PushOne(FSUIPCSnapshot Snapshot);
    }
}
