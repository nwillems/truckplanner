﻿using System;
using System.Collections.Generic;
using System.Linq;
using Truckplanner.Model;

namespace Truckplanner
{
    namespace Business
    {
        public class TruckplanDistanceCalculator
        {
            private Func<float, float, double> distanceCalculation;

            public TruckplanDistanceCalculator()
            {
                // Possibly allow specification of distance calculation method
                distanceCalculation = (a, b) => Math.Sqrt(a * a + b * b);
            }

            public double calculateDistance(TruckPlan truckPlan)
            {
                IEnumerable<LocationLogEntry> entries = truckPlan.LocationLog;

                return calculate(entries);
            }

            public double calculateDistanceFiltered(TruckPlan truckPlan, Func<LocationLogEntry, bool> filter)
            {
                IEnumerable<LocationLogEntry> entries = from ll in truckPlan.LocationLog
                                                        where filter(ll)
                                                        select ll;
                return calculate(entries);
            }
             
            private double calculate(IEnumerable<LocationLogEntry> entries)
            {
                double result = 0.0;
                // No worries in taking the first element - since its difference in distance with the first element is 0.0(plus minus rounding)
                //  Yes, it's a waste of CPU cycles, but it's more readable
                LocationLogEntry previous = entries.FirstOrDefault();
                foreach (var ll in entries)
                {
                    result += distanceCalculation(Math.Abs(previous.Latitude - ll.Latitude), Math.Abs(previous.Longitude - ll.Longitude));
                    previous = ll;
                }

                return result;
            }
        }
    }
}
