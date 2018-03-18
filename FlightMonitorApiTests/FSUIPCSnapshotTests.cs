using System;
using FlightMonitorApi;
using NUnit.Framework;

namespace FlightMonitorApiTests
{
    [TestFixture]
    public partial class FSUIPCSnapshotTests
    {
        [TestCase]
        public void IsInteresting_OneSecondDuration_SetsCache()
        {
            FSUIPCInterest interest = new FSUIPCInterest()
            {
                Scenario = (
                    FSUIPCSnapshot Snapshot,
                    ref FSUIPCSnapshot Cached) =>
                {
                    Cached = Cached ?? Snapshot;

                    var minutes = (Snapshot.TimeStamp - Cached.TimeStamp)
                        .TotalMinutes;

                    return (minutes > 10);
                }
            };
            Assert.IsFalse(interest.IsInteresting(new FSUIPCSnapshot()
            {
                TimeStamp = DateTime.UtcNow
            }));
            Assert.IsNotNull(interest.Cached);
            Assert.IsFalse(interest.IsInteresting(new FSUIPCSnapshot()
            {
                TimeStamp = DateTime.UtcNow.AddMinutes(9)
            }));
            Assert.IsFalse(interest.IsInteresting(new FSUIPCSnapshot()
            {
                TimeStamp = DateTime.UtcNow.AddMinutes(9)
            }));
            Assert.IsTrue(interest.IsInteresting(new FSUIPCSnapshot()
            {
                TimeStamp = DateTime.UtcNow.AddMinutes(10)
            }));
        }
    }
}
