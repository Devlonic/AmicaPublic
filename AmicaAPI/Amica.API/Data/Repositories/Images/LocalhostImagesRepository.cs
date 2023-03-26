using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Linq;
using System.Xml;

namespace Amica.API.Data.Repositories {
    public class LocalhostImagesRepository : HttpSqlRepository<Image>, IImagesRepository {
        public LocalhostImagesRepository(JsonConfig<Image> config, ILogger<Repository> logger, AmicaDbContext db, HttpClient http) : base(config, logger, db, http) {
            http.BaseAddress = new Uri(@"https://localhost:5555");
        }

        public async Task<long> SetPrimaryImagesForPostsUriAsync(ICollection<PostDTO> posts, long profile_id) {
            List<PrimaryPostImages> images;
            using ( var dbConnection = db.Database.GetDbConnection() ) {
                await dbConnection.OpenAsync();
                // loading additional data
                using ( var sql = dbConnection.CreateCommand() ) {
                    var imagesTableName = Image.GetTableName();
                    var postsTableName = Post.GetTableName();

                    sql.CommandText = @$"
                            SELECT {postsTableName}.{nameof(Post.ID)}, {imagesTableName}.{nameof(Image.Uri)} 
                            FROM {postsTableName} 
                            INNER JOIN {imagesTableName} ON {imagesTableName}.{nameof(Image.PostID)} = {postsTableName}.{nameof(Post.ID)} 
                            WHERE {postsTableName}.{nameof(Post.ID_Author)} = {profile_id} and {imagesTableName}.{nameof(Image.IsPrimary)} = 1";
                    logger.LogDebug(sql.CommandText);

                    images = new List<PrimaryPostImages>();

                    using ( var reader = await sql.ExecuteReaderAsync() ) {
                        while ( await reader.ReadAsync() ) {
                            var obj = new PrimaryPostImages() {
                                PostID = reader.GetInt64(0),
                                Uri = reader.GetString(1),
                            };
                            images.Add(obj);
                        }
                    }
                }
                await dbConnection.CloseAsync();

                foreach ( var p in posts ) {
                    p.PrimaryImage = images.Find(i => i.PostID == p.ID)?.Uri;
                }

                return images.Count;
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file) {
            return ( await UploadImagesAsync(new[] { file }) )[0];
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> files) {
            var form = new MultipartFormDataContent();
            form.Add(new StringContent("files"), "files");

            foreach ( var file in files ) {
                form.Add(new StreamContent(file.OpenReadStream()) {
                    Headers = {
                ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") {
                    Name = "files",
                    FileName = file.Name
                },
                        ContentLength = file.Length,
                        ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType)
                    }
                });
            }

            var responce = await http.PostAsync("/api/Images", form);
            if ( responce.IsSuccessStatusCode ) {
                var x = await responce.Content.ReadFromJsonAsync<List<string>>();

                return x ?? throw new Exception("Upload error");
            }

            throw new Exception("Upload error");
        }
    }
}
