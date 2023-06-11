using System;
using System.Collections.Generic;
using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using SGPdotNET.Util;

namespace SGPdotNET.CoordinateSystem
{
	/// <summary>
	///     Stores a generic location
	/// </summary>
	public abstract class Coordinate
	{
		private static readonly int[] LocCharRangeAaXx = { 18, 10, 24, 10, 24, 10 };
		private static readonly int[] LocCharRangeAaYy = { 18, 10, 24, 10, 25, 10 };

		/// <summary>
		///     A coordinate that represents the geographic North Pole
		/// </summary>
		public static Coordinate NorthPole = new GeodeticCoordinate(Angle.FromDegrees(90), Angle.Zero, 0);

		/// <summary>
		///     A coordinate that represents the geographic South Pole
		/// </summary>
		public static Coordinate SouthPole = new GeodeticCoordinate(Angle.FromDegrees(-90), Angle.Zero, 0);

		/// <summary>
		///     Converts this coordinate to an ECI one
		/// </summary>
		/// <param name="dt">The time for the ECI frame</param>
		/// <returns>The coordinate in an ECI reference frame with the supplied time</returns>
		public abstract EciCoordinate ToEci(DateTime dt);

		/// <summary>
		///     Converts this coordinate to a geodetic one
		/// </summary>
		/// <returns>The coordinate in a geodetic reference frame</returns>
		public abstract GeodeticCoordinate ToGeodetic();

		/// <summary>
		///     Converts this coordinate to its Maidenhead Locator System representation, disregarding altitude
		/// </summary>
		/// <param name="precision">The precision of the conversion, which defines the number of pairs in the conversion</param>
		/// <param name="standard">The conversion standard to use for the 5th pair</param>
		/// <returns>The Maidenhead representation string</returns>
		public string ToMaidenhead(MaidenheadPrecision precision = MaidenheadPrecision.FiveKilometers,
			MaidenheadStandard standard = MaidenheadStandard.AaToXx)
		{
			var geo = ToGeodetic();
			var pairCount = (int)precision + 1;

			var locator = new char[pairCount * 2];
			int[] charRange;

			switch (standard)
			{
				case MaidenheadStandard.AaToXx:
					charRange = LocCharRangeAaXx;
					break;
				case MaidenheadStandard.AaToYy:
					charRange = LocCharRangeAaYy;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(standard), standard, null);
			}

			for (var xOrY = 0; xOrY < 2; ++xOrY)
			{
				var ordinate = xOrY == 0
					? geo.Longitude.Degrees / 2.0
					: geo.Latitude.Degrees;
				var divisions = 1;

				/* The 1e-6 here guards against floating point rounding errors */
				ordinate += 270.000001 % 180.0;
				for (var pair = 0; pair < pairCount; ++pair)
				{
					divisions *= charRange[pair];
					var squareSize = 180.0 / divisions;

					var locvalue = (char)(ordinate / squareSize);
					ordinate -= squareSize * locvalue;
					locvalue += charRange[pair] == 10 ? '0' : 'A';
					locator[pair * 2 + xOrY] = locvalue;
				}
			}

			return new string(locator);
		}

		/// <summary>
		///     Converts this coordinate to its Degrees-Minutes-Seconds (DMS) representation, disregarding altitude
		/// </summary>
		/// <returns>The Degrees-Minutes-Seconds representation string</returns>
		public string ToDegreesMinutesSeconds()
		{
			var geo = ToGeodetic();

			var north = geo.Latitude > Angle.Zero;
			var east = geo.Longitude > Angle.Zero;

			var latd = Angle.FromDegrees(Math.Abs(geo.Latitude.Degrees));
			var lond = Angle.FromDegrees(Math.Abs(geo.Longitude.Degrees));

			return $"{latd.ToDegreesMinutesSeconds()}\"{(north ? "N" : "S")} {lond.ToDegreesMinutesSeconds()}\"{(east ? "E" : "W")}";
		}

		/// <summary>
		///     Converts this coordinate to an ECEF one, assuming a spherical earth
		/// </summary>
		/// <returns>A spherical ECEF coordinate vector</returns>
		public Vector3 ToSphericalEcef()
		{
			var geo = ToGeodetic();
			return new Vector3(
				Math.Cos(geo.Latitude.Radians) * Math.Cos(-geo.Longitude.Radians + Math.PI) *
				(geo.Altitude + SgpConstants.EarthRadiusKm),
				Math.Sin(geo.Latitude.Radians) * (geo.Altitude + SgpConstants.EarthRadiusKm),
				Math.Cos(geo.Latitude.Radians) * Math.Sin(-geo.Longitude.Radians + Math.PI) *
				(geo.Altitude + SgpConstants.EarthRadiusKm)
			);
		}

		/// <summary>
		///     Calculates the visibility radius (km) of the satellite by which any distances from this coordinate less than the
		///     radius are able to see this coordinate
		/// </summary>
		/// <returns>The visibility radius, in kilometers</returns>
		public double GetFootprint()
		{
			return GetFootprintAngle().Radians * SgpConstants.EarthRadiusKm;
		}

		/// <summary>
		///     Calculates the visibility radius (radians) of the satellite by which any distances from this coordinate less than
		///     the
		///     radius are able to see this coordinate
		/// </summary>
		/// <returns>The visibility radius as an angle across Earth's surface</returns>
		public Angle GetFootprintAngle()
		{
			var geo = ToGeodetic();
			return Angle.FromRadians(Math.Acos(SgpConstants.EarthRadiusKm / (SgpConstants.EarthRadiusKm + geo.Altitude)));
		}

		/// <summary>
		///     Gets a list of geodetic coordinates which define the bounds of the visibility footprint at a specific time
		/// </summary>
		/// <param name="numPoints">The number of points in the resulting circle</param>
		/// <returns>A list of geodetic coordinates for the specified time</returns>
		public List<GeodeticCoordinate> GetFootprintBoundary(int numPoints = 60)
		{
			var center = ToGeodetic();
			var coords = new List<GeodeticCoordinate>();

			var lat = center.Latitude;
			var lon = center.Longitude;
			var d = center.GetFootprintAngle().Radians;

			for (var i = 0; i < numPoints; i++)
			{
				var perc = i / (float)numPoints * 2 * Math.PI;

				var latRadians = Math.Asin(Math.Sin(lat.Radians) * Math.Cos(d) +
				                           Math.Cos(lat.Radians) * Math.Sin(d) * Math.Cos(perc));
				var lngRadians = lon.Radians +
				                 Math.Atan2(Math.Sin(perc) * Math.Sin(d) * Math.Cos(lat.Radians),
					                 Math.Cos(d) - Math.Sin(lat.Radians) * Math.Sin(latRadians));

				lngRadians = MathUtil.WrapNegPosPi(lngRadians);

				coords.Add(new GeodeticCoordinate(Angle.FromRadians(latRadians), Angle.FromRadians(lngRadians), 10));
			}

			return coords;
		}

		/// <summary>
		///     Calculates the Great Circle distance (km) to another coordinate
		/// </summary>
		/// <param name="to">The coordinate to measure against</param>
		/// <returns>The distance between the coordinates, in kilometers</returns>
		public double DistanceTo(Coordinate to)
		{
			return AngleTo(to).Radians * SgpConstants.EarthRadiusKm;
		}

		/// <summary>
		///     Calculates the Great Circle distance as an angle to another geodetic coordinate, ignoring altitude
		/// </summary>
		/// <param name="to">The coordinate to measure against</param>
		/// <returns>The distance between the coordinates as an angle across Earth's surface</returns>
		public Angle AngleTo(Coordinate to)
		{
			var geo = ToGeodetic();
			var toGeo = to.ToGeodetic();
			var dist = Math.Sin(geo.Latitude.Radians) * Math.Sin(toGeo.Latitude.Radians) +
			           Math.Cos(geo.Latitude.Radians) * Math.Cos(toGeo.Latitude.Radians) *
			           Math.Cos(geo.Longitude.Radians - toGeo.Longitude.Radians);
			dist = Math.Acos(dist);

			return Angle.FromRadians(dist);
		}

		/// <summary>
		///     Returns true if there is line-of-sight between this coordinate and the supplied one by checking if this coordinate
		///     is within the footprint of the other
		/// </summary>
		/// <param name="other">The coordinate to check against</param>
		/// <returns>True if there is line-of-sight between this coordinate and the supplied one</returns>
		public bool CanSee(Coordinate other)
		{
			return AngleTo(other) < other.GetFootprintAngle();
		}

		/// <summary>
		///     Calculates the look angles between this coordinate and target
		/// </summary>
		/// <param name="time">The time of observation</param>
		/// <param name="to">The coordinate to observe</param>
		/// <returns>The topocentric angles between this coordinate and another</returns>
		public TopocentricObservation Observe(Coordinate to, DateTime? time = null)
		{
			var t = DateTime.UtcNow;
			if (time.HasValue)
				t = time.Value.ToStrictUtc();

			var geo = ToGeodetic();
			var eci = to.ToEci(t);
			var self = ToEci(t);

			var rangeRate = eci.Velocity - self.Velocity;
			var range = eci.Position - self.Position;

			var theta = eci.Time.ToLocalMeanSiderealTime(geo.Longitude);

			var sinLat = Math.Sin(geo.Latitude.Radians);
			var cosLat = Math.Cos(geo.Latitude.Radians);
			var sinTheta = Math.Sin(theta);
			var cosTheta = Math.Cos(theta);

			var topS = sinLat * cosTheta * range.X
				+ sinLat * sinTheta * range.Y - cosLat * range.Z;
			var topE = -sinTheta * range.X
			           + cosTheta * range.Y;
			var topZ = cosLat * cosTheta * range.X
			           + cosLat * sinTheta * range.Y + sinLat * range.Z;
			var az = Math.Atan(-topE / topS);

			if (topS > 0.0)
				az += Math.PI;

			if (az < 0.0)
				az += 2.0 * Math.PI;

			var el = Math.Asin(topZ / range.Length);
			var rate = range.Dot(rangeRate) / range.Length;

			return new TopocentricObservation(Angle.FromRadians(az), Angle.FromRadians(el), range.Length, rate, this);
		}

		/// <inheritdoc />
		protected bool Equals(Coordinate other)
		{
			return ToGeodetic().Equals(other.ToGeodetic());
		}

		/// <inheritdoc />
		public static bool operator ==(Coordinate left, Coordinate right)
		{
			return Equals(left, right);
		}

		/// <inheritdoc />
		public static bool operator !=(Coordinate left, Coordinate right)
		{
			return !Equals(left, right);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return obj is Coordinate coord && Equals(coord);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return ToGeodetic().GetHashCode();
		}
	}
}