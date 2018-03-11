using Acars.FlightData;
using Dapper;
using FlightMonitorApi;
using MySql.Data.MySqlClient;
using System;

namespace Acars
{
    public partial class DatabaseConnector : IDataConnector
    {
        public DatabaseConnector(string UserEmail)
        {
            this.UserEmail = UserEmail;
            ActiveFlightPlan = null;
            LoadedFlight = null;
        }

        /// <summary>
        /// MySql Connection String
        /// 
        /// gathered from application properties, alter them to match your testing
        /// database, this is replaced with the build app.config on the app veyor
        /// build process
        /// </summary>
        private static string ConnectionString =>
            String.Format(
                @"server={0};uid={1};pwd={2};database={3};Connection Timeout=60;",
                Properties.Settings.Default.Server,
                Properties.Settings.Default.Dbuser,
                Properties.Settings.Default.Dbpass,
                Properties.Settings.Default.Database);

        /// <summary>
        /// The current logged in user email
        /// </summary>
        private string UserEmail
        { get; set; }

        /// <summary>
        /// Currently active flight plan
        ///
        /// Set to null if no flight plan active, or if the previously active flight
        /// plan was cancelled or finished
        /// </summary>
        public FlightPlan ActiveFlightPlan
        { get; private set; }

        /// <summary>
        /// Returns true if the current flight plan has been set as 'started' on the
        /// database, false otherwise.
        /// </summary>
        public bool FlightStarted
        {
            get { return (ActiveFlightPlan.Id != null); }
        }

        /// <summary>
        /// MySql datastructure supporting flight data reports
        /// </summary>
        public Flight LoadedFlight
        { get; private set; }

        /// <summary>
        /// Tries to load the currently active flight plan, if one available.
        /// 
        /// Returns true if a valid flight is available at `ActiveFlightPlan`, false
        /// otherwise
        /// NOTE: it does not override an existing flight plan, set
        /// `ActiveFlightPlan = null` before to achieve that.
        /// </summary>
        /// <returns></returns>
        public bool BeforeStart()
        {
            if (ActiveFlightPlan != null)
                return true;

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    ActiveFlightPlan = conn.QueryFirstOrDefault<FlightPlan>(
                        SELECT_FLIGHT_PLAN,
                        new { UserEmail }
                    );
                }
                catch (InvalidOperationException)
                {
                    ActiveFlightPlan = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return (ActiveFlightPlan != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flight"></param>
        /// <returns>(int) Inserted pirep ID</returns>
        public long StartFlight()
        {
            if (ActiveFlightPlan == null)
                throw new NullReferenceException(
                    "Trying to start a flight without a flight plan load, verify " +
                    "ActiveFlightPlan instantiation.");

            int InsertedId = -1;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    InsertedId = conn.Execute(
                        INSERT_PIREP,
                        new
                        {
                            StartTime = DateTime.UtcNow,
                            ActiveFlightPlan.Id,
                            UserEmail
                        }
                    );
                }
                catch (Exception crap)
                {
                    throw new Exception(String.Format(
                        "INSERT_PIREP command failed:: {0}, {1}",
                        ActiveFlightPlan.Id,
                        UserEmail),
                        crap
                    );
                }
                finally
                {
                    conn.Close();
                }
            }

            if (InsertedId > -1)
                ActiveFlightPlan.Id = InsertedId;

            return InsertedId;
        }

        /// <summary>
        /// Executes StartFlight up to five times or until a positive response is
        /// returned
        /// </summary>
        /// <param name="Tries"></param>
        public void TryStartFlight(int Tries = 5)
        {
            while (!FlightStarted || Tries < 0)
            {
                StartFlight();
                Tries--;
            }

            // TODO: we should do something about Tries -1 at this point
        }

        /// <summary>
        /// 
        /// </summary>
        public int UpdateFlight(FSUIPCSnapshot Snapshot)
        {
            int FlightLogId = -1;
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    conn.Execute(
                        TOUCH_PILOT_ASSIGNMENT,
                        new { UserEmail }
                    );

                    FlightLogId = conn.Execute(
                        INSERT_FLIGHT_LOG,
                        new
                        {
                            FlightId = ActiveFlightPlan.Id,
                            Snapshot.TimeStamp,
                            Latitude = Snapshot.Position[0],
                            Longitude = Snapshot.Position[1],
                            Snapshot.Altitude,
                            Heading = Snapshot.Compass,
                            Snapshot.GroundSpeed,
                            FlightPhase = "Not Implemented in 1.2"
                        }
                    );
                }
                catch (Exception crap)
                {
                    // TODO: TOUCH_PILOT_ASSIGNMENT might have been the cause..
                    throw new Exception(String.Format(
                        "INSERT_FLIGHT_LOG command failed:: {0}",
                        ActiveFlightPlan.Id),
                        crap);
                }
                finally
                {
                    conn.Close();
                }

                return FlightLogId;
            }
        }

        public bool PushOne(FSUIPCSnapshot Snapshot)
        {
            TryStartFlight();

            return (UpdateFlight(Snapshot) > -1);
        }
    }
}
