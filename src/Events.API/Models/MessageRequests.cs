using SharedModels.Events;
#pragma warning disable CS8618
namespace Events.API.Models {
    public class MessageRequests {

        public string Content { get; set; }
        public MessageTypes Type { get; set; }
        public Guid UserId { get; set; }

    }
}

