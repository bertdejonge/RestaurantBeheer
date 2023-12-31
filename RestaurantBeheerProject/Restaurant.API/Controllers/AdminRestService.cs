using Microsoft.AspNetCore.Mvc;
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
public class AdminRestService : Controller
{
    private readonly IUserService _userService;
    private readonly IRestaurantService _restaurantService;
    private readonly IReservationService _reservationService;
    private readonly RestaurantDbContext _context = new();
    private string route = "localhost:5138/api/AdminRestService/";
    private readonly ILogger logger;

    public AdminRestService(IUserService userService, IRestaurantService restaurantService, IReservationService reservationService, ILoggerFactory loggerFactory)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        //this.logger = loggerFactory.AddFile("AdminRestLog.txt").CreateLogger("AdminRestLogger");
    }

    
    [HttpGet("reservations/{restaurantID}")]
    public async Task<ActionResult<List<ReservationOutputDTO>>> GetReservationsRestaurantForDateOrRangeAsync(int restaurantID, DateOnly date, DateOnly optionalDate)
    {
        try
        {
            var reservations = await _reservationService.GetReservationsForRestaurantForDateOrRangeAsync(restaurantID, date, optionalDate);

            if (reservations.Count == 0)
            {
                return NotFound($"No reservations found for given restaurant, date or range {date} - {optionalDate} ");
            }

            var reservationsOutput = reservations.Select(r => MapFromDomain.MapFromReservationDomain(r)).ToList();
            return Ok(reservationsOutput);

        }
        catch (Exception ex)
        {
            return BadRequest("Error in GetReservationsRestaurantForDateOrRangeAsync: " + ex.Message);
        }
    }


    [HttpPost("restaurants/addrestaurant/")]
    public async Task<ActionResult<RestaurantOutputDTO>> CreateRestaurantAsync([FromBody] RestaurantInputDTO input)
    {
        try
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            Restaurant domainRestaurant = MapToDomain.MapToRestaurantDomain(input);
            Restaurant domainRestaurantOutput = await _restaurantService.CreateRestaurantAsync(domainRestaurant);
            RestaurantOutputDTO restaurantOutput = MapFromDomain.MapFromRestaurantDomain(domainRestaurantOutput);
            return Created(route + $"/restaurants/addrestaurant/{restaurantOutput.ID}", restaurantOutput);
        }
        catch (Exception ex)
        {
            return BadRequest("Error while creating a restaurant: " + ex.Message);
        }
    }

    [HttpPut("restaurants/update/{restaurantID}")]
    public async Task<ActionResult<RestaurantOutputDTO>> UpdateRestaurantAsync(int restaurantID, [FromBody] RestaurantUpdateInputDTO input)
    {
        try
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // Make DTO with empty list, this ensures no tables can be changed
            RestaurantInputDTO inputDTO = new() { Name = input.Name, Cuisine = input.Cuisine, Email = input.Email, Municipality = input.Municipality, PhoneNumber = input.PhoneNumber, StreetName = input.StreetName, ZipCode = input.ZipCode, HouseNumberLabel = input.HouseNumberLabel, Tables = new() };

            Restaurant domainRestaurant = MapToDomain.MapToRestaurantDomain(inputDTO);

            if (await _restaurantService.ExistsById(restaurantID)) 
            {
                Restaurant updatedRestaurant = await _restaurantService.UpdateRestaurantAsync(restaurantID, domainRestaurant);
                RestaurantOutputDTO restaurantOutput = MapFromDomain.MapFromRestaurantDomain(updatedRestaurant);
                return Ok(restaurantOutput);
            }
            else
            {
                return NotFound("No restaurant found to update");
            }

        }
        catch (Exception ex)
        {
            return BadRequest("Error while updating a restaurant: " + ex.Message);
        }
    }

    [HttpDelete("restaurants/delete/{restaurantID}")]
    public async Task<ActionResult> DeleteRestaurantAsync(int restaurantID)
    {
        try
        {
            if (restaurantID > 0) {
                if (await _restaurantService.GetRestaurantByIdAsync(restaurantID) != null) {
                    await _restaurantService.RemoveRestaurantAsync(restaurantID);
                    return NoContent();
                } else {
                    return NotFound($"No restaurant found with ID {restaurantID}");
                }
            } else {
                return BadRequest("Invalid ID. Insert an ID bigger than 0.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest("Error while deleting a restaurant: " + ex.Message);
        }
    }
}
