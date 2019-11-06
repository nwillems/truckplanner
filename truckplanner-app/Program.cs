using System;
using System.Collections.Generic;
using System.Linq;
using Truckplanner.Model;
using Truckplanner.Business;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Truckplanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var db = new TruckPlannerContext())
            {
                db.Database.EnsureDeleted();
            }

            using (var db = new TruckPlannerContext())
            {
                db.Database.EnsureCreated();

                Console.WriteLine("Creating some drivers");
                var jane = new Driver { Name = "Jane Doe", Birthdate = new DateTime(1950, 4, 25) };
                db.Drivers.Add(jane);

                var monster = new Truck() { LocationLog = new List<LocationLogEntry>() };
                monster.LocationLog.AddRange(new LocationLogEntry[] {
                    new LocationLogEntry { Latitude = 2, Longitude = 0, Time = new DateTime(2018, 2, 10, 1, 0,0) },
                    new LocationLogEntry { Latitude = 2, Longitude = 1, Time = new DateTime(2018, 2, 10, 2, 0, 0) },
                    new LocationLogEntry { Latitude = 2, Longitude = 2, Time = new DateTime(2018, 2, 10, 3, 0, 0) },
                    new LocationLogEntry { Latitude = 2, Longitude = 3, Time = new DateTime(2018, 2, 10, 4, 0, 0) },
                }.AsEnumerable());
                db.Trucks.Add(monster);
                
                var tp = new TruckPlan { Driver = jane, Start = new DateTime(2018, 2, 10), Length = new TimeSpan(8, 0, 0), Truck = monster };
                db.TruckPlans.Add(tp);
                db.SaveChanges();

                tp.Driver = db.Drivers.First();
                tp.Truck = db.Trucks.First();

                db.SaveChanges();
            }

            // Actual meat and potatoes
            using (var countryService = new CountryService())
            using (var db = new TruckPlannerContext())
            {
                // Ensure we are somewhat eagerly lodaing all the datas.
                var tps = db.TruckPlans
                    .Include(tp => tp.Driver)
                    .Include(tp => tp.Truck)
                        .ThenInclude(t => t.LocationLog)
                    .ToList();

                var distanceCalculator = new TruckplanDistanceCalculator();
                // Fun hoops to jump thorugh to get async stuff to be synchronous.
                Func<LocationLogEntry, Task<bool>> ff = async (ll) => await countryService.GetCountry((ll.Latitude, ll.Longitude)) == "Germany";
                Func<LocationLogEntry, bool> germanyFilter = (llorg) => (ff(llorg)).Result;

                // drivers.Age > 50, in February 2018, in Germany select km
                DateTime dt_start = DateTime.Parse("2018-02-01"), dt_end = DateTime.Parse("2018-02-28");
                Func<Driver, bool> driverFilter = (driver) => (dt_start.Year - driver.Birthdate.Year) > 50;

                var truckplans = from tp in db.TruckPlans.AsEnumerable()
                                 where (dt_start <= tp.Start && tp.Start <= dt_end) && driverFilter(tp.Driver)
                                 select tp;
                var truckplan_distances = from tp in truckplans.AsEnumerable()
                                          select distanceCalculator.calculateDistanceFiltered(tp, germanyFilter);

                Console.WriteLine(string.Format("In total, drivers over the age of 50, in the February 2018, drove in Germany, this many kilometers: {0}", truckplan_distances.Sum()));
            }
        }
    }
}
