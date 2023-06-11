using System;
using SGPdotNET.Propagation;
using SGPdotNET.Util;

namespace SGPdotNET.CoordinateSystem
{
    /// <inheritdoc />
    /// <summary>
    ///     Stores an Earth-centered inertial position for a particular time
    /// </summary>
    public class EciCoordinate : Coordinate
    {
        /// <summary>
        ///     The time component of the coordinate
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        ///     The position component of the coordinate
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        ///     The velocity component of the coordinate
        /// </summary>
        public Vector3 Velocity { get; }

        /// <summary>
        ///     Creates a new ECI coordinate at the origin
        /// </summary>
        public EciCoordinate()
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     Creates a new ECI coordinate with the specified values
        /// </summary>
        /// <param name="dt">The date to be used for this position</param>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        /// <param name="altitude">The altitude in kilometers</param>
        public EciCoordinate(DateTime dt, Angle latitude, Angle longitude, double altitude)
            : this(dt, new GeodeticCoordinate(latitude, longitude, altitude))
        {
        }

        /// <summary>
        ///     Creates a new ECI coordinate with the specified values
        /// </summary>
        /// <param name="dt">The date to be used for this position</param>
        /// <param name="coord">The position top copy</param>
        public EciCoordinate(DateTime dt, Coordinate coord)
        {
            dt = dt.ToStrictUtc();
            var eci = coord.ToEci(dt);

            Time = dt;
            Position = eci.Position;
            Velocity = eci.Velocity;
        }

        /// <inheritdoc />
        public EciCoordinate(DateTime dt, Vector3 position) : this(dt, position, new Vector3())
        {
        }

        /// <summary>
        ///     Creates a new ECI coordinate with the specified values
        /// </summary>
        /// <param name="dt">The date to be used for this position</param>
        /// <param name="position">The ECI position vector</param>
        /// <param name="velocity">The ECI velocity vector</param>
        public EciCoordinate(DateTime dt, Vector3 position, Vector3 velocity)
        {
            Time = dt.ToStrictUtc();
            Position = position;
            Velocity = velocity;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Converts this ECI position to a geodetic one
        /// </summary>
        /// <returns>The position in a geodetic reference frame</returns>
        public override GeodeticCoordinate ToGeodetic()
        {
            var theta = MathUtil.AcTan(Position.Y, Position.X);

            var lon = MathUtil.WrapNegPosPi(theta - Time.ToGreenwichSiderealTime());

            var r = Math.Sqrt(Position.X * Position.X + Position.Y * Position.Y);

            const double e2 = SgpConstants.EarthFlatteningConstant * (2.0 - SgpConstants.EarthFlatteningConstant);

            var lat = MathUtil.AcTan(Position.Z, r);
            double phi;
            double c;
            var cnt = 0;

            do
            {
                phi = lat;
                var sinphi = Math.Sin(phi);
                c = 1.0 / Math.Sqrt(1.0 - e2 * sinphi * sinphi);
                lat = MathUtil.AcTan(Position.Z + SgpConstants.EarthRadiusKm * c * e2 * sinphi, r);
                cnt++;
            } while (Math.Abs(lat - phi) >= 1e-10 && cnt < 10);

            var alt = r / Math.Cos(lat) - SgpConstants.EarthRadiusKm * c;

            return new GeodeticCoordinate(Angle.FromRadians(lat), Angle.FromRadians(lon), alt);
        }

        /// <inheritdoc />
        public override EciCoordinate ToEci(DateTime dt)
        {
            dt = dt.ToStrictUtc();
            // Can't directly compare dates here because the round-trip conversion from
            // the observation DateTime to total seconds since the epoch back to
            // the DateTime given to this EciCoordinate is lossy by about 10 microseconds
            return Math.Abs((dt - Time).TotalMilliseconds) < 1 ? this : ToGeodetic().ToEci(dt);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"EciCoordinate[Position={Position}, Velocity={Velocity}]";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = 818017616;
            hashCode = hashCode * -1521134295 + Time.GetHashCode();
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Velocity.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc />
        public bool Equals(EciCoordinate other)
        {
            return base.Equals(other) && Time.Equals(other.Time) && Equals(Position, other.Position) &&
                   Equals(Velocity, other.Velocity);
        }

        /// <inheritdoc />
        public static bool operator ==(EciCoordinate left, EciCoordinate right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(EciCoordinate left, EciCoordinate right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is EciCoordinate eci && Equals(eci);
        }
    }
}