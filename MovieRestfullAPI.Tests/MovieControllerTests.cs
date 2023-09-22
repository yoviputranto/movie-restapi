using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieRestfullAPI.Controllers;
using MovieRestfullAPI.DTO;
using MovieRestfullAPI.Entities;

namespace MovieRestfullAPI.Tests
{
    public class MovieControllerTests
    {
        private readonly xsisContext _xsisContext;
        public static IWebHostEnvironment _environtment;
        public MovieControllerTests()
        {
            _xsisContext = A.Fake<xsisContext>();
            _environtment = A.Fake<IWebHostEnvironment>();

        }
        [Fact]
        public async Task GetAllMovies_ReturnNotNull()
        {
            //Arange
            var movie = A.Fake<List<MovieDTO>>();
            
            var controller = new MovieController(_environtment,_xsisContext);
            //Act
            var actionResult = await controller.GetAllMovies();
            // Assert
            Assert.NotNull(actionResult);
        }

    }
}