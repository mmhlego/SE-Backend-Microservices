using Events.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Events.API.Data {
    public class EventsContext : DbContext {
        public EventsContext(DbContextOptions<EventsContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; } = null!;
    }
}