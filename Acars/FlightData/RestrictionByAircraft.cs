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
    public class AircraftPerformance
    {
        public AircraftPerformance(string Identifier, int MTW, int MLW, int Celling, Dictionary<short, FlapSetting> FlapSettings)
        {
            this.Identifier = Identifier;
            this.MTW = MTW;
            this.MLW = MLW;
            this.Celling = Celling;
            this.FlapSettings = FlapSettings;
        }

        public string Identifier
        { get; private set; }
        public int MTW
        { get; private set; }
        public int MLW
        { get; private set; }
        public int Celling
        { get; private set; }
        public Dictionary<short, FlapSetting> FlapSettings
        { get; private set; }
    }

    public class FlapSetting
    {
        /// <summary>
        /// Dados de performance para cada configuração e flaps
        /// </summary>
        /// <param name="Identifier">Valor lido pelo FSUIPC</param>
        /// <param name="Name">Valor indicado no cockpit do avião</param>
        /// <param name="IASLimit">Limite em IAS de operação</param>
        public FlapSetting(string Name, int IASLimit)
        {
            this.Name = Name;
            this.IASLimit = IASLimit;
        }

        public short Position
        { get; private set; }
        public string Name
        { get; private set; }
        public int IASLimit
        { get; private set; }

        public bool isOverSpeed(Telemetry t)
        {
            return (t.IndicatedAirSpeed > IASLimit);
        }
        
        
    }

    //public class RestrictionByAircraft
    //{                     

    //    //kg
    //    public int maximumTakeoffWeight
    //    { get; set; }

    //    public int maximumLandingWeight
    //    { get; set; }

    //    public int maximumCeiling
    //    { get; set; }

    //    public int maximumspeedflaps
    //    { get; set; }

    //    public int flapPosition
    //    { get; set; }

    //    public int maximumSpeedIndicated
    //    { get; set; }


    //    public void Aircrafts(Flight f)
    //    {      
    //        try
    //        {
    //            if (f.LoadedFlightPlan != null)
    //            {
    //                if (f.LoadedFlightPlan.Aircraft == "C172")
    //                {
    //                    maximumTakeoffWeight = 1111;
    //                    maximumLandingWeight = 1111;
    //                    maximumCeiling = 14500;
    //                    maximumSpeedIndicated = 128;

    //                    //Flaps
    //                    int flaps;

    //                    flaps = f.LastTelemetry.Flaps;

    //                    // Flaps 0
    //                    if (flaps < 1)
    //                    {
    //                        flapPosition = 0;
    //                        maximumspeedflaps = maximumSpeedIndicated + 500;
    //                    }
    //                    // Flaps 10
    //                    if (flaps > 10 && flaps < 5461)
    //                    {
    //                        flapPosition = 10;
    //                        maximumspeedflaps = 85;
    //                    }
    //                    // Flaps 20
    //                    if (flaps > 5461 && flaps < 10922)
    //                    {
    //                        flapPosition = 20;
    //                        maximumspeedflaps = 85;
    //                    }
    //                    // Flaps 30
    //                    if (flaps > 10922 && flaps < 16500)
    //                    {
    //                        flapPosition = 30;
    //                        maximumspeedflaps = 85;
    //                    }
    //                }

    //                if (f.LoadedFlightPlan.Aircraft == "B190")
    //                {
    //                    maximumTakeoffWeight = 7765;
    //                    maximumLandingWeight = 7620;
    //                    maximumCeiling = 25500;
    //                }

    //                if (f.LoadedFlightPlan.Aircraft == "JS41")
    //                {
    //                    maximumTakeoffWeight = 10886;
    //                    maximumLandingWeight = 10569;
    //                    maximumCeiling = 26500;
    //                }

    //                if (f.LoadedFlightPlan.Aircraft == "DH8D")
    //                {
    //                    maximumTakeoffWeight = 29574;
    //                    maximumLandingWeight = 28123;
    //                    maximumCeiling = 27500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "AT72")
    //                {
    //                    maximumTakeoffWeight = 23000;
    //                    maximumLandingWeight = 22500;
    //                    maximumCeiling = 25500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "RJ1H")
    //                {
    //                    maximumTakeoffWeight = 46500;
    //                    maximumLandingWeight = 40500;
    //                    maximumCeiling = 25500;
    //                }

    //                if (f.LoadedFlightPlan.Aircraft == "A320")
    //                {
    //                    maximumTakeoffWeight = 77000;
    //                    maximumLandingWeight = 64500;
    //                    maximumCeiling = 40000;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "A332")
    //                {
    //                    maximumTakeoffWeight = 242000;
    //                    maximumLandingWeight = 182000;
    //                    maximumCeiling = 41500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "A343")
    //                {
    //                    maximumTakeoffWeight = 276500;
    //                    maximumLandingWeight = 192000;
    //                    maximumCeiling = 41500;
    //                }

    //                if (f.LoadedFlightPlan.Aircraft == "B738")
    //                {
    //                    maximumTakeoffWeight = 78741;
    //                    maximumLandingWeight = 66361;
    //                    maximumCeiling = 41500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "B744")
    //                {
    //                    maximumTakeoffWeight = 412760;
    //                    maximumLandingWeight = 295743;
    //                    maximumCeiling = 45500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "B763")
    //                {
    //                    maximumTakeoffWeight = 181437;
    //                    maximumLandingWeight = 145150;
    //                    maximumCeiling = 43500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "B77L")
    //                {
    //                    maximumTakeoffWeight = 242672;
    //                    maximumLandingWeight = 201800;
    //                    maximumCeiling = 43500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "MD11")
    //                {
    //                    maximumTakeoffWeight = 286000;
    //                    maximumLandingWeight = 200000;
    //                    maximumCeiling = 43500;
    //                }

    //                //CONFIRM
    //                if (f.LoadedFlightPlan.Aircraft == "CONC")
    //                {
    //                    maximumTakeoffWeight = 185000;
    //                    maximumLandingWeight = 111130;
    //                    maximumCeiling = 60500;
    //                }
    //            }
    //        }
    //        catch (Exception crap)
    //        {
    //            Console.WriteLine("GetFlightTimer_Tick \r\n {0}", App.GetFullMessage(crap));
    //        }
    //        finally
    //        {

    //        }

    //    }
       
    //}
}
