namespace SharedModels.Events {
    public class EventBase {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        public EventBase() {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
    }
}