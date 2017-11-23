using Acars.FlightData;
using Acars.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FSUIPC;

namespace Acars.FlightData
{
    class RestrictionByAircraft
    {
        
        public Flight f = new Flight();

        //kg
        public int maximumTakeoffWeight
        { get; set; }

        public int maximumLandingWeight
        { get; set; }

        public int maximumCeiling
        { get; set; }


        public void Aircrafts()
        {
            if (f.LoadedFlightPlan.Aircraft == "C172")
            {
                maximumTakeoffWeight = 1111;
                maximumLandingWeight = 1111;
                maximumCeiling = 14500;
            }

            if (f.LoadedFlightPlan.Aircraft == "B190")
            {
                maximumTakeoffWeight = 7765;
                maximumLandingWeight = 7620;
                maximumCeiling = 25500;
            }

            if (f.LoadedFlightPlan.Aircraft == "JS41")
            {
                maximumTakeoffWeight = 10886;
                maximumLandingWeight = 10569;
                maximumCeiling = 26500;
            }

            if (f.LoadedFlightPlan.Aircraft == "DH8D")
            {
                maximumTakeoffWeight = 29574;
                maximumLandingWeight = 28123;
                maximumCeiling = 27500;
            }

            if (f.LoadedFlightPlan.Aircraft == "A320")
            {
                maximumTakeoffWeight = 77000;
                maximumLandingWeight = 64500;
                maximumCeiling = 40000;
            }

            if (f.LoadedFlightPlan.Aircraft == "B738")
            {
                maximumTakeoffWeight = 78741;
                maximumLandingWeight = 66361;
                maximumCeiling = 41500;
            }
        }
       
    }
}
