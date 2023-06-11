﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SGPdotNET.TLE
{
    /// <inheritdoc cref="ITleProvider" />
    /// <summary>
    ///     Provides a class to retrieve TLEs from a local resource
    /// </summary>
    public class LocalTleProvider : ITleProvider
    {
        private Dictionary<int, Tle> _tles;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="threeLine">True if the TLEs contain a third, preceding name line (3le format)</param>
        /// <param name="sourceFilenames">The source that should be loaded</param>
        public LocalTleProvider(bool threeLine, params string[] sourceFilenames)
        {
            LoadTles(threeLine, sourceFilenames);
        }

        private void LoadTles(bool threeLine, IEnumerable<string> sourceFilenames)
        {
            _tles = new Dictionary<int, Tle>();
            foreach (var sourceFilename in sourceFilenames)
                using (var file = File.OpenRead(sourceFilename))
                {
                    using (var sr = new StreamReader(file))
                    {
                        var restOfFile = sr.ReadToEnd()
                            .Replace("\r\n", "\n") // normalize line endings
                            .Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries); // split into lines

                        var elementSets = Tle.ParseElements(restOfFile, threeLine);

                        // E29AHU 5-June-2023 Prevent Duplicat key add to dictinary
                        Dictionary<int, Tle> tempSet = new Dictionary<int, Tle>();
                        foreach (var elementSet in elementSets)
                        {
                            if (!tempSet.ContainsKey((int)elementSet.NoradNumber))
                            {
                                tempSet.Add((int)elementSet.NoradNumber, elementSet);
                            }
                        }

                        //var tempSet = elementSets.ToDictionary(elementSet => (int) elementSet.NoradNumber);

                        _tles = _tles.Concat(tempSet.Where(kvp => !_tles.ContainsKey(kvp.Key)))
                            .ToDictionary(x => x.Key, x => x.Value);
                    }
                }
        }

        /// <inheritdoc />
        public Tle GetTle(int satelliteId)
        {
            return !_tles.ContainsKey(satelliteId) ? null : _tles[satelliteId];
        }

        /// <inheritdoc />
        public Dictionary<int, Tle> GetTles()
        {
            return _tles;
        }
    }
}