namespace SharedModels {
    public class Pagination<T> {
        public int PerPage { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }
}