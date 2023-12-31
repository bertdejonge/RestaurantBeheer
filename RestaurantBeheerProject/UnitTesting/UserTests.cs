using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RestaurantProject.API.Controllers;
using RestaurantProject.API.Mappers;
using RestaurantProject.API.Models.Input;
using RestaurantProject.API.Models.Output;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Managers;
using RestaurantProject.Domain.Models;
using RestaurantProject.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting {
    public class UserTests {
        // Controller
        private UserRestService _uc;

        // Services
        private RestaurantManager _restaurantManager;
        private ReservationManager _reservationManager;
        private UserManager _userManager;

        // Repo's
        private Mock<IRestaurantRepository> _restoRepo;
        private Mock<IReservationRepository> _reserRepo;
        private Mock<IUserRepository> _userRepo;

        private ILoggerFactory logger = new LoggerFactory();

        private DateOnly curDate = DateOnly.FromDateTime(DateTime.Now.Date);
        TimeOnly curTime = TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay);

        public UserTests() {
            _restoRepo = new Mock<IRestaurantRepository>();
            _reserRepo = new Mock<IReservationRepository>();
            _userRepo = new Mock<IUserRepository>();

            _restaurantManager = new(_restoRepo.Object);
            _reservationManager = new(_reserRepo.Object);
            _userManager = new(_userRepo.Object);

            _uc = new(_userManager, _restaurantManager, _reservationManager, logger);
        }

        // GetuserByID
        [Fact]
        public async void GetUserById_InvalidID_Returns_BadRequest() {
            _userRepo.Setup(repo => repo.GetUserByIdAsync(-1))
            .Throws(new UserManagerException("ID can't be null"));

            var result = await _uc.GetUserByIdAsync(-1);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetUserById_Unused_Returns_NotFound() {
            _userRepo.Setup(repo => repo.GetUserByIdAsync(200)).ReturnsAsync(It.IsAny<User>());

            var result = await _uc.GetUserByIdAsync(200);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async void GetUserById_ValidID_Returns_Ok() {
            _userRepo.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => new User("testname", "test@test.com", "04451236", 9000, "Brakel"));

            var result = await _uc.GetUserByIdAsync(2);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // CReateUserAsync
        [Fact]
        public async void CreateUserAsync_Invalidinput_Returns_BadRequest() {
            UserInputDTO input = new() {
                Name = "username",
                Email = "username@test.com",
                Municipality = "mun",
                ZipCode = 5321,
                PhoneNumber = "2121546"
            };

            _userRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>())).Returns((User u) => null);
            var result = await _uc.CreateUserAsync(input);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void CreateUserAsync_ValdInput_Returns_CreatedResult() {
            UserInputDTO input = new() {
                Name = "username",
                Email = "username@test.com",
                Municipality = "mun",
                ZipCode = 5321,
                PhoneNumber = "2121546"
            };

            _userRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);
            var result = await _uc.CreateUserAsync(input);
            Assert.IsType<CreatedResult>(result.Result);
        }

        // Update User
        [Fact]
        public async void UpdateUserAsync_InvalidInput_ReturnsBadRequest() {
            int id = 1;

            UserUpdateInputDTO input = new() {
                Email = "username@test.com",
                Municipality = "mun",
                ZipCode = 1000000000, // 1000 - 9999
                PhoneNumber = "2121546"
            };

            var result = await _uc.UpdateUserAsync(id, input);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void UpdateUserAsync_UnusedId_ReturnsNotFoundObjectResult() {
            UserUpdateInputDTO input = new() {
                Email = "username@test.com",
                Municipality = "mun",
                ZipCode = 1012,
                PhoneNumber = "2121546",
            };

            _userRepo.Setup(r => r.GetUserByIdAsync(100)).ReturnsAsync((int id) => null);

            var result = await _uc.UpdateUserAsync(100, input);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async void UpdateUserAsync_InvalidInput_ReturnsOkResult() {
            UserUpdateInputDTO input = new() {
                Email = "username@test.com",
                Municipality = "mun",
                ZipCode = 1012,
                PhoneNumber = "2121546",
            };

            _userRepo.Setup(r => r.GetUserByIdAsync(5)).ReturnsAsync((int id) => new User("test", "test@test.com", "0001213", 1253, "mun", "str", "blaaaa"));

            var result = await _uc.UpdateUserAsync(5, input);
            Assert.IsType<OkObjectResult>(result.Result);
        }


        // DELETE USER
        [Fact]
        public async void DeleteUser_UnusedID_Returns_NotFound() {
            _userRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => null);
            var result = await _uc.DeleteUser(10000);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void DeleteUser_ValidID_Returns_NoContent() {
            _userRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => new User("test", "test@test.com", "0001213", 1253, "mun", "str", "blaaaa"));
            var result = await _uc.DeleteUser(10000);

            Assert.IsType<NoContentResult>(result);
        }


        // GET RESTO'S BY FILTER
        [Fact]
        public async void GetRestaurantsByFilter_NoInput_Returns_BadRequest() {
            _restoRepo.Setup(r => r.GetRestaurantsByZipAndCuisineAsync(null, null)).Throws(new RestaurantManagerException());

            var result = await _uc.GetRestaurantsByFilterAsync(null, null);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetRestaurantsByFilter_InvalidInput_Returns_BadRequest() {
            _restoRepo.Setup(r => r.GetRestaurantsByZipAndCuisineAsync(It.Is<int>(zip => zip >= 1000 && zip <= 9999), It.IsAny<string>())).ReturnsAsync(new List<Restaurant>());
                var result = await _uc.GetRestaurantsByFilterAsync(12, "blabla");

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetRestaurantsByFilter_UncorrespondingInput_Returns_NotFound() {
            _restoRepo.Setup(r => r.GetRestaurantsByZipAndCuisineAsync(It.Is<int>(zip => zip >= 1000 && zip <= 9999), It.IsAny<string>()))
                .ReturnsAsync(new List<Restaurant>()); 


            var result = await _uc.GetRestaurantsByFilterAsync(9000, "belgian");

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetRestaurantsByFilter_OneValidInput_Returns_OkResult() {
            _restoRepo.Setup(r => r.GetRestaurantsByZipAndCuisineAsync(
                It.IsAny<int>(),
                It.Is<string>(s => CuisineType.IsInList(s))))
                .ReturnsAsync(new List<Restaurant>() {
            new Restaurant("'t blauw kotje", "Gent", 9000, "belgian", "test@test.be", "01454231", new())
                });

            var result = await _uc.GetRestaurantsByFilterAsync(0, "belgian");

            Assert.IsType<OkObjectResult>(result.Result);
        }


        [Fact]
        public async void GetRestaurantsByFilter_TwoValidInput_Returns_OkResult() {
            _restoRepo.Setup(r => r.GetRestaurantsByZipAndCuisineAsync(It.Is<int>(zip => zip >= 1000 && zip <= 9999), It.IsAny<string>()))
                .ReturnsAsync(new List<Restaurant>() {
                    new Restaurant("'t blauw kotje", "Gent", 9000, "belgian", "test@test.be", "01454231", new())
                });


            var result = await _uc.GetRestaurantsByFilterAsync(9000, "belgian");

            Assert.IsType<OkObjectResult>(result.Result);
        }

        // GET AVAILABLE RESTO'S
        [Fact]
        public async void GetAvailableRestaurants_InvalidInput_Returns_BadRequest() {
            _restoRepo.Setup(r => r.GetAvailableRestaurantsForDateAsync(It.Is<DateOnly>(d => d >= curDate 
                                                                            && d <= curDate.AddMonths(3)), It.Is<int>(i => i >=  1)))
                                                                            .ReturnsAsync((DateOnly date, int i) => new List<Restaurant>());

            // Will only succeed if Date and partysize are correct
            var result = await _uc.GetAvailableRestaurantsForDate(new DateOnly(100, 8, 12), -1);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetAvailableRestaurants_UncorrespondingInput_Returns_NotFound() {
            _restoRepo.Setup(r => r.GetAvailableRestaurantsForDateAsync(It.Is<DateOnly>(d => d >= curDate
                                                                             && d <= curDate.AddMonths(3)), It.Is<int>(i => i >= 1)))
                                                                             .ReturnsAsync((DateOnly date, int i) => new List<Restaurant>());

            // Will only succeed if Date and partysize are correct
            var result = await _uc.GetAvailableRestaurantsForDate(new DateOnly(2024,01,15), 4);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAvailableRestaurants_ValidInput_Returns_OkResult() {
            _restoRepo.Setup(r => r.GetAvailableRestaurantsForDateAsync(It.Is<DateOnly>(d => d >= curDate
                                                                             && d <= curDate.AddMonths(3)), It.Is<int>(i => i >= 1)))
                                                                             .ReturnsAsync((DateOnly date, int i) => new List<Restaurant>() { 
                                                                                 new Restaurant("'t blauw kotje", "Gent", 9000, "belgian", "test@test.be", "01454231", new()) });

            // Will only succeed if Date and partysize are correct
            var result = await _uc.GetAvailableRestaurantsForDate(new DateOnly(2024, 01, 15), 4);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Create Reservations
        [Fact]
        public async void CreateReservationAsync_InvalidInput_Returns_Badrequest() {
            ReservationInputDTO input = new() {
                RestaurantID = 11,
                UserID = 5,
                Date = new DateOnly(2024, 01, 15),
                PartySize = 4,
                StartTime = new(16, 30) // Start times between 17:00-22:00
            };

            _reserRepo.Setup(r => r.CreateReservationAsync(
                It.Is<Reservation>(r => r.Restaurant.RestaurantID > 1 && r.User.UserID > 0
                    && r.Date >= curDate
                    && r.Date <= curDate.AddMonths(3)
                    && r.PartySize >= 1
                    && r.StartTime >= new TimeOnly(17, 00)
                    && r.StartTime < new TimeOnly(22, 00))
                ))
                .ReturnsAsync((Reservation r) => r); // Use ReturnsAsync for asynchronous methods

            var result = await _uc.CreateReservationAsync(input);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }



        [Fact]
        public async void CreateReservationAsync_ValidInput_Returns_OkObject() {
            ReservationInputDTO input = new() {
                RestaurantID = 3,
                UserID = 11,
                Date = new DateOnly(2024, 01, 15),
                PartySize = 4,
                StartTime = new(17, 30, 00)
            }; // Starttimes between 17.00-22.00

            _reserRepo.Setup(r => r.CreateReservationAsync(It.IsAny<Reservation>()))
                .ReturnsAsync((Reservation r) => r);

            var result = await _uc.CreateReservationAsync(input);
            Assert.IsType<CreatedResult>(result.Result);
        }

        // UPDATE RESERVATION
        [Fact]
        public async void UpdateReservationAsync_InvalidInput_Returns_BadRequest() {
            int reservationID = 16;

            ReservationInputDTO updateInput = new ReservationInputDTO() {
                RestaurantID = -11,     // Wrong ID
                UserID = 5,
                Date = new(2024, 01, 16),
                PartySize = 4,
                StartTime = new(18, 30)
            };
            var result = await _uc.UpdateReservationAsync(reservationID, updateInput);
            Assert.IsType<BadRequestObjectResult>(result.Result);

        }

        [Fact]
        public async void UpdateReservationAsync_UnusedID_Returns_NotFound() {
            int reservationID = 11;
            ReservationInputDTO updateInput = new ReservationInputDTO() { 
                RestaurantID = 3, 
                UserID = 13, 
                Date = new(2024, 01, 16), 
                PartySize = 4, 
                StartTime = new(18, 30) 
            };

            _reserRepo.Setup(r => r.ExistingReservationByID(reservationID)).ReturnsAsync(false);
            var result = await _uc.UpdateReservationAsync(reservationID, updateInput);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async void UpdateReservationAsync_UsedInput_Returns_OkObject() {
            int reservationID = 7;
            ReservationInputDTO updateInput = new ReservationInputDTO() {
                RestaurantID = 3,
                UserID = 13,
                Date = new(2024, 01, 16),
                PartySize = 4,
                StartTime = new(18, 30)
            };

            _reserRepo.Setup(r => r.ExistingReservationByID(reservationID)).ReturnsAsync(true);
            _reserRepo.Setup(r => r.UpdateReservationAsync(reservationID, It.IsAny<Reservation>()))
                 .ReturnsAsync((int id, Reservation r) => r);

            var result = await _uc.UpdateReservationAsync(reservationID, updateInput);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void DeleteReservation_InvalidID_Returns_BadRequest() {
            var result = await _uc.DeleteReservationAsync(-1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteReservation_UnusedID_Returns_NotFound() {
            int unusedID = 1000;

            _reserRepo.Setup(r => r.GetReservationByIDAsync(unusedID)).ReturnsAsync((int id) => null);
            var result = await _uc.DeleteReservationAsync(unusedID);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void DeleteReservation_ValidID_ButPassedDate_Returns_BadRequest() {
            int ID = 10;

            // Returns reservation with date smaller than today
            _reserRepo.Setup(r => r.GetReservationByIDAsync(ID)).ReturnsAsync((int id) => new Reservation(new Restaurant("'t blauw kotje", "Gent", 9000, "belgian", "test@test.be", "01454231", new()), new User("testname", "test@test.com", "04451236", 9000, "Brakel"),
                                                                /*wrong date*/  4, new(2022, 10, 12), new(17, 30)));



            var result = await _uc.DeleteReservationAsync(ID);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteReservation_ValidID_CorrectDate_Returns_NoContent() {
            int ID = 10;

            // Returns reservation with date smaller than today
            _reserRepo.Setup(r => r.GetReservationByIDAsync(ID)).ReturnsAsync((int id) =>
                                            new Reservation(new Restaurant("'t blauw kotje", "Gent", 9000, "belgian", "test@test.be", "01454231", new()),
                                            new User("testname", "test@test.com", "04451236", 9000, "Brakel"),
                                            4, new(2024, 01, 20), new(17, 30)));


            var result = await _uc.DeleteReservationAsync(ID);

            Assert.IsType<NoContentResult>(result);
        }
    }
 }