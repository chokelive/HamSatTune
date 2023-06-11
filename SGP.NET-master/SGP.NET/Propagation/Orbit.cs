using System;
using SGPdotNET.TLE;
using SGPdotNET.Util;

namespace SGPdotNET.Propagation
{
    /// <summary>
    ///     Container for the extracted orbital elements used by the SGP4 propagator.
    /// </summary>
    public class Orbit
    {
        /// <summary>
        ///     The XMO mean anomoly
        /// </summary>
        public Angle MeanAnomoly { get; }

        /// <summary>
        ///     The XNODEO right ascension of the ascending node
        /// </summary>
        public Angle AscendingNode { get; }

        /// <summary>
        ///     The OMEGAO argument of perigree
        /// </summary>
        public Angle ArgumentPerigee { get; }

        /// <summary>
        ///     The XNO mean motion, in revolutions per day
        /// </summary>
        public double MeanMotion { get; }

        /// <summary>
        ///     The AODP recovered semi-major axis
        /// </summary>
        public double RecoveredSemiMajorAxis { get; }

        /// <summary>
        ///     The semi-major axis, in kilometers
        /// </summary>
        public double SemiMajorAxis => RecoveredSemiMajorAxis * SgpConstants.EarthRadiusKm;

        /// <summary>
        ///     The XNODP recovered mean motion
        /// </summary>
        public double RecoveredMeanMotion { get; }

        /// <summary>
        ///     The perigree, in kilometers
        /// </summary>
        public double Perigee { get; }

        /// <summary>
        ///     The apogee, in kilometers
        /// </summary>
        public double Apogee { get; }

        /// <summary>
        ///     Time, in minutes, of revolution (recovered from 2pi / RecoveredMeanMotion)
        /// </summary>
        public double Period { get; }

        /// <summary>
        ///     The epoch of the element
        /// </summary>
        public DateTime Epoch { get; }

        /// <summary>
        ///     BSTAR drag term
        /// </summary>
        public double BStar { get; }

        /// <summary>
        ///     Eccentricity
        /// </summary>
        public double Eccentricity { get; }

        /// <summary>
        ///     Inclination
        /// </summary>
        public Angle Inclination { get; }

        /// <summary>
        ///     Creates a new numerical orbital element descriptor set for the provided two-line element set
        /// </summary>
        /// <param name="tle">The set to extract numerical values from</param>
        public Orbit(Tle tle)
        {
            // extract and format tle data
            MeanAnomoly = tle.MeanAnomaly;
            AscendingNode = tle.RightAscendingNode;
            ArgumentPerigee = tle.ArgumentPerigee;
            Eccentricity = tle.Eccentricity;
            Inclination = tle.Inclination;
            MeanMotion = tle.MeanMotionRevPerDay * SgpConstants.TwoPi / SgpConstants.MinutesPerDay;
            BStar = tle.BStarDragTerm;
            Epoch = tle.Epoch;

            // recover original mean motion (xnodp) and semimajor axis (aodp) from input elements
            var a1 = Math.Pow(SgpConstants.ReciprocalOfMinutesPerTimeUnit / MeanMotion, SgpConstants.TwoThirds);
            var cosio = Math.Cos(Inclination.Radians);
            var theta2 = cosio * cosio;
            var x3Thm1 = 3.0 * theta2 - 1.0;
            var eosq = Eccentricity * Eccentricity;
            var betao2 = 1.0 - eosq;
            var betao = Math.Sqrt(betao2);
            var temp = 1.5 * SgpConstants.Ck2 * x3Thm1 / (betao * betao2);
            var del1 = temp / (a1 * a1);
            var a0 = a1 * (1.0 - del1 * (1.0 / 3.0 + del1 * (1.0 + del1 * 134.0 / 81.0)));
            var del0 = temp / (a0 * a0);

            RecoveredMeanMotion = MeanMotion / (1.0 + del0);
            RecoveredSemiMajorAxis = a0 / (1.0 - del0);

            // find perigee and period
            Perigee = (RecoveredSemiMajorAxis * (1.0 - Eccentricity) - SgpConstants.DistanceUnitsPerEarthRadii) *
                      SgpConstants.EarthRadiusKm;
            Apogee = (RecoveredSemiMajorAxis * (1.0 + Eccentricity) - SgpConstants.DistanceUnitsPerEarthRadii) *
                     SgpConstants.EarthRadiusKm;
            Period = SgpConstants.TwoPi / RecoveredMeanMotion;
        }

        /// <inheritdoc />
        protected bool Equals(Orbit other)
        {
            return MeanAnomoly.Equals(other.MeanAnomoly) && AscendingNode.Equals(other.AscendingNode) &&
                   ArgumentPerigee.Equals(other.ArgumentPerigee) && MeanMotion.Equals(other.MeanMotion) &&
                   RecoveredSemiMajorAxis.Equals(other.RecoveredSemiMajorAxis) &&
                   RecoveredMeanMotion.Equals(other.RecoveredMeanMotion) && Perigee.Equals(other.Perigee) &&
                   Apogee.Equals(other.Apogee) && Period.Equals(other.Period) && Epoch.Equals(other.Epoch) &&
                   BStar.Equals(other.BStar) && Eccentricity.Equals(other.Eccentricity) &&
                   Inclination.Equals(other.Inclination);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Orbit o && Equals(o);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = MeanAnomoly.GetHashCode();
                hashCode = (hashCode * 397) ^ AscendingNode.GetHashCode();
                hashCode = (hashCode * 397) ^ ArgumentPerigee.GetHashCode();
                hashCode = (hashCode * 397) ^ MeanMotion.GetHashCode();
                hashCode = (hashCode * 397) ^ RecoveredSemiMajorAxis.GetHashCode();
                hashCode = (hashCode * 397) ^ RecoveredMeanMotion.GetHashCode();
                hashCode = (hashCode * 397) ^ Perigee.GetHashCode();
                hashCode = (hashCode * 397) ^ Apogee.GetHashCode();
                hashCode = (hashCode * 397) ^ Period.GetHashCode();
                hashCode = (hashCode * 397) ^ Epoch.GetHashCode();
                hashCode = (hashCode * 397) ^ BStar.GetHashCode();
                hashCode = (hashCode * 397) ^ Eccentricity.GetHashCode();
                hashCode = (hashCode * 397) ^ Inclination.GetHashCode();
                return hashCode;
            }
        }

        /// <inheritdoc />
        public static bool operator ==(Orbit left, Orbit right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc />
        public static bool operator !=(Orbit left, Orbit right)
        {
            return !Equals(left, right);
        }
    }
}