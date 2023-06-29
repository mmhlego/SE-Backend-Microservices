using Events.API.Data;
using Events.API.Models;
using SharedModels;

namespace Events.API.Services {
    public class MessageService : IMessageService {
        private readonly EventsContext _context;
        public MessageService(EventsContext context) {
            _context = context;
        }
        public List<Message> GetUserMessages(Guid userId) {
            // if (userId == Guid.Empty)
            // throw new ArgumentNullException("Invalid user id");
            // return new List<Message>();

            return _context.Messages.Where(x => x.UserId == userId).ToList();
        }

        public void AddMessage(Guid userId, string content, MessageTypes type) {
            // if (userId == Guid.Empty)
            //     throw new ArgumentNullException("Invalid user id");

            // if (string.IsNullOrWhiteSpace(content))
            //     throw new ArgumentNullException("Content cannot be empty");

            var message = new Message {
                UserId = userId,
                Content = content,
                Type = type
            };

            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public bool ReadMessage(Guid id) {
            // if (Id == Guid.Empty)
            //     throw new ArgumentNullException("Invalid message id");

            var message = _context.Messages.FirstOrDefault(x => x.Id == id);
            if (message == null)
                return false;
            // throw new ArgumentNullException("Message doesn't exist");

            message.IsRead = true;
            _context.Messages.Update(message);
            _context.SaveChanges();
            return true;
        }
    }
}