namespace FlightMonitorApi
{
    public interface IDataConnector
    {
        bool BeforeStart();

        bool PushOne(FSUIPCSnapshot Snapshot);
    }
}
