using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, string username, Stop newStop)
        {
            var theTrip = GetTripByName(tripName, username);
            newStop.Order = theTrip.Stops.Max(s => s.Order) + 1;
            theTrip.Stops.Add(newStop);
            _context.Stops.Add(newStop);
        }

        public void AddTrip(Trip newTrip)
        {
            _context.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return _context.Trips
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database.", ex);
            }

            return null;
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database.", ex);
            }

            return null;
        }

        public IEnumerable<Trip> GetAllTripsWithStops(string username)
        {
            try
            {
                return _context.Trips
                    .Where(t => t.UserName == username)
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database.", ex);
            }

            return null;
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public Trip GetTripByName(string tripName, string username)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.UserName == username)
                .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
