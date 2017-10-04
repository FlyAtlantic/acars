using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acars.FlightData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}