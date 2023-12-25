using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantProject.API.Exceptions;
using RestaurantProject.API.Mappers;
using RestaurantProject.API.Models.Input;
using RestaurantProject.API.Models.Output;
using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;

namespace RestaurantProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRestService : Controller {
    private readonly IUserService _userService;
    private readonly IRestaurantService _restaurantService;
    private readonly IReservationService _reservationService;

    public UserRestService(IUserService userService, IRestaurantService restaurantService, IReservationService reservationService) {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
    }

    // USER
    [HttpGet("{userId}")]
    [Route("/users/")]
    public async Task<ActionResult<UserOutputDTO>> GetUserByIdAsync(int userId) {
        User user = await _userService.GetUserByIdAsync(userId);

        if (user == null) {
            throw new NotFoundException($"User with ID {userId} not found.");
        }

        var userOutput = MapFromDomain.MapFromUserDomain(user);
        return Ok(userOutput);
    }

    [HttpPost]
    [Route("/users/adduser/")]
    public async Task<ActionResult<UserOutputDTO>> CreateUserAsync([FromBody] UserInputDTO user) {
        try {

            if (user == null) {
                throw new ArgumentNullException(nameof(user));
            }

            User domainUser = MapToDomain.MapToUserDomain(user);
            var userDomainOutput = await _userService.CreateUserAsync(domainUser);
            var userOutput = MapFromDomain.MapFromUserDomain(userDomainOutput);

            return Created("test", userOutput);
        } catch (Exception) {

            throw;
        }
    }

    [HttpPut("{userId}")]
    [Route("/users/{userId}/update")]
    public async Task<ActionResult<UserOutputDTO>> UpdateUserAsync(int userId, UserInputDTO input) {
        try {
            User mappedInput = MapToDomain.MapToUserDomain(input);
            User updatedUser = await _userService.UpdateUserAsync(userId, mappedInput);
            var updatedUserOutput = MapFromDomain.MapFromUserDomain(updatedUser);
            return Ok(updatedUserOutput);
        } catch (Exception ex) {

            throw;
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser(int userId) {
        var existingUser = await _userService.GetUserByIdAsync(userId);
        if (existingUser == null) {
            throw new NotFoundException($"User with ID {userId} not found.");
        }

        await _userService.RemoveUserAsync(userId);
        return NoContent();
    }

    // Restaurant
    [HttpGet]
    public async Task<ActionResult<List<RestaurantOutputDTO>>> GetRestaurantsByFilterAsync(int? zipCode, string? cuisine) {
        var restaurants = await _restaurantService.GetRestaurantsByZipAndCuisineAsync(zipCode, cuisine);

        if (restaurants.Count == 0) {
            return BadRequest("No restaurants found that correspond with the filter");
        }

        var restaurantOutput = restaurants.Select(r => MapFromDomain.MapFromRestaurantDomain(r));
        return Ok(restaurantOutput);
    }

    [HttpGet]
    public async Task<ActionResult<List<RestaurantOutputDTO>>> GetAvailableRestaurantsForDate(DateOnly date, int partySize) {
        var restaurants = await _restaurantService.GetAvailableRestaurantsForDateAsync(date, partySize);

        if (restaurants.Count == 0) {
            return BadRequest($"No available restaurants found that can accomodate {partySize} people at {date}");
        }
        
        var restaurantOutput = restaurants.Select(r => MapFromDomain.MapFromRestaurantDomain(r));
        return Ok(restaurantOutput);
    }

    // RESERVATION
    [HttpGet]
    public async Task<ActionResult<List<ReservationOutputDTO>>> GetReservationsUserForDateOrRangeAsync(int userID, DateOnly date, DateOnly? optionalDate) {
        var reservations = await _reservationService.GetReservationsUserForDateOrRangeAsync(userID, date, optionalDate);

        if(reservations.Count == 0) {
            return BadRequest($"No reservations found for given date {date} or range {date} - {optionalDate}");
        }

        var reservationsOutput = reservations.Select(r => MapFromDomain.MapFromReservationDomain(r));
        return Ok(reservationsOutput);
    }

    [HttpPost]
    public async Task<ActionResult<RestaurantOutputDTO>> CreateReservationAsync() {
        throw new NotImplementedException();
    }
        

}
