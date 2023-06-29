using Events.API.Models;
using Microsoft.EntityFrameworkCore;
using Sales.API.Models;
using SharedModels;
using System.Diagnostics.Eventing.Reader;

namespace Events.API.Data {
    public class EventsContext : DbContext {
        public EventsContext(DbContextOptions<EventsContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Bookmark> bookmarks { get; set; } = null!;
        public DbSet<Sales.API.Models.Sale> Sales { get; set; } = null!;
    }
}