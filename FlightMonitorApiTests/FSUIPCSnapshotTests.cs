using FlightMonitorApi;
using NUnit.Framework;

namespace FlightMonitorApiTests
{
    [TestFixture]
    public partial class FSUIPCSnapshotTests
    {
        [TestCase]
        public void IsInteresting_ZeroDuration()
        {
            FSUIPCInterest interest = new FSUIPCInterest()
            {
                Scenario = (
                    FSUIPCSnapshot Snapshot,
                    ref FSUIPCSnapshot Cached) =>
                {
                    return true;
                }
            };
            Assert.IsTrue(interest.IsInteresting(new FSUIPCSnapshot()));
            Assert.IsNull(interest.Cached);


            interest.Scenario = (
                FSUIPCSnapshot Snapshot,
                ref FSUIPCSnapshot Cached) =>
            {
                if (Cached == null)
                {
                    Cached = new FSUIPCSnapshot();
                    return false;
                }
                return true;
            };
            Assert.IsFalse(interest.IsInteresting(new FSUIPCSnapshot()));
            Assert.IsNotNull(interest.Cached);
            Assert.IsTrue(interest.IsInteresting(new FSUIPCSnapshot()));
        }
    }
}
