using Events.API.Models;

namespace Events.API.Services {
    public interface IMessageService
    {
        List<Message> GetUserMessages(Guid userId);
        void AddMessage(Guid userId, string content, MessageTypes type);
        void ReadMessage(Guid Id);
    }
}