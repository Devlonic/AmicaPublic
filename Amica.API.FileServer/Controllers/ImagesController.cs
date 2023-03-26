//using Amica.API.Images;
using Amica.API.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amica.API.FileServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase {
        private readonly IImageManager images;

        public ImagesController(IImageManager images) {
            this.images = images;
        }

        [HttpGet]
        [Route("{id}/{scale?}")]
        public async Task<IActionResult> Get(string id, ImageScale? scale = null) {
            var file = await images.GetImageAsync(id, scale ?? ImageScale.Original);
            if ( file is null )
                return NotFound(id);

            return File(
                file.Data ?? throw new Exception(),
                ( file.ContentType ?? System.Net.Mime.MediaTypeNames.Image.Jpeg ));
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files) {
            try {
                List<string> created = new();

                foreach ( var file in files ) {
                    var ms = new MemoryStream();
                    await file.CopyToAsync(ms);

                    var result = await images.CreateImageAsync(ms.ToArray());
                    created.Add($"https://{HttpContext.Request.Host}/api/Images/{result.ID}.{result.Format.ToString().ToLower()}");
                }

                if ( created.Count == 0 )
                    return UnprocessableEntity("No images present");

                return Ok(created);
            }
            catch ( Exception e ) {
                return Problem($"Not uploaded. Error: {e}");
            }
        }
    }
}
