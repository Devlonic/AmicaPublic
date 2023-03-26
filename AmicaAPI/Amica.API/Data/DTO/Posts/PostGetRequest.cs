using Microsoft.AspNetCore.Mvc;

namespace Amica.API.WebServer.Data.DTO.Posts {
    public class PaginationGetRequest {
        [FromQuery(Name = "Page")]
        public int PageNumber { get; set; }
    }
}
