namespace Amica.API.WebServer.Data.DTO {
    public class CountedCollectionDTO<Model> {
        public long Count { get; set; }
        public IEnumerable<Model> Result { get; set; }
        public CountedCollectionDTO(IEnumerable<Model> res) {
            this.Result = res;
            this.Count = res.Count();
        }
    }
}
