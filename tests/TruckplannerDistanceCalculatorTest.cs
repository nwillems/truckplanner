using System;
using Xunit;

using Truckplanner.Model;
using Truckplanner.Business;

namespace tests
{
    public class TruckplannerDistanceCalculatorTest
    {
        private readonly TruckplanDistanceCalculator _calculator;

        public TruckplannerDistanceCalculatorTest()
        {
            _calculator = new TruckplanDistanceCalculator();
        }

        [Fact]
        public void DoesTheRightThing()
        {
            Truck t = new Truck();
            t.LocationLog = new System.Collections.Generic.List<LocationLogEntry>() {
                new LocationLogEntry() { Longitude = 0.0f, Latitude = 0.0f, Time = new DateTime(1) },
                new LocationLogEntry() { Longitude = 3.0f, Latitude = 4.0f, Time = new DateTime(2) },
            };

            TruckPlan tp = new TruckPlan() { Start = new DateTime(0), Length = new TimeSpan(3), Truck = t };

            Assert.Equal(5.0, _calculator.calculateDistance(tp));
        }

        [Theory]
        [InlineData( 3.0,  4.0)]
        [InlineData( 3.0, -4.0)]
        [InlineData(-3.0,  4.0)]
        [InlineData(-3.0, -4.0)]
        public void HandlesAllDirections(float x, float y)
        {
            Truck t = new Truck();
            t.LocationLog = new System.Collections.Generic.List<LocationLogEntry>() {
                new LocationLogEntry() { Longitude = 0.0f, Latitude = 0.0f, Time = new DateTime(1) },
                new LocationLogEntry() { Longitude = x, Latitude = y, Time = new DateTime(2) },
            };

            TruckPlan tp = new TruckPlan() { Start = new DateTime(0), Length = new TimeSpan(3), Truck = t };

            Assert.Equal(5.0, _calculator.calculateDistance(tp));
        }

        [Fact]
        public void OnlyCalculatesBasedOnTheMatchingInterval()
        {
            Truck t = new Truck();
            t.LocationLog = new System.Collections.Generic.List<LocationLogEntry>() {
                new LocationLogEntry() { Longitude = 0.0f, Latitude = 0.0f, Time = new DateTime(10) },
                new LocationLogEntry() { Longitude = 3.0f, Latitude = 4.0f, Time = new DateTime(20) },
                new LocationLogEntry() { Longitude = 6.0f, Latitude = 8.0f, Time = new DateTime(30) },
                new LocationLogEntry() { Longitude = 9.0f, Latitude = 12.0f, Time = new DateTime(40) },
            };

            TruckPlan tp = new TruckPlan() { Start = new DateTime(19), Length = new TimeSpan(20), Truck = t };

            Assert.Equal(5.0, _calculator.calculateDistance(tp));
        }
    }
}
