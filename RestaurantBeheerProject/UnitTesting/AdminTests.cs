using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Managers;
using RestaurantProject.Domain.Models;
using RestaurantProject.Domain.Exceptions;
using RestaurantProject.API.Controllers;
using RestaurantProject.API.Models.Input;
using RestaurantProject.API.Models.Output;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using RestaurantProject.Datalayer.Models;

namespace UnitTesting {
    public class AdminTests {
        // Controller
        private AdminRestService _ac;

        // Services
        private RestaurantManager _restaurantManager;
        private ReservationManager _reservationManager;
        private UserManager _userManager;

        // Repo's
        private Mock<IRestaurantRepository> _restoRepo;
        private Mock<IReservationRepository> _reserRepo;
        private Mock<IUserRepository> _userRepo;

        private ILoggerFactory logger = new LoggerFactory();

        public AdminTests() {
            _restoRepo = new Mock<IRestaurantRepository>();
            _reserRepo = new Mock<IReservationRepository>();
            _userRepo = new Mock<IUserRepository>();

            _restaurantManager = new(_restoRepo.Object);
            _reservationManager = new(_reserRepo.Object);
            _userManager = new(_userRepo.Object);

            _ac = new(_userManager, _restaurantManager, _reservationManager, logger);
        }

        // TESTS FOR GetReservationsRestaurantForDateOrRange

        [Fact]
        public async void GetReservationsRestaurantForDateOrRange_InvalidID_ReturnsBadRequest() {
            // ASSERT
            DateOnly date = new(2023, 12, 30);
            DateOnly optionalDate = new(2024, 01, 01);

            // Set that an error will be thrown when invalid ID 
            _reserRepo.Setup(repo => repo.GetReservationsRestaurantForDateOrRangeAsync(-1, date, optionalDate))
                .Throws(new ReservationManagerException("RestaurantID must be positive."));

            // Restult will be a BadRequest
            var result = await _ac.GetReservationsRestaurantForDateOrRangeAsync(-1, date, optionalDate);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        [Fact]
        public async void GetReservationsRestaurantForDateOrRange_ValidButUnusedID_ReturnsNotFound() {
            // ASSERT
            DateOnly date = new(2023, 12, 30);
            DateOnly optionalDate = new(2024, 01, 01);

            // Set that an error will be thrown when invalid ID 
            _reserRepo.Setup(repo => repo.GetReservationsRestaurantForDateOrRangeAsync(30, date, optionalDate))
                .ReturnsAsync(new List<Reservation>());

            // Restult will be a BadRequest
            var result = await _ac.GetReservationsRestaurantForDateOrRangeAsync(30, date, optionalDate);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async void GetReservationsRestaurantForDateOrRange_ValidID_ReturnsReservations() {
            // ASSERT
            DateOnly date = new(2024, 01, 08);
            DateOnly optionalDate = new(2024,01,20);

            // Sample Restaurant
            var restaurant = new Restaurant("Sample Restaurant", "Sample Municipality", 1234, "belgian", "sample@example.com", "1234567890", new List<Table>());

            // Sample User
            var user = new User("Sample User", "user@example.com", "9876543210", 5431, "Sample Municipality", "Sample Street");

            // Sample Reservations
            var reservation1 = new Reservation(restaurant, user, 4, new DateOnly(2024, 01, 10), new TimeOnly(18, 30)) {
                TableNumber = 1 
            };

            var reservation2 = new Reservation(restaurant, user, 2, new DateOnly(2024, 01, 15), new TimeOnly(19, 00)) {
                TableNumber = 2
            };


            List<Reservation> reservations = new() {reservation1, reservation2 };

            _reserRepo.Setup(repo => repo.GetReservationsRestaurantForDateOrRangeAsync(2, date, optionalDate))
                .ReturnsAsync(reservations);

            var result = await _ac.GetReservationsRestaurantForDateOrRangeAsync(2, date, optionalDate);

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<List<ReservationOutputDTO>>(((OkObjectResult)result.Result).Value);
        }

        [Fact]
        public async void GetReservationsRestaurantForDateOrRange_ValidID_InvalidStartDate_ReturnsBadRequest() {
            // ASSERT
            DateOnly date = new(2023, 12, 30);
            DateOnly optionalDate = new(2024, 01, 01);

            // Set that an error will be thrown when invalid ID 
            _reserRepo.Setup(repo => repo.GetReservationsRestaurantForDateOrRangeAsync(1, date, optionalDate))
                .Throws(new ReservationManagerException("Invalid date. Date must be between today and three months from now"));

            // Restult will be a BadRequest
            var result = await _ac.GetReservationsRestaurantForDateOrRangeAsync(-1, date, optionalDate);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetReservationsRestaurantForDateOrRange_ValidID_ValidStartDate_InvalidOptionalDate_ReturnsBadRequest() {
            // ASSERT
            DateOnly date = new(2024, 01, 12);
            DateOnly optionalDate = new(2023, 01, 01);      // Must be bigger than startdate

            // Set that an error will be thrown when invalid ID 
            _reserRepo.Setup(repo => repo.GetReservationsRestaurantForDateOrRangeAsync(1, date, optionalDate))
                .Throws(new ReservationManagerException("Invalid date. Date must be between today and three months from now"));

            // Restult will be a BadRequest
            var result = await _ac.GetReservationsRestaurantForDateOrRangeAsync(-1, date, optionalDate);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // TEST FOR CreaterestaurantAsync
        [Fact]
        public async void CreateRestaurantAsync_ValidInput_Returns_Created() {
            RestaurantInputDTO input = new() { Name = "TEst", Cuisine = "french", Email = "test@test.com", 
                                               Municipality = "test", StreetName = "teststreet", PhoneNumber = "000485123", 
                                               HouseNumberLabel = "48A", ZipCode = 9000, Tables = new List<TableInputDTO>() 
                                                                                         { new TableInputDTO() { Seats = 4, TableNumber = 1 } } };

            _restoRepo.Setup(r => r.CreateRestaurantAsync(It.IsAny<Restaurant>())).ReturnsAsync((Restaurant r) => r);
            var result = await _ac.CreateRestaurantAsync(input);
            Assert.IsType<CreatedResult>(result.Result);
        }

        [Fact]
        public async void CreateRestaurantAsync_InvalidInput_ReturnsBadRequest() {
            RestaurantInputDTO input = new() {
                Name = "TEst",
                Cuisine = "french",
                Email = "test@test.com",
                Municipality = "test",
                StreetName = "teststreet",
                PhoneNumber = "000485123",
                HouseNumberLabel = "48A",
                ZipCode = 00000,            // Must be between 1000 & 9999  
                Tables = new List<TableInputDTO>() { new TableInputDTO() { Seats = 4, TableNumber = 1 } }
            };

            _restoRepo.Setup(r => r.CreateRestaurantAsync(It.IsAny<Restaurant>())).Returns((Restaurant r) => null);
            var result = await _ac.CreateRestaurantAsync(input);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // Tests for UpdateRestaurant
        [Fact]
        public async void UpdateRestaurantAsync_ValidInput_ReturnsOkResult() {
            int restaurantID = 1;
            RestaurantUpdateInputDTO input = new RestaurantUpdateInputDTO {
                Name = "oldName",
                Cuisine = "french",
                Email = "updated@test.com",
                Municipality = "UpdatedMunicipality",
                PhoneNumber = "000123456",
                StreetName = "UpdatedStreet",
                ZipCode = 9001,
                HouseNumberLabel = "49B"
            };

            _restoRepo.Setup(r => r.ExistingRestaurantByIdAsync(restaurantID)).ReturnsAsync(true);
            _restoRepo.Setup(r => r.UpdateRestaurantAsync(restaurantID, It.IsAny<Restaurant>())).ReturnsAsync((int id, Restaurant r) => r);

            var result = await _ac.UpdateRestaurantAsync(restaurantID, input);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        
        [Fact]
        public async void UpdateRestaurantAsync_UnusedID_ReturnsNotFound() {
            int restaurantID = 30;
            RestaurantUpdateInputDTO input = new RestaurantUpdateInputDTO {
                Name = "oldName",
                Cuisine = "french",
                Email = "updated@test.com",
                Municipality = "UpdatedMunicipality",
                PhoneNumber = "000123456",
                StreetName = "UpdatedStreet",
                ZipCode = 9001,
                HouseNumberLabel = "49B"
            };

            _restoRepo.Setup(r => r.ExistingRestaurantByIdAsync(restaurantID)).ReturnsAsync(false);

            var result = await _ac.UpdateRestaurantAsync(restaurantID, input);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async void UpdateRestaurantAsync_InvalidInput_ReturnsBadRequest() {
            int restaurantID = 1;
            RestaurantUpdateInputDTO input = new RestaurantUpdateInputDTO {
                Name = "oldName",
                Cuisine = "french",
                Email = "updated@test.com",
                Municipality = "UpdatedMunicipality",
                PhoneNumber = "000123456",
                StreetName = "UpdatedStreet",
                ZipCode = 100000,                // Invalid zipcode
                HouseNumberLabel = "49B"
            };

            var result = await _ac.UpdateRestaurantAsync(restaurantID, input);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // DeleteRestaurantTests
        [Fact]
        public async void DeleteRestaurant_InvalidID_Returns_BadReques() {
            var result = await _ac.DeleteRestaurantAsync(-1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteRestaurant_UnusedID_Returns_NotFound() {

            _restoRepo.Setup(r => r.GetRestaurantByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => null);   
            var result = await _ac.DeleteRestaurantAsync(30);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void DeleteRestaurant_ValidID_Returns_NoContent() {
            _restoRepo.Setup(r => r.GetRestaurantByIdAsync(5)).ReturnsAsync((int id) => new Restaurant("test", "test",8500, "french", "french@test.be", "0004481", new List<Table>()));

            var result = await _ac.DeleteRestaurantAsync(5);
            Assert.IsType<NoContentResult>(result);
        }
    }

    
}