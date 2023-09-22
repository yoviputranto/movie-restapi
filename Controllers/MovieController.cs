using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Microsoft.EntityFrameworkCore;
using MovieRestfullAPI.Entities;
using MovieRestfullAPI.DTO;
using System.Net;
using static System.Net.Mime.MediaTypeNames;


namespace MovieRestfullAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private readonly xsisContext xsisContext;
        public static IWebHostEnvironment _environtment;
        public MovieController(IWebHostEnvironment environtment, xsisContext xsisContext)
        {
            _environtment = environtment;
            this.xsisContext = xsisContext;
        }

       


        // GET: api/<MovieController>
        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> GetAllMovies()
        {
            try
            {
                var List = await xsisContext.Movies.Select(
                s => new MovieDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Rating = s.Rating,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                }).ToListAsync();

                if (List.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return List;
                }
            }
            catch
            {
                return NotFound();
            }
            
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovieById(int Id)
        {
            try 
            {
                MovieDTO? movieDTO = await xsisContext.Movies.Select(s => new MovieDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Rating = s.Rating,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                }).FirstOrDefaultAsync(s => s.Id == Id);
                if (movieDTO == null)
                {
                    return NotFound();
                }
                else
                {
                    return movieDTO;
                }
            } catch {
                return NotFound();
            }
            
        }

        // POST api/<MovieController>
        [HttpPost]
        public async Task<HttpResponseMessage>InsertMovie([FromForm]MovieDTO Movie)
        {
            try {
                if (ModelState.IsValid)
                {
                    var entity = new Movie();

                    entity.Title = Movie.Title;
                    entity.Description = Movie.Description;
                    entity.Rating = Movie.Rating;


                    if (Movie.ImageFile != null)
                    {
                        if (Movie.ImageFile.Length > 0)
                        {
                            entity.Image = "\\Upload\\" + Movie.Title + "-" + Movie.ImageFile.FileName;
                            if (!Directory.Exists(_environtment.WebRootPath + "\\Upload"))
                            {
                                Directory.CreateDirectory(_environtment.WebRootPath + "\\Upload\\");
                            }
                            using (FileStream filestream = System.IO.File.Create(_environtment.WebRootPath + "\\Upload\\" + Movie.Title + "-" + Movie.ImageFile.FileName))
                            {
                                Movie.ImageFile.CopyTo(filestream);
                                filestream.Flush();
                            }
                        }
                    }

                    xsisContext.Movies.Add(entity);
                    await xsisContext.SaveChangesAsync();
                    return new HttpResponseMessage(HttpStatusCode.Created);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            catch {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            
            
        }
        
        // PUT api/<MovieController>/5
        [HttpPut("{id}")]
        public async Task <HttpResponseMessage> UpdateMovie([FromForm]MovieDTO Movie,int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await xsisContext.Movies.FirstOrDefaultAsync(s => s.Id == Id);
                    entity.Title = Movie.Title;
                    entity.Description = Movie.Description;
                    entity.Rating = Movie.Rating;
                    if (Movie.ImageFile != null)
                    {
                        if (Movie.ImageFile.Length > 0)
                        {
                            if (!Directory.Exists(_environtment.WebRootPath + "\\Upload"))
                            {
                                Directory.CreateDirectory(_environtment.WebRootPath + "\\Upload\\");
                            }
                            if (System.IO.File.Exists(_environtment.WebRootPath + entity.Image))
                            {
                                System.IO.File.Delete(_environtment.WebRootPath + entity.Image);
                            }

                            using (FileStream filestream = System.IO.File.Create(_environtment.WebRootPath + "\\Upload\\" + Movie.Title + "-" + Movie.ImageFile.FileName))
                            {
                                Movie.ImageFile.CopyTo(filestream);
                                filestream.Flush();
                            }
                            entity.Image = "\\Upload\\" + Movie.Title + "-" + Movie.ImageFile.FileName;
                        }
                    }


                    await xsisContext.SaveChangesAsync();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
                
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        public async Task <HttpResponseMessage> DeleteMovie(int Id)
        {
            try
            {
                var entity = new Movie()
                {
                    Id = Id
                };
                xsisContext.Movies.Attach(entity);
                xsisContext.Movies.Remove(entity);
                await xsisContext.SaveChangesAsync();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            
        }
    }
}
