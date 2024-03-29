﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Truckplanner
{
    namespace Model
    {
        public class TruckPlannerContext : DbContext
        {
            public DbSet<Driver> Drivers { get; set; }
            public DbSet<TruckPlan> TruckPlans { get; set; }
            public DbSet<Truck> Trucks { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSqlite("Data source=../truckplanner.db");

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<LocationLogEntry>()
                    .HasKey(l => new { l.TruckId, l.Time });

                modelBuilder.Entity<TruckPlan>()
                    .HasOne(tp => tp.Driver)
                    .WithMany()
                    .IsRequired();

                modelBuilder.Entity<TruckPlan>()
                    .HasOne(tp => tp.Truck)
                    .WithMany()
                    .IsRequired();
            }
        }

        //TODO: Make data classes immuteable
        public class Truck
        {
            public int Id { get; set;  }

            public List<LocationLogEntry> LocationLog { get; set;  }
        }

        public class LocationLogEntry
        {
            public DateTime Time { get; set; }
            public int TruckId { get; set; }

            public float Longitude { get; set; }
            public float Latitude { get; set; }
        }

        public class TruckPlan
        {
            public int Id { get; set;  }

            public DateTime Start { get; set; }
            public TimeSpan Length { get; set; }

            public Driver Driver { get; set; }

            /* I desired to make this a propper slice of the LocationLog,
             * However, that was slightly impractical, with the times as
             * indicies into the list.
             *
             * It could be done, and probably would, if more time was allocated.
             * TODO, for version 2.
             */
            public Truck Truck { get; set; }

            [NotMapped]
            public IEnumerable<LocationLogEntry> LocationLog
            {
                get
                {
                    return (from ll in this.Truck.LocationLog
                           where ll.Time >= this.Start && ll.Time <= (this.Start.Add(this.Length))
                           select ll).AsEnumerable();
                }
            }

            public override string ToString()
            {
                return string.Format("{0}( {1}, {2}, {3}, {4}, {5})", base.ToString(), Id, Driver, Start, Length, Truck);
            }
        }

        public class Driver
        {
            
            public int Id { get; set; }

            public string Name { get; set; }
            public DateTime Birthdate { get; set; }
        }
    }
}