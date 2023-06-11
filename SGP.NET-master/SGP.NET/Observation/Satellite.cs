﻿using System;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using SGPdotNET.Util;

namespace SGPdotNET.Observation
{
    /// <summary>
    ///     A representation of a satellite in orbit
    /// </summary>
    public class Satellite
    {
        private readonly Sgp4 _sgp4;

        /// <summary>
        ///     The two-line element representation of the satellite
        /// </summary>
        public Tle Tle { get; }

        /// <summary>
        ///     The name of this satellite
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The orbit information of the satellite
        /// </summary>
        public Orbit Orbit => _sgp4.Orbit;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tle1">The first line of the set</param>
        /// <param name="tle2">The second line of the set</param>
        public Satellite(string tle1, string tle2) : this("Unnamed", tle1, tle2)
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name">The name of the satellite</param>
        /// <param name="tle1">The first line of the set</param>
        /// <param name="tle2">The second line of the set</param>
        public Satellite(string name, string tle1, string tle2) : this(new Tle(name, tle1, tle2))
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tle">The two-line representation of the satellite</param>
        public Satellite(Tle tle)
        {
            Tle = tle;
            _sgp4 = new Sgp4(tle);

            Name = tle.Name;
        }

        /// <summary>
        ///     Predicts the satellite's real-time location
        /// </summary>
        /// <returns>An ECI coordinate set representing the satellite</returns>
        public EciCoordinate Predict()
        {
            return Predict(DateTime.UtcNow);
        }

        /// <summary>
        ///     Predicts the satellite's location at a specific time
        /// </summary>
        /// <param name="time">The time of observation</param>
        /// <returns>An ECI coordinate set representing the satellite at the given time</returns>
        public EciCoordinate Predict(DateTime time)
        {
            time = time.ToStrictUtc();

            return _sgp4.FindPosition(time);
        }

        /// <inheritdoc />
        protected bool Equals(Satellite other)
        {
            return Tle.Equals(other.Tle);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Satellite sat && Equals(sat);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Tle.GetHashCode();
        }

        /// <inheritdoc />
        public static bool operator ==(Satellite left, Satellite right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(Satellite left, Satellite right)
        {
            return !Equals(left, right);
        }
    }
}