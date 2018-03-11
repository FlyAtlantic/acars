using Acars.FlightData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acars
{
    public static class DatabaseConnector
    {
        private static string ConnectionString
        {
            get
            {
                return String.Format(
                    @"server={0};uid={1};pwd={2};database={3};Connection Timeout=
60;",
                    Properties.Settings.Default.Server,
                    Properties.Settings.Default.Dbuser,
                    Properties.Settings.Default.Dbpass,
                    Properties.Settings.Default.Database);
            }
        }

        private static string UserEmail
        { get; set; }

        private static FlightPlan ActiveFlightPlan
        { get; set; }
        
    }
}
