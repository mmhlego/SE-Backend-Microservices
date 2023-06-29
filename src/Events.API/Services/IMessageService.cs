using Events.API.Models;
using SharedModels.Events;
namespace Events.API.Services {
    
        public interface IMessageService
        {
            List<Message> GetUserMessages(Guid userId);
            void AddMessage(Guid userId, string content, MessageTypes type);
            bool ReadMessage(Guid id);
            void addMessageToAllCustomers(string content, MessageTypes type);
        }
    
}