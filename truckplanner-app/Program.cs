using System;
using Truckplanner.Model;

namespace Truckplanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using(var db = new TruckPlannerContext())
            {
                db.Database.EnsureCreated();

                Console.WriteLine("Creating some drivers");
                db.Add(new Driver { Name = "John Doe", Birthdate = new DateTime(1990, 4, 25) });
                db.Add(new Driver { Name = "Jane Doe", Birthdate = new DateTime(1950, 4, 25) });

                db.SaveChanges();

            }
        }
    }
}
