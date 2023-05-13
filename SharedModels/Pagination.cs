namespace SharedModels {
    public class Pagination<T> {
        public int PerPage { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } = new List<T>();

        public static Pagination<T> Paginate(T[] data, int perPage, int page) {
            var count = data.Length;
            perPage = Math.Max(1, perPage);
            var totalPages = (count + perPage - 1) / perPage;
            page = Math.Max(1, Math.Min(page, totalPages));

            return new Pagination<T> {
                Page = page,
                PerPage = perPage,
                TotalPages = totalPages,
                Data = data.Skip((page - 1) * perPage).Take(perPage).ToList(),
            };
        }
    }
}