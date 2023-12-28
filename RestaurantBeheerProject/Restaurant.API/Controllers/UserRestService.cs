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
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RestaurantProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRestService : Controller
{
    private readonly IUserService _userService;
    private readonly IRestaurantService _restaurantService;
    private readonly IReservationService _reservationService;
    private readonly RestaurantDbContext _context = new();
    private string route = "localhost:5138/api/UserRestService/";

    public UserRestService(IUserService userService, IRestaurantService restaurantService, IReservationService reservationService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
    }

    // USER
    [HttpGet("users/{userId}", Name = "GetUserByID")]
    public async Task<ActionResult<UserOutputDTO>> GetUserByIdAsync(int userId)
    {
        try
        {
            User user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            var userOutput = MapFromDomain.MapFromUserDomain(user);
            return Ok(userOutput);
        }
        catch (Exception ex)
        {
            return BadRequest("Error in GetUserByIdAsync: " + ex.Message);
        }
    }

    [HttpPost("users/adduser/")]
    public async Task<ActionResult<UserOutputDTO>> CreateUserAsync([FromBody] UserInputDTO user)
    {
        try
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User domainUser = MapToDomain.MapToUserDomain(user);
            var userDomainOutput = await _userService.CreateUserAsync(domainUser);
            var userOutput = MapFromDomain.MapFromUserDomain(userDomainOutput);

            return Created(route + $"users/adduser/{userOutput.Id}", userOutput);
        }
        catch (Exception ex)
        {
            return BadRequest("Error in CreateUserAsync " + ex.Message);
        }
    }

    [HttpPut("users/update/")]
    public async Task<ActionResult<UserOutputDTO>> UpdateUserAsync(int userId, UserInputDTO input)
    {
        try
        {
            User mappedInput = MapToDomain.MapToUserDomain(input);

            if (await _userService.ExistingUser(mappedInput.PhoneNumber, mappedInput.Email))
            {
                User updatedUser = await _userService.UpdateUserAsync(userId, mappedInput);
                var updatedUserOutput = MapFromDomain.MapFromUserDomain(updatedUser);
                return Ok(updatedUserOutput);
            }
            else
            {
                User newUser = await _userService.CreateUserAsync(mappedInput);
                var userOutput = MapFromDomain.MapFromUserDomain(newUser);
                return Created(route + $"users/adduser/{userOutput.Id}", userOutput);
            }


        }
        catch (Exception ex)
        {
            return BadRequest("Error in UpdateuserAsync: " + ex.Message);
        }
    }

    [HttpDelete("users/delete/{userId}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        try
        {
            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            await _userService.RemoveUserAsync(userId);
            return NoContent();

        }
        catch (Exception ex)
        {
            return BadRequest("Error in DeleteUser: " + ex.Message);
        }
    }

    // Restaurant
    [HttpGet("restaurants/filter")]
    public async Task<ActionResult<List<RestaurantOutputDTO>>> GetRestaurantsByFilterAsync(int? zipCode, string? cuisine)
    {
        try
        {
            var restaurants = await _restaurantService.GetRestaurantsByZipAndCuisineAsync(zipCode, cuisine);

            if (restaurants.Count == 0)
            {
                return NotFound("No restaurants found that correspond with the filter");
            }

            var restaurantOutput = restaurants.Select(r => MapFromDomain.MapFromRestaurantDomain(r));
            return Ok(restaurantOutput);

        }
        catch (Exception ex)
        {
            return BadRequest("Error in GetRestaurantsByFilterAsync: " + ex.Message);
        }

    }

    [HttpGet("restaurants/available")]
    public async Task<ActionResult<List<RestaurantOutputDTO>>> GetAvailableRestaurantsForDate(DateOnly date, int partySize)
    {
        try
        {
            var restaurants = await _restaurantService.GetAvailableRestaurantsForDateAsync(date, partySize);

            if (restaurants.Count == 0)
            {
                return NotFound($"No available restaurants found that can accomodate {partySize} people at {date}");
            }

            var restaurantOutput = restaurants.Select(r => MapFromDomain.MapFromRestaurantDomain(r));
            return Ok(restaurantOutput);

        }
        catch (Exception ex)
        {
            return BadRequest("Error in GetAvailableRestaurantsForDate: " + ex.Message);
        }

    }

    // RESERVATION
    [HttpGet("reservations/get/user")]
    public async Task<ActionResult<List<ReservationOutputDTO>>> GetReservationsUserForDateOrRangeAsync(int userID, DateOnly date, DateOnly optionalDate)
    {
        try
        {
            var reservations = await _reservationService.GetReservationsUserForDateOrRangeAsync(userID, date, optionalDate);

            if (reservations.Count == 0)
            {
                return BadRequest($"No reservations found for given date {date} or range {date} - {optionalDate}");
            }

            var reservationsOutput = reservations.Select(r => MapFromDomain.MapFromReservationDomain(r)).ToList();
            return Ok(reservationsOutput);
        }
        catch (Exception ex)
        {
            return BadRequest("Error in GetReservationsUserForDateOrRangeAsync : " + ex.Message);
        }
    }

    [HttpPost("reservations/addreservation")]
    public async Task<ActionResult<ReservationOutputDTO>> CreateReservationAsync([FromBody] ReservationInputDTO input)
    {
        try
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            Reservation domainReservation = MapToDomain.MapToReservationDomain(input, _context);                        // W/O ID
            Reservation domainReservationOutput = await _reservationService.CreateReservationAsync(domainReservation);  // W/ ID
            ReservationOutputDTO reservationOutput = MapFromDomain.MapFromReservationDomain(domainReservationOutput);

            return Created(route + $"reservations/addreservation/{reservationOutput.ReservationID}", reservationOutput);

        }
        catch (Exception ex)
        {
            return BadRequest("Error in CreateReservationAsync: " + ex.Message);
        }
    }

    [HttpPut("reservations/update/{reservationID}")]
    public async Task<ActionResult<ReservationOutputDTO>> UpdateReservationAsync(int reservationID, [FromBody] ReservationInputDTO input)
    {
        try
        {
            Reservation domainReservation = MapToDomain.MapToReservationDomain(input, _context);

            if (await _reservationService.ExistingReservation(domainReservation))
            {
                Reservation updatedReservation = await _reservationService.UpdateReservationAsync(reservationID, domainReservation);
                var updatedReservationOutput = MapFromDomain.MapFromReservationDomain(updatedReservation);
                return Ok(updatedReservationOutput);
            }
            else
            {
                Reservation newReservation = await _reservationService.CreateReservationAsync(domainReservation);
                ReservationOutputDTO reservationOutput = MapFromDomain.MapFromReservationDomain(newReservation);
                return Created(route + $"reservations/addreservation/{reservationOutput.ReservationID}", reservationOutput);
            }

        }
        catch (Exception ex)
        {
            return BadRequest("Error in UpdateReservationAsync: " + ex.Message);
        }
    }

    [HttpDelete("reservations/delete/{reservationID}")]
    public async Task<ActionResult> DeleteReservationAsync(int reservationID)
    {
        try
        {
            var reservation = await _reservationService.GetReservationByIDAsync(reservationID);

            if (reservation.Date < DateOnly.FromDateTime(DateTime.Now.Date))
            {
                return BadRequest("Can't delete a reservation that has passed. ");
            }
            else if (reservation.Date == DateOnly.FromDateTime(DateTime.Now.Date)
                && reservation.StartTime < TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay).AddHours(2))
            { // min 2h in adv.
                return BadRequest("Can't delete a reservation that takes place in the upcoming 2 hours. ");
            }
            else
            {
                await _reservationService.CancelReservationAsync(reservationID);
                return NoContent();
            }
        }
        catch (Exception ex)
        {
            return BadRequest("Error in DeleteReservationAsync: " + ex.Message);
        }
    }

}
