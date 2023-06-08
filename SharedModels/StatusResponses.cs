namespace Chat.API.Models {
    public class StatusResponse {
        public string Status { get; init; } = null!; // Success , Failed
        public string? Description { get; init; } = null;

        public static StatusResponse Success = new StatusResponse { Status = "Success" };

        public static StatusResponse Failed(string message) {
            return new StatusResponse {
                Status = "Failed",
                Description = message
            };
        }
    }
}
