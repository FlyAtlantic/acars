using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acars.FlightData
{
    public class Location
    {
        public GeoCoordinate Position
        { get; private set; }

        public string Identifier
        { get; private set; }

        public Location(string Identifier, GeoCoordinate Position)
        {
            this.Identifier = Identifier;
            this.Position = Position;
        }

        /// <summary>
        /// Returns true if currectPosition is within margin meters from this.Position
        /// </summary>
        /// <param name="currentPosition">Coordinate to compare position to</param>
        /// <param name="margin">Margin distance, in meters</param>
        /// <returns></returns>
        public bool IsAt(GeoCoordinate currentPosition, int margin)
        {
            //retorna distancia em metros
            return Position.GetDistanceTo(currentPosition) > margin;
        }

        /// <summary>
        /// Returns true if currentLocation is within margin meters from this
        /// </summary>
        /// <param name="currentLocation">Location to compare Position to</param>
        /// <param name="margin">Margin distance, in meters</param>
        /// <returns></returns>
        public bool IsAt(Location currentLocation, int margin)
        {
            return IsAt(currentLocation.Position, margin);
        }
    }
}
